using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using apiAgenda.Credential.Helper;
using Google.Apis.Calendar.v3.Data;

namespace OndaMental
{
    public partial class individual_agendaPSI : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                if (Session["logado"] != "Sim")
                {
                    Response.Redirect("Login.aspx");
                }
                if (Session["perfil"] == null || !Session["perfil"].ToString().Equals("Psicologo", StringComparison.OrdinalIgnoreCase))
                {
            
                    Response.Redirect("index.aspx");
                }

                criarAgenda();

            }

        }
        public class DataEvento
        {
            public string Data { get; set; }
            public int QuantidadeEventos { get; set; }
            public bool TemEvento { get; set; }
        }


        private async void criarAgenda()
        {
            int psicologoId = Convert.ToInt32(Session["utilizadorID"]);
            var tokens = await ObterTokenBD(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                var events = await EventsHelper.GetEventsGoogleCalendar(tokens.accessToken, tokens.refreshToken, psicologoId);

                var eventosAgrupados = events
                    .GroupBy(e =>
                        e.Start.Date ??
                        (e.Start.DateTime.HasValue ? e.Start.DateTime.Value.Date.ToString("yyyy-MM-dd") : null))
                    .Select(group => new DataEvento
                    {
                        Data = group.Key,
                        QuantidadeEventos = group.Count(),
                        TemEvento = true
                    })
                    .ToList();

                

                var jsonEventos = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(eventosAgrupados);
                ClientScript.RegisterStartupScript(this.GetType(), "dadosEventos", "var dadosEventos = " + jsonEventos + ";", true);

            }
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



        [WebMethod]
        public static string CalcularTotalMensal(int eventosMes)
        {
            // Instancia da classe para acessar métodos não estáticos
            var pagina = new individual_agendaPSI();
            
            // Obter preço por consulta
            decimal precoPorConsulta = pagina.ObterPrecoPorConsulta();


            // Calcular total
            decimal total = eventosMes * precoPorConsulta;

            return total.ToString("C");
        }

        public decimal ObterPrecoPorConsulta()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;
            decimal precoPorConsulta = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT valor_hora FROM tb_profissionais WHERE cod_user = @CodUser";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodUser", Convert.ToInt32(Session["utilizadorID"]));
                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        precoPorConsulta = Convert.ToDecimal(result);
                    }
                }
            }

            return precoPorConsulta;
        }

    }
}