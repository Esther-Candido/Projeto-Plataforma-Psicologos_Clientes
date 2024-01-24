using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OndaMental
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["logado"] == null)
            {
                Session["logado"] = "Nao";
            }

            if (!IsPostBack)
            {
                if (Session["utilizador"] == null)
                {
                    btn_registro.Visible = true;
                    btn_login.Visible = true;
                    btn_user.Visible = false;
                    btn_logout.Visible = false;
                }
                else
                {
                    btn_registro.Visible = false;
                    btn_login.Visible = false;
                    btn_user.Visible = true;
                    btn_logout.Visible = true;
                }

                if (Session["perfil"] == null)
                {
                    Session["logado"] = "Nao";
                    Session["utilizador"] = null;
                    Session["utilizadorID"] = null;
                }
                else
                {
                    switch (Session["perfil"].ToString())
                    {
                        case "Utilizador":
                            lb_navbar_ut1.Visible = true;
                            break;
                        case "Psicologo":
                            BotaoAgenda2();
                            
                            break;
                        default:
                            break;
                    }
                }

              
            }



        }

        private void BotaoAgenda2()
        {
            bool tokenExists = false;

            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);


            string query = "SELECT COUNT(1) FROM tb_token WHERE psicologo_id = @PsicologoID";
            SqlCommand myCommand = new SqlCommand(query, myConn);

            myCommand.Parameters.AddWithValue("@PsicologoID", Session["utilizadorID"]);

            myConn.Open();

            // Se o resultado for maior que 0, significa que existe um token
            tokenExists = (int)myCommand.ExecuteScalar() > 0;

            myConn.Close();

            // Se existir um token, o botão fica invisível
            lb_navbar_ps1.Visible = !tokenExists;
            lb_navbar_ps2.Visible = tokenExists;
        }

        protected void btn_user_Click(object sender, EventArgs e)
        {
            if (Session["logado"] == null || Session["logado"].ToString() != "Sim")
            {
                Response.Redirect("Index.aspx");
            }
            else if(Session["perfil"] == null)
            {
                Session["logado"] = "Nao";
                Session["utilizador"] = null;
                Session["utilizadorID"] = null;
                Response.Redirect("Index.aspx");
            }

            switch (Session["perfil"].ToString())
            {
                case "Utilizador":
                    Response.Redirect("User.aspx");
                    break;
                case "Psicologo":
                    Response.Redirect("Psicologo.aspx");
                    break;
                case "Admin":
                    Response.Redirect("Admin.aspx");
                    break;
                default:
                    Response.Redirect("index.aspx");
                    break;
            }

        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btn_registro_Click(object sender, EventArgs e)
        {
            Response.Redirect("Selecionar_registro.aspx");
        }

        protected void btn_logout_Click(object sender, EventArgs e)
        {
            Session["logado"] = "Nao";
            Session["utilizador"] = null;
            Session["utilizadorID"] = null;
            Session["perfil"] = null;
            Response.Redirect("index.aspx");

        }


        protected void lb_navbar_ps1_Click(object sender, EventArgs e)
        {
          
                //CONFIGURAÇÃO CRIAÇAO DE AGENDA DO GOOGLE...
                string clientId = "1064229684992-fas3lf2ni6qblevb4e8r7codhejp51ac.apps.googleusercontent.com";
                string redirectUri = "https://localhost:44337/Default.aspx";
                string scopes = Google.Apis.Calendar.v3.CalendarService.Scope.Calendar;


                string authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scopes}&access_type=offline&prompt=consent";

                Response.Redirect(authorizationUrl);
            
        }
       
        protected void lb_navbar_ut1_Click(object sender, EventArgs e)
        {
            Response.Redirect("minhasconsultas_user.aspx");
        }

        protected void lb_navbar_ps2_Click(object sender, EventArgs e)
        {
            Response.Redirect("individual_agendaPSI.aspx");
        }
    }
}