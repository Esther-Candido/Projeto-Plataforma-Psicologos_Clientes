using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using apiAgenda.Credential.Helper;
using apiAgenda.Models;

namespace apiAgenda.Controllers { 


     public static class buscarTokenBD
    {
    public static (string accessToken, string refreshToken) GetTokensForPsicologo(int psicologoId)
    {
        string accessToken = "";
        string refreshToken = "";
        string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT access_token, refresh_token FROM tb_token WHERE psicologo_id = @psicologoId";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@psicologoId", psicologoId);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        accessToken = reader["access_token"].ToString();
                        refreshToken = reader["refresh_token"].ToString();
                    }
                }
            }
        }

        return (accessToken, refreshToken);
    }
}



    [RoutePrefix("api/Event")] //a url da rota tem esse caminho fixo.. entao é localhost:44350/api/Event/ e o caminho que vc deseja ir nas rotas identificadas abaixo
    public class EventController : ApiController
    {
        //EVENTO COMPLETO AGENDA (ROTA)
        /* URL: localhost:44350/api/Event/Create
         * METODO: POST
         * 
         * Passar em BODY>RAY>JSON os campos para criaçao antes de dar SEND.
         * 
         * {
                "Summary" : "titulo da agenda aqui" ,
                "Description" : "sua descrição aqui", 
                "Location" : "sua localizacao aqui(no caso nossa agenda vai ser online, entao esse campo pode remover caso queira)",
                "Start" : "2023-11-05T15:00:00", 
                "End" : "2023-11-05T16:00:00"
            }
         * Teste realizado <Postman>
         */
        [HttpPost]
        [Route("Create")]
        public async Task<IHttpActionResult> CreateEventGoogleCalendar([FromBody] GoogleCalendar request, int psicologoId)
        {
            var tokens = buscarTokenBD.GetTokensForPsicologo(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                return Ok(await EventsHelper.CreateGoogleCalendar(request, tokens.accessToken, tokens.refreshToken, psicologoId));
            }
            else
            {
                return BadRequest("Não foi possível obter os tokens de acesso para o psicólogo.");
            }

        }



        //EVENTO RAPIDO AGENDA (ROTA)
        /* URL: localhost:44350/api/Event/CreateQuick
         * METODO: POST
         * Teste realizado <Postman>
         * 
         * Passar em BODY>RAY>JSON os campos para criaçao antes de dar SEND.
         * {
                "Summary" : "titulo da agenda aqui" ,
            }
         */
        [HttpPost]
        [Route("CreateQuick")]
        public async Task<IHttpActionResult> CreateQuickEventGoogleCalendar([FromBody] GoogleQuickCalendar request, int psicologoId)
        {
            var tokens = buscarTokenBD.GetTokensForPsicologo(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                var eventCreated = await EventsHelper.CreateQuickEventGoogleCalendar(request.Summary, tokens.accessToken, tokens.refreshToken, psicologoId);
                return Ok(eventCreated);
            }
            else
            {
                return BadRequest("Não foi possível obter os tokens de acesso para o psicólogo.");
            }
        }

        //VER TODOS OS EVENTOS DA AGENDA (ROTA) 
        /* URL: localhost:44350/api/Event/GetAll
         * METODO: GET
         * Teste realizado <Postman>
         * 
         */
        [HttpGet]
        [Route("GetAll")]
        public async Task<IHttpActionResult> GetEventsGoogleCalendar(int psicologoId)
        {
            var tokens = buscarTokenBD.GetTokensForPsicologo(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                return Ok(await EventsHelper.GetEventsGoogleCalendar(tokens.accessToken, tokens.refreshToken, psicologoId));
            }
            else
            {
                return BadRequest("Não foi possível obter os tokens de acesso para o psicólogo.");
            }
        }

        //VER UM EVENTO ESPECIFICO AGENDA (ROTA) pelo ID do evento 
        /* URL: localhost:44350/api/Event/Get/iD do evento aqui  (ex: localhost:44350/api/Event/Get/a3uhjgshp9067lb3q08edhsfjs)
         * METODO: GET 
         *Teste realizado <Postman>
         *
         */
        [HttpGet]
        [Route("Get/{eventId}")]
        public async Task<IHttpActionResult> GetEventGoogleCalendar(string eventId, int psicologoId)
        {
            var tokens = buscarTokenBD.GetTokensForPsicologo(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                return Ok(await EventsHelper.GetEventGoogleCalendar(eventId, tokens.accessToken, tokens.refreshToken, psicologoId));
            }
            else
            {
                return BadRequest("Não foi possível obter os tokens de acesso para o psicólogo.");
            }
        }

        //DELETAR UM EVENTO ESPECIFICO AGENDA (ROTA)
        /* URL: localhost:44350/api/Event/Delete/Colocar o ID do evento criado aqui... 
         * Metodo: DELETE 
         * Teste realizado <Postman>
         */
        [HttpDelete]
        [Route("Delete/{eventId}")]
        public async Task<IHttpActionResult> DeleteEventGoogleCalendar(string eventId, int psicologoId)
        {
            var tokens = buscarTokenBD.GetTokensForPsicologo(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                return Ok(await EventsHelper.DeleteEventGoogleCalendar(eventId, tokens.accessToken, tokens.refreshToken, psicologoId));
            }
            else
            {
                return BadRequest("Não foi possível obter os tokens de acesso para o psicólogo.");
            }
        }

        //ATUALIZAR UM EVENTO ESPECIFICO AGENDA (ROTA)
        /* URL: localhost:44350/api/Event/Update/Colocar o ID do evento criado aqui.
         * METODO: POST
         * Teste realizado <Postman>
         * 
         */
        [HttpPost]
        [Route("Update/{eventId}")]
        public async Task<IHttpActionResult> UpdateEventGoogleCalendar(string eventId, DateTime newStart, DateTime newEnd, int psicologoId)
        {
            var tokens = buscarTokenBD.GetTokensForPsicologo(psicologoId);
            if (!string.IsNullOrEmpty(tokens.accessToken) && !string.IsNullOrEmpty(tokens.refreshToken))
            {
                return Ok(await EventsHelper.UpdateEventGoogleCalendar(eventId, newStart, newEnd, tokens.accessToken, tokens.refreshToken, psicologoId));
            }
            else
            {
                return BadRequest("Não foi possível obter os tokens de acesso para o psicólogo.");
            }
        }
    }
}
