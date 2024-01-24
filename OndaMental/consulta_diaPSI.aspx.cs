using apiAgenda.Credential.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Apis.Calendar.v3.Data;
using static OndaMental.individual_agendaPSI;
using System.Globalization;
using System.Runtime.InteropServices.ComTypes;
using apiAgenda.Controllers;
using Microsoft.Owin;

namespace OndaMental
{
    public partial class consulta_diaPSI : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["logado"] != "Sim")
                {
                    Response.Redirect("Login.aspx", false);

                }
                else if (Session["perfil"] == null || !Session["perfil"].ToString().Equals("Psicologo", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("index.aspx", false);

                }


                await BuscarEventos();
            }


        }


        private async Task BuscarEventos()
        {
            int psicologoId = Convert.ToInt32(Session["utilizadorID"]);
     
            var tokens = await ObterTokenBD(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                var events = await EventsHelper.GetEventsGoogleCalendar(tokens.accessToken, tokens.refreshToken, psicologoId);

                await CriarNoRepeater(events);
            }
        }


        public class EventoDisplay
        {
            public string IdEvento { get; set; }
            public string EventName { get; set; }
            public string EmailClient { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime FimDate { get; set; }
            public string ContentType { get; set; }
            public byte[] ImgBinario { get; set; }
            public string Url { get; set; }
        }

        private async Task CriarNoRepeater(IList<Event> events)
        {

            // Pegar o valores que estão na url, para comparar se existe evento
            Uri url = HttpContext.Current.Request.Url;

            var parametros = HttpUtility.ParseQueryString(url.Query);
            if (Convert.ToInt32(parametros["mes"]) < 10)
            {
                parametros["mes"] = "0" + parametros["mes"];
            }
            if (Convert.ToInt32(parametros["dia"]) < 10)
            {
                parametros["dia"] = "0" + parametros["dia"];
            }

            string dataString = $"{parametros["ano"]}-{parametros["mes"]}-{parametros["dia"]}";

            if (!DateTime.TryParseExact(dataString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime dataSelecionada))
            {
                return;
            }

            var eventosFiltrados = new List<EventoDisplay>();

            foreach (var e in events.Where(e =>
                         (e.Start.DateTime.HasValue && e.Start.DateTime.Value.Date == dataSelecionada) ||
                         (!e.Start.DateTime.HasValue && DateTime.Parse(e.Start.Date).Date == dataSelecionada)))
            {
                var eventoDisplay = new EventoDisplay
                {
                    IdEvento = e.Id,
                    EventName = e.Summary,
                    EmailClient = e.Attendees != null && e.Attendees.Count > 0 ? e.Attendees[0].Email : "N/A",
                    StartDate = e.Start.DateTime.HasValue ? e.Start.DateTime.Value : DateTime.Parse(e.Start.Date),
                    FimDate = e.End.DateTime.HasValue ? e.End.DateTime.Value : DateTime.Parse(e.End.Date),
                    Url = e.ConferenceData != null && e.ConferenceData.EntryPoints != null &&
                          e.ConferenceData.EntryPoints.Count > 0
                        ? e.ConferenceData.EntryPoints[0].Uri
                        : string.Empty
                };

                // obter os dados do cliente da sua base de dados
                var cliente = await ObterClientePorEmail(eventoDisplay.EmailClient);
                if (cliente != null)
                {
                    eventoDisplay.ContentType = cliente.ContentType;
                    eventoDisplay.ImgBinario = cliente.ImageBytes;
                }

                eventosFiltrados.Add(eventoDisplay);
            }

            eventosFiltrados = eventosFiltrados.OrderBy(e => e.StartDate).ToList();

            EventosRepeater.DataSource = eventosFiltrados;
            EventosRepeater.DataBind();
        }

        public class Cliente
        {
            public string ContentType { get; set; }
            public byte[] ImageBytes { get; set; }
        }

        public async Task<Cliente> ObterClientePorEmail(string email)
        {
            Cliente cliente = null;

            // String de conexão com o banco de dados
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            // Consulta SQL para buscar o cliente pelo email
            string query = "SELECT contentType, dadosBinarios FROM tb_users WHERE email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                cliente = new Cliente
                                {
                                    ContentType = reader["contentType"] as string,
                                    ImageBytes = reader["dadosBinarios"] as byte[]
                                };
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return cliente;
        }

        private async Task<(string accessToken, string refreshToken)> ObterTokenBD(int psicologoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                string query = "SELECT access_token, refresh_token FROM tb_token WHERE psicologo_id = @psicologoId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@psicologoId", psicologoId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string accessToken = reader["access_token"].ToString();
                            string refreshToken = reader["refresh_token"].ToString();
                            return (accessToken, refreshToken);
                        }
                    }
                }
            }

            // retornar valores vazios se não encontrar os tokens de auth do psicologo
            return (string.Empty, string.Empty);
        }

        protected async void EventosRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            TextBox tb_StartDate = (TextBox)e.Item.FindControl("tb_StartDate");
            DropDownList ddl = (DropDownList)e.Item.FindControl("ddlHorarios");
            Label lbl_data = (Label)e.Item.FindControl("lbl_data");
            Label lbl_horario = (Label)e.Item.FindControl("lbl_horario");
            Label lbl_fim = (Label)e.Item.FindControl("lbl_fim");
            Label lbl_fim2 = (Label)e.Item.FindControl("lbl_fim2");

            // transforma a data em dateTime
            DateTime data;
            DateTime.TryParse(lbl_data.Text, out data);
            
            switch (e.CommandName)
            {

                case "Editar":
                 
                    // Verifica os Horarios ocupados e guarda na variavel "horariosOcupados"
                    var horariosOcupados = await BuscarHorariosOcupados(data, Convert.ToInt32(Session["utilizadorID"]));

                    // Limpa a ddl que vai os horarios disponiveis
                    ddl.Items.Clear();
                    var todosHorarios = Enumerable.Range(0, 24).Select(h => TimeSpan.FromHours(h)).ToList();
                    // Exclui os horarios Ocupados
                    var horariosDisponiveis = todosHorarios.Except(horariosOcupados).ToList();

                    tb_StartDate.Text = data.ToString("yyyy-MM-dd");

                    ddl.Items.Add(lbl_horario.Text);
                    // Adiciona os horarios a ddl
                    foreach (var horario in horariosDisponiveis)
                    {
                        ddl.Items.Add(new ListItem(horario.ToString(@"hh\:mm")));
                    }
                    ddl.Text = lbl_horario.Text;
                    // Deixa invisivel as labels e Deixa visivel a DDL dos horarios
                    lbl_data.Visible = false;
                    lbl_horario.Visible = false;
                    ddl.Visible = true;
                    lbl_fim.Visible = false;

                   
                    lbl_fim2.Text = "+1hr";
                    lbl_fim2.Visible = true;


                    ToggleEditMode(e.Item, true);
                    break;

                case "Salvar":
                    lbl_data.Visible = true;
                    lbl_horario.Visible = true;
                    ddl.Visible = false;
                    lbl_fim.Visible = true;
                    lbl_fim2.Visible = false;

                    ImageButton btn_salvar = (ImageButton)e.Item.FindControl("btn_salvar");
                    var tokensSalvar = await ObterTokenBD(Convert.ToInt32(Session["utilizadorID"]));


                    // Conversão da data e hora
                    DateTime date = DateTime.ParseExact(tb_StartDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    TimeSpan time = TimeSpan.ParseExact(ddl.SelectedValue, @"hh\:mm", CultureInfo.InvariantCulture);
                    DateTime newDate = date.Add(time);
                    DateTime endDate = newDate.AddHours(1); // Adiciona 1 hora, para o termino do evento
                   

                    // Atualizar evento
                    await EventsHelper.UpdateEventGoogleCalendar(btn_salvar.CommandArgument, newDate, endDate, tokensSalvar.accessToken,
                        tokensSalvar.refreshToken, Convert.ToInt32(Session["utilizadorID"]));
                    
                    ToggleEditMode(e.Item, false);

                    // Verifica os Horarios ocupados e guarda na variavel "horariosOcupados"
                    var horariosExistentes2 = await BuscarHorariosOcupados(data, Convert.ToInt32(Session["utilizadorID"]));

                    if (horariosExistentes2.Count > 0)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        Response.Redirect("individual_agendaPSI.aspx");
                    }
                    break;


                case "Cancelar":
                    lbl_data.Visible = true;
                    lbl_horario.Visible = true;
                    ddl.Visible = false;
                    lbl_fim.Visible = true;
                    lbl_fim2.Visible = false;

                    // Lógica para cancelar a edição
                    ToggleEditMode(e.Item, false);
                    break;

                case "Excluir":
                    ImageButton btn_excluir = (ImageButton)e.Item.FindControl("btn_excluir");
                    var tokens = await ObterTokenBD(Convert.ToInt32(Session["utilizadorID"]));

                    await EventsHelper.DeleteEventGoogleCalendar(btn_excluir.CommandArgument, tokens.accessToken,
                        tokens.refreshToken, Convert.ToInt32(Session["utilizadorID"]));

                    // Verifica os Horarios ocupados e guarda na variavel "horariosOcupados"
                    var horariosExistentes = await BuscarHorariosOcupados(data, Convert.ToInt32(Session["utilizadorID"]));

                    if (horariosExistentes.Count > 0)
                    {
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        Response.Redirect("individual_agendaPSI.aspx");
                    }
                    break;
            }
        }


        private void ToggleEditMode(RepeaterItem item, bool isEdit)
        {
            item.FindControl("tb_StartDate").Visible = isEdit;
            item.FindControl("btn_salvar").Visible = isEdit;
            item.FindControl("btn_cancelar").Visible = isEdit;

            item.FindControl("btn_excluir").Visible = !isEdit;
            item.FindControl("btn_editar").Visible = !isEdit;
        }

        protected async void tb_StartDate_TextChanged(object sender, EventArgs e)
        {
            TextBox txtStartDate = (TextBox)sender;
            RepeaterItem item = (RepeaterItem)txtStartDate.NamingContainer;

            if (DateTime.TryParse(txtStartDate.Text, out DateTime selectedDateTime))
            {
                int psicologoId = Convert.ToInt32(Session["utilizadorID"]);
                var horariosOcupados = await BuscarHorariosOcupados(selectedDateTime, psicologoId);

                AtualizarDDLComHorariosDisponiveis(item, horariosOcupados);
            }
            
        }

        private void AtualizarDDLComHorariosDisponiveis(RepeaterItem item, List<TimeSpan> horariosOcupados)
        {
            DropDownList ddlHorarios = (DropDownList)item.FindControl("ddlHorarios");

            if (ddlHorarios != null)
            {

                ddlHorarios.Items.Clear();
                var todosHorarios = Enumerable.Range(0, 23 ).SelectMany(h => new TimeSpan[] { TimeSpan.FromHours(h), TimeSpan.FromHours(h).Add(TimeSpan.FromMinutes(60)) });

                var horariosDisponiveis = todosHorarios.Except(horariosOcupados).ToList();

                foreach (var horario in horariosDisponiveis)
                {
                    ddlHorarios.Items.Add(new ListItem(horario.ToString(@"hh\:mm")));
                }
            }
        }

        private async Task<List<TimeSpan>> BuscarHorariosOcupados(DateTime dataSelecionada, int psicologoId)
        {
            var tokens = await ObterTokenBD(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                var events = await EventsHelper.GetEventsGoogleCalendar(tokens.accessToken, tokens.refreshToken, psicologoId);
                return events.Where(e => e.Start.DateTime.HasValue && e.Start.DateTime.Value.Date == dataSelecionada)
                    .Select(e => e.Start.DateTime.Value.TimeOfDay)
                    .ToList();
            }
            return new List<TimeSpan>();
        }

        protected void btn_voltar_pag_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("individual_agendaPSI.aspx", false);
        }
    }
}