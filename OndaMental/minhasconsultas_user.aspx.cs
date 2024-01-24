using apiAgenda.Credential.Helper;
using Google.Apis.PeopleService.v1.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace OndaMental
{
    public partial class minhasconsultas_user : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_mensagem_cancelamento.Visible = false;
            if (!IsPostBack)
            {

                if (Session["logado"] != "Sim")
                {
                    Response.Redirect("Login.aspx");
                }
                else if(Session["perfil"] == null || !Session["perfil"].ToString().Equals("Utilizador", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect("Index.aspx");
                }

                CarregarConsultasAsync();


            }

        }

        //botao para recarregar as consultas async
        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            CarregarConsultasAsync();

        }

        //carregar as consultas que o cliente tem
        private async void CarregarConsultasAsync()
        {
            var emailCliente = Session["utilizador"].ToString(); // Email do cliente logado
       
            var consultas = await Pegar_todas_ConsultasDoUsuario(emailCliente);
            GridViewConsultas.DataSource = consultas;
            GridViewConsultas.DataBind();
        }



        /*
         * Irá pegar todos os agendamento que o cliente tem com os psicologos do ondamental, na qual tem agenda..
         */
        public async Task<List<ConsultaViewModel>> Pegar_todas_ConsultasDoUsuario(string emailCliente)
        {
            var consultasDoUsuario = new List<ConsultaViewModel>();
            var tokensPsicologos = await Pegar_Tokens_Psicologos();

            foreach (var token in tokensPsicologos)
            {
                var eventos = await EventsHelper.GetEventsGoogleCalendar(token.AccessToken, token.RefreshToken, token.PsicologoId);
                foreach (var evento in eventos.Where(evento => evento.Attendees.Any(a => a.Email == emailCliente)))
                {
                    consultasDoUsuario.Add(new ConsultaViewModel
                    {
                        IdEvento = evento.Id,
                        UrlEvento = evento.HtmlLink,  
                        EmailPsicologo = evento.Organizer?.DisplayName ?? evento.Organizer?.Email ?? "Não Disponível",
                        Titulo = evento.Summary,
                        Data = evento.Start.DateTime.HasValue ? evento.Start.DateTime.Value.ToString("dd-MM-yyyy") : evento.Start.Date, // Formata apenas a data 
                        HoraInicio = evento.Start.DateTime.HasValue ? evento.Start.DateTime.Value.ToString("HH:mm") : string.Empty,
                        HoraFim = evento.End.DateTime.HasValue ? evento.End.DateTime.Value.ToString("HH:mm") : string.Empty,
                        UrlMeet = evento.HangoutLink
                    });
                }
            }


            // Ordenar a lista por data e hora
            return consultasDoUsuario.OrderBy(c => DateTime.Parse(c.Data)).ThenBy(c => DateTime.ParseExact(c.HoraInicio, "HH:mm", CultureInfo.InvariantCulture)).ToList();
            /*
            return consultasDoUsuario;*/
        }


        /*
   * buscar todos os token dos psicologos na BD para verificar se o email do convidado (cliente) tem consulta com esses psciologos.
   */
        private async Task<List<(string AccessToken, string RefreshToken, int PsicologoId)>> Pegar_Tokens_Psicologos()
        {
            List<(string AccessToken, string RefreshToken, int PsicologoId)> tokensPsicologos = new List<(string AccessToken, string RefreshToken, int PsicologoId)>();
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                string query = "SELECT access_token, refresh_token, psicologo_id FROM tb_token";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string accessToken = reader["access_token"].ToString();
                            string refreshToken = reader["refresh_token"].ToString();
                            int psicologoId = Convert.ToInt32(reader["psicologo_id"]);
                            tokensPsicologos.Add((accessToken, refreshToken, psicologoId));
                        }
                    }
                }
            }

            return tokensPsicologos;
        }


        public class ConsultaViewModel
        {
            public string IdEvento { get; set; }
            public string UrlEvento { get; set; }
            public string EmailPsicologo { get; set; }

            public string Titulo { get; set; }
            public String Data { get; set; }

            public string HoraInicio { get; set; }
            public string HoraFim { get; set; }

            public string UrlMeet { get; set; }


        }

        //botao para enviar email ao psicologo onde deseja cancelar a consulta
        protected void btnCancelarConsulta_Click1(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string argumento = btn.CommandArgument;
            string[] args = argumento.Split(';');

            string urlEvento = args[0];
            string tituloConsulta = args[1];
            string dataConsulta = args[2];
            string horaConsulta = args[3];
            string emailPsicologo = args[4];


            //enviar o email com a URL do evento
            EnviarEmailCancelamento(urlEvento, tituloConsulta, dataConsulta, horaConsulta, emailPsicologo);

            // mostrar a label com a mensagem do envio do email
            lbl_mensagem_cancelamento.Visible = true;
            lbl_mensagem_cancelamento.Text = $"E-mail enviado com sucesso, para cancelamento da consulta com {emailPsicologo} no dia {dataConsulta} as {horaConsulta}";

            // esconder a label apos 10 seg
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideLabel",
                "setTimeout(function() { document.getElementById('" + lbl_mensagem_cancelamento.ClientID + "').style.display = 'none'; }, 5000);", true);

        }

       

        // Email de cancelamento de consulta para o psicologo
        private void EnviarEmailCancelamento(string urlEvento, string tituloConsulta, string dataConsulta, string horaConsulta, string emailPsicologo)
        {
            try
            {

                string smtpHost = ConfigurationManager.AppSettings["SMTP_HOST"];
                int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
                string smtpUser = ConfigurationManager.AppSettings["SMTP_USER"];
                string smtpPassword = ConfigurationManager.AppSettings["SMTP_PASSWORD"];

                var fromAddress = new MailAddress(smtpUser, "OndaMental");
                var toAddress = new MailAddress(emailPsicologo);
                const string subject = "Solicitação de Cancelamento de Consulta";
              
               
                string body = $"Olá,\n\n" +
                $"O cliente com o e-mail {Session["utilizador"].ToString()} solicitou o cancelamento da seguinte consulta:\n\n" +
                $"Título: {tituloConsulta}\n" +
                $"Data e Hora: {dataConsulta} às {horaConsulta}\n\n" +
                $"Para visualizar ou modificar este evento em seu Google Calendar, por favor, siga as instruções abaixo:\n\n" +
                $"1. Certifique-se de que você está logado em sua conta Google correta. Se necessário, faça login na conta associada ao seu endereço de email profissional.\n" +
                $"2. Acesse o evento do Google Calendar através do seguinte link: {urlEvento}\n\n" +
                $"Se o link direto não funcionar, você pode copiar e colar a URL em seu navegador. Lembre-se de estar logado com a conta correta no Google.\n\n" +
                $"Se houver quaisquer dúvidas ou necessidade de assistência, por favor, não hesite em entrar em contato.\n\n" +
                $"Atenciosamente,\n" +
                $"[OndaMental]";



                var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpUser, smtpPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }

               
                
            }
            catch (Exception)
            {
                // Trate a exceção, talvez registrando em um log
                Debug.WriteLine("Erro ao enviar e-mail ");
            }
        }




   

  
    }
}