using apiAgenda.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using Grpc.Core;
using System.Configuration;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using static Google.Apis.Calendar.v3.CalendarService;
using System.Runtime.InteropServices.ComTypes;
using System.Data.SqlClient;

namespace apiAgenda.Credential.Helper
{
    public class EventsHelper
    {
        const string CALENDAR_ID = "primary";


        protected EventsHelper()
        {
        }

        
        //se caso o refresh token ficar expirado, na qual dura meses ou anos, ele sera atualizado na bd..
        private static async Task AtualizarTokenBD(string newAccessToken, string newRefreshToken, int psicologoId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Comando SQL para atualizar o token
                string updateCommand = "UPDATE tb_token SET access_token = @newAccessToken, refresh_token = @newRefreshToken WHERE psicologo_id = @psicologoId";

                using (SqlCommand cmd = new SqlCommand(updateCommand, conn))
                {
                    // Adicionar os parâmetros ao comando SQL
                    cmd.Parameters.AddWithValue("@newAccessToken", newAccessToken);
                    cmd.Parameters.AddWithValue("@newRefreshToken", newRefreshToken);
                    cmd.Parameters.AddWithValue("@psicologoId", psicologoId);

                    // Abrir a conexão e executar o comando
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }



        //Realizar a conexão do email ao google agenda, realizar a liberação para ter acesso a (Editar, remover e ver eventos)
        public static async Task<CalendarService> ConnectGoogleAgenda(string accessToken, string refreshToken, int psicologoId)
        {
            var tokenResponse = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    // credenciais de acordo com o google console. Temos criado a credential auth OndaMental Web
                    ClientId = "SEU CLIENTID AQUI (IDs do cliente OAuth 2.0) ",
                    ClientSecret = "SEU CLIENTSECRET AQUI  (IDs do cliente OAuth 2.0)"
                }
            });

            var credential = new UserCredential(flow, "user", tokenResponse);

            //  atualizar o token de acesso atual se necessário. Criamos uma função para fazer essa atualizacao na BD tambem
            if (await credential.RefreshTokenAsync(CancellationToken.None))  // esse método é responsável por verificar se o token de acesso atual está expirado e, se estiver, ele usa o token de atualização (refresh token) para obter um novo token de acesso
            {
                await AtualizarTokenBD(credential.Token.AccessToken, credential.Token.RefreshToken, psicologoId);

            }



            var services = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "OndaMental Web"
            });

            return services;
        }

        //criar um evento rapido no google agenda
        public static async Task<Event> CreateQuickEventGoogleCalendar(string summary, string accessToken, string refreshToken, int psicologoId)
        {
            var services = await ConnectGoogleAgenda(accessToken, refreshToken, psicologoId);

            var requestCreate = services.Events.QuickAdd(CALENDAR_ID, summary).Execute();

            return requestCreate;
        }



        //criar um evento completo no google agenda
        public static async Task<Event> CreateGoogleCalendar(GoogleCalendar request, string accessToken, string refreshToken, int psicologoId)
        {
            var services = await ConnectGoogleAgenda(accessToken, refreshToken, psicologoId);

            //define request
            Event eventCalendar = new Event()
            {

                Summary = request.Summary,
                Location = request.Location,
                Start = new EventDateTime
                {
                    DateTime = request.Start,
                    TimeZone = "Europe/Lisbon"
                },
                End = new EventDateTime
                {
                    DateTime = request.End,
                    TimeZone = "Europe/Lisbon"
                },
                Description = request.Description,

                Attendees = request.Attendees.Select(a => new EventAttendee { Email = a.Email }).ToList(), //convidado da agenda
                // adicionar dados de conferencia para o google meet
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = Guid.NewGuid().ToString(), // identificador unico para a conferencia
                        ConferenceSolutionKey = new ConferenceSolutionKey { Type = "hangoutsMeet" }
                    }
                }
            };

            var eventRequest = services.Events.Insert(eventCalendar, CALENDAR_ID);
            eventRequest.ConferenceDataVersion = 1;
            var requestCreate = await eventRequest.ExecuteAsync();

            return requestCreate;

        }



        //Pegar todo os eventos da lista da agenda
        public static async Task<IList<Event>> GetEventsGoogleCalendar(string accessToken, string refreshToken, int psicologoId)
        {
            var services = await ConnectGoogleAgenda(accessToken, refreshToken, psicologoId);

            var events = services.Events.List(CALENDAR_ID).Execute();

            return events.Items;
        }

        //pegar um evento da agenda 
        public static async Task<Event> GetEventGoogleCalendar(string eventId, string accessToken, string refreshToken, int psicologoId)
        {
            var services = await ConnectGoogleAgenda(accessToken, refreshToken, psicologoId);

            var events = await services.Events.Get(CALENDAR_ID, eventId).ExecuteAsync();

            return events;
        }


        //Deletar um evento da agenda por ID
        public static async Task<string> DeleteEventGoogleCalendar(string eventId, string accessToken, string refreshToken, int psicologoId)
        {
            var services = await ConnectGoogleAgenda(accessToken, refreshToken, psicologoId);

            var events = await services.Events.Delete(CALENDAR_ID, eventId).ExecuteAsync();

            return events;
        }

        //Atualizar um evento da agenda por ID
        public static async Task<Event> UpdateEventGoogleCalendar(string eventId, DateTime newStart, DateTime newEnd, string accessToken, string refreshToken, int psicologoId)
        {
            var services = await ConnectGoogleAgenda(accessToken, refreshToken, psicologoId);

            // Buscar o evento existente
            var eventToUpdate = await services.Events.Get(CALENDAR_ID, eventId).ExecuteAsync();

            // Atualizar as datas do evento
            eventToUpdate.Start = new EventDateTime
            {
                DateTime = newStart,
                TimeZone = "Europe/Lisbon"
            };
            eventToUpdate.End = new EventDateTime
            {
                DateTime = newEnd,
                TimeZone = "Europe/Lisbon"
            };

            // Atualizar o evento
            var updateRequest = services.Events.Update(eventToUpdate, CALENDAR_ID, eventId);
            var updatedEvent = await updateRequest.ExecuteAsync();
          
            return updatedEvent;
        }


    }
}