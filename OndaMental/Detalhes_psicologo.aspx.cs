using apiAgenda.Models;
using apiAgenda.Credential.Helper;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Microsoft.Web.Infrastructure;
using Google.Apis.PeopleService.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
using System.Drawing;

namespace OndaMental
{
    public partial class Detalhes_psicologo : System.Web.UI.Page
    {
    

        protected void Page_Load(object sender, EventArgs e)
        {
            lblStatusAgendamento.Visible = false;

            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null && int.TryParse(Request.QueryString["id"], out int psicologoId))
                {
                    lb_mensagem.Text = "Id do Psicologo = " + psicologoId;
                    
                    CarregarHorariosDisponiveis(psicologoId, ConfigurarDropDownListComSessao);
                    CarregarInformacaoPsicologo(psicologoId); //buscar informaçao do psicologo 

                    ConfigurarDropDownListComSessao();  //carregar horários caso tenha a session preenchida

                }
            }

        }






       
        private async void CarregarHorariosDisponiveis(int psicologoId, Action afterLoadCallback = null)
        {
            List<string> diasDisponiveis = await diasdisponiveis_BD(psicologoId);
            TimeSpan horasInicio = horasinicio_BD(psicologoId);
            TimeSpan horasFim = horasfim_BD(psicologoId);

        

            // realizar a busca do token do psicologo na BD
            var (accessToken, refreshToken) = await ObterTokenBD(psicologoId);
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                Response.Redirect("Index.aspx");
                return;
            }


            IList<Google.Apis.Calendar.v3.Data.Event> eventos = await EventsHelper.GetEventsGoogleCalendar(accessToken, refreshToken, psicologoId);

            ddlDatasDisponiveis.Items.Clear();
            ddlInicio.Items.Clear();

            for (int i = 0; i < 31; i++)
            {
                DateTime data = DateTime.Today.AddDays(i);
                if (diasDisponiveis.Contains(data.DayOfWeek.ToString()))  //dias em ingles para leitura.. entao na BD tem que subir o valor da semana em ingles
                {
                    ddlDatasDisponiveis.Items.Add(new ListItem(data.ToString("dd/MM/yyyy"), data.ToString("yyyy-MM-dd")));
                }
            }
            if (ddlDatasDisponiveis.Items.Count > 0)
            {
                ddlDatasDisponiveis.SelectedIndex = 0; // Seleciona a primeira data
                string dataSelecionada = ddlDatasDisponiveis.SelectedValue;
                await CarregarHorariosDisponiveisParaData(psicologoId, dataSelecionada);
            }
            afterLoadCallback?.Invoke();
        }


        //carregar os horarios e data para quem ja tinha session preenchida
        private async void ConfigurarDropDownListComSessao()
        {
            if (Session["data_agendamento"] != null)
            {
                string dataEscolhida = Session["data_agendamento"].ToString();
                ddlDatasDisponiveis.SelectedValue = dataEscolhida;

                // Agora carregue os horários para essa data e defina o valor da hora
                if (Session["hora_agendamento"] != null)
                {
                    string horaEscolhida = Session["hora_agendamento"].ToString();
                    await CarregarHorariosDisponiveisParaData(Convert.ToInt32(Request.QueryString["id"]), dataEscolhida, () =>
                    {
                        ddlInicio.SelectedValue = horaEscolhida;
                    });
                }
            }
        }




        /*
         * Botao para chamar o evento de criação de um evento no google calendar.
         * Caso nao estiver logado, irá logar para conseguir obter sucesso na criaçao do evento
         * 
         */
        protected async void btnAgendar_Click(object sender, EventArgs e)
        {
            if (Session["logado"] != null && Session["logado"].ToString() == "Sim")
            {
                // cliente logado, poderá ter o evento criado
                criar_evento_googlecalendar();
            }
            else
            {
                // cliente nao ta logado, iremos guardar na sessao (data e a hora) e redirecionar para o login
                Session["data_agendamento"] = ddlDatasDisponiveis.SelectedValue;
                Session["hora_agendamento"] = ddlInicio.SelectedValue;
                Session["id_psicologo"] = Request.QueryString["id"];

                // Redireciona para a página de login sem abortar o thread
                Response.Redirect("Login.aspx", false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            
        }

                

        /*Função na qual vai criar um evento no google calendar
         * Verificar o token do psicologo para incluir em sua agenda
         * Mostrar apenas horario e data de acordo com a escolha do psicologo
         * 
         */
        private async void criar_evento_googlecalendar()
        {
            try
            {
                var emailCliente = Session["utilizador"].ToString();  //sessao com o email do cliente armazenado
                string dataSelecionada = ddlDatasDisponiveis.SelectedValue;
                string horaSelecionada = ddlInicio.SelectedValue;

                string inicio = dataSelecionada + "T" + horaSelecionada + ":00";
                DateTime dataInicio = DateTime.ParseExact(inicio, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                DateTime dataFim = dataInicio.AddHours(1);

                int psicologoId = Convert.ToInt32(Request.QueryString["id"]);

                // verificar se o psicologo tem tokens, caso nao retorna a msg
                var (accessToken, refreshToken) = await ObterTokenBD(psicologoId);
                if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
                {
                   Response.Redirect("Index.aspx");
                    return;
                }

                // irá criar evento na agenda do psicologo usando tokens de auth baseado no ID do psicologo
                GoogleCalendar novoEvento = new GoogleCalendar
                {
                    Summary = "Consulta Online",
                    Start = dataInicio,
                    End = dataFim,
                    Attendees = new List<EventAttendee> { new EventAttendee { Email = emailCliente } },

                };

                var eventoCriado = await EventsHelper.CreateGoogleCalendar(novoEvento, accessToken, refreshToken, psicologoId);
                if (eventoCriado != null)
                {
                    lblStatusAgendamento.Visible = true;
                    lblStatusAgendamento.Text = "Consulta agendada com sucesso!";

                    btnAgendar.Visible = false;
                    lblInicio.Visible = false;
                    lblData.Visible = false;
                    ddlInicio.Visible = false;
                    ddlDatasDisponiveis.Visible = false;

                    btn_marcarOutra.Visible = true;

                    // incluir url do meet
                    string meetUrl = eventoCriado.HangoutLink;
                }
                else
                {
                    lblStatusAgendamento.Visible = true;
                    lblStatusAgendamento.Text = "Falha ao agendar a consulta.";
                }
            }
            catch (Exception)
            {
                lblStatusAgendamento.Visible = true;
                lblStatusAgendamento.Text = "Sem horario para marcação!!";
                lblStatusAgendamento.BackColor = Color.Red;
            }

        }







       






        //busca na BD as informaçoes do psicologo de acordo com o seu ID
        private void CarregarInformacaoPsicologo(int psicologoId)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);


            string query = "SELECT u.*, p.* FROM tb_users u LEFT JOIN tb_profissionais p ON u.id = p.cod_user WHERE p.cod_user = @psicologoId";
            SqlCommand myCommand = new SqlCommand(query, myConn);
            myCommand.Parameters.AddWithValue("@psicologoId", psicologoId);
            myConn.Open();
            SqlDataReader reader = myCommand.ExecuteReader();

            while (reader.Read())
            {
               
                //foto psicologo
                if (reader["dadosBinarios"] != DBNull.Value)
                {
                    byte[] dadosBinarios = (byte[])reader["dadosBinarios"];
                    img_user.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(dadosBinarios);
                }
                else
                {
                    img_user.ImageUrl = "img/semfoto.png";
                }


                if (reader["verificado"].ToString() == "True")
                {
                    lbl_verificado.Visible = true;
                    lbl_naoverificado.Visible = false;
                }



                lbl_instaPSI.Text = reader["instagram"].ToString();
                lbl_linkPSI.Text = reader["linkedin"].ToString();
                lbl_precoPSI.Text = reader["valor_hora"].ToString();
                lbl_descrProfissionalPSI.Text = reader["descricao"].ToString();
                lbl_nomePSI.Text = reader["utilizador"].ToString();
                lbl_emailPSI.Text = reader["email"].ToString();
                lbl_cpPSI.Text = reader["cp"].ToString();
                lbl_telemovelPSI.Text = reader["telemovel"].ToString();
                lbl_descrPessoalPSI.Text = reader["sobreSi"].ToString();


            }
            reader.Close();
            myConn.Close();
        }






        private TimeSpan horasinicio_BD(int psicologoId)
        {
            // Supondo que você tenha uma conexão com o banco de dados definida em seu web.config
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT horas_inicio FROM tb_profissionais WHERE cod_user = @psicologoId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@psicologoId", psicologoId);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return TimeSpan.Parse(result.ToString());
                    }
                }
            }
            // Retornar um valor padrão se não encontrar nada 8 horas padrao
            return TimeSpan.FromHours(8); 
        }

        private TimeSpan horasfim_BD(int psicologoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT horas_fim FROM tb_profissionais WHERE cod_user = @psicologoId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@psicologoId", psicologoId);
                    var result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return TimeSpan.Parse(result.ToString());
                    }
                }
            }
            // Retornar um valor padrão se não encontrar nada 18 horas padrao
            return TimeSpan.FromHours(18); 
        }

        private async Task<List<string>> diasdisponiveis_BD(int psicologoId)
        {
            List<string> diasDisponiveis = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT dias_disponiveis FROM tb_profissionais WHERE cod_user = @psicologoId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@psicologoId", psicologoId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            string dias = reader["dias_disponiveis"].ToString();
                            diasDisponiveis = dias.Split(',').ToList();
                        }
                    }
                }
            }

            return diasDisponiveis;
        }

        protected async void ddlDatasDisponiveis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDatasDisponiveis.SelectedItem != null)
            {
                int psicologoId = Convert.ToInt32(Request.QueryString["id"]);
                string dataSelecionada = ddlDatasDisponiveis.SelectedValue;
                await CarregarHorariosDisponiveisParaData(psicologoId, dataSelecionada);
            }
        }


        private async Task CarregarHorariosDisponiveisParaData(int psicologoId, string dataSelecionada, Action afterLoadCallback = null)
        {
            DateTime dataEscolhida = DateTime.ParseExact(dataSelecionada, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            TimeSpan horasInicio = horasinicio_BD(psicologoId);
            TimeSpan horasFim = horasfim_BD(psicologoId);

            var (accessToken, refreshToken) = await ObterTokenBD(psicologoId);
            IList<Google.Apis.Calendar.v3.Data.Event> eventos = await EventsHelper.GetEventsGoogleCalendar(accessToken, refreshToken, psicologoId);

            ddlInicio.Items.Clear();

            DateTime agora = DateTime.Now;
            for (TimeSpan hora = horasInicio; hora < horasFim; hora = hora.Add(TimeSpan.FromHours(1)))
            {
                DateTime horarioCompleto = dataEscolhida + hora;

                // Se a data escolhida for hoje e a hora já tiver passado, pule para a próxima hora
                if (dataEscolhida.Date == agora.Date && horarioCompleto <= agora)
                {
                    continue;
                }

                if (eventos.All(e => e.Start.DateTime == null || e.Start.DateTime.Value != horarioCompleto))
                {
                    ddlInicio.Items.Add(new ListItem(hora.ToString(@"hh\:mm"), hora.ToString(@"hh\:mm")));
                }
            }
            afterLoadCallback?.Invoke();
        }










        //recuperar o Token na BD para buscar o auth do psicologo corretamente e registrar o evento
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

        protected void btn_marcarOutra_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btn_voltar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Index.aspx");
        }
    }

}
