using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;


namespace OndaMental
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                string clientId = "1064229684992-fas3lf2ni6qblevb4e8r7codhejp51ac.apps.googleusercontent.com";
                string clientSecret = "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";
                string redirectUri = "https://localhost:44337/Default.aspx";

                var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    },

                });

      

                var tokenResponse = codeFlow.ExchangeCodeForTokenAsync("user", code, redirectUri, CancellationToken.None).Result;

                //guardar os dados do token no psicologo authenticado
                if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.RefreshToken))
                {
                    int psicologoID = Convert.ToInt32(Session["utilizadorID"]); //pegar esse ID do psicologo 

                    // String de conexão com o banco de dados
                    string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;

                    SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
                    SqlCommand myCommand = new SqlCommand();
                    myCommand.Parameters.AddWithValue("@psicologoID", psicologoID);
                    myCommand.Parameters.AddWithValue("@accessToken", tokenResponse.AccessToken);
                    myCommand.Parameters.AddWithValue("@refreshToken", tokenResponse.RefreshToken);
                    myCommand.Parameters.AddWithValue("@token_expiracao", DateTime.Now.AddSeconds(tokenResponse.ExpiresInSeconds.Value));

                   

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.CommandText = "inserir_token";


                    myCommand.Connection = myConn;
                    myConn.Open();
                    myCommand.ExecuteNonQuery();

                    //redirecionar o psicologo para a pagina de eventos dele....
                    Response.Redirect("individual_agendaPSI.aspx");

                 
                }
            }

        }
    }

}

    


