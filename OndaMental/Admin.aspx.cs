using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;


namespace OndaMental
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Verifica a Session do site
            if (Session["logado"] == null || Session["logado"] != "Sim")
            {
                Response.Redirect("Login.aspx");

            }
            else if (Session["perfil"] == null || !Session["perfil"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("index.aspx");

            }

            //SQL para mostrar dados do Admin
            string user = (string)Session["utilizador"];
            if (!IsPostBack)
            {
                mvMainContent.ActiveViewIndex = 0;
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);

                string query = "select u.*, p.* from tb_users u left join tb_profissionais p ON u.id = p.cod_user where email= @userEmail";
                SqlCommand myCommand = new SqlCommand(query, myConn);
                myCommand.Parameters.AddWithValue("@userEmail", user);
                myConn.Open();
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    lbl_user.Text = reader["utilizador"].ToString();

                    if (reader["dadosBinarios"] != DBNull.Value)
                    {
                        byte[] dadosBinarios = (byte[])reader["dadosBinarios"];
                        img_user.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(dadosBinarios);
                    }
                    else
                    {
                        img_user.ImageUrl = "img/semfoto.png";
                    }

                    tb_user.Text = reader["utilizador"].ToString();
                    tb_nome.Text = reader["nomeCompleto"].ToString();
                    lbl_email.Text = reader["email"].ToString();
                    tb_data_nasc.Text = reader["data_nasc"].ToString();
                    tb_telemovel.Text = reader["telemovel"].ToString();
                    tb_descricao.Text = reader["sobreSi"].ToString();
                }
                reader.Close();
                myConn.Close();
            }

        }
       
        public static string EncryptString(string Message)
        {
            string Passphrase = "atec";
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string

            string enc = Convert.ToBase64String(Results);
            enc = enc.Replace("+", "KKK");
            enc = enc.Replace("/", "JJJ");
            enc = enc.Replace("\\", "III");
            return enc;
        }
        
        //View 1 e padrao
        protected void lnkDashboard_Click(object sender, EventArgs e)
        {
            mvMainContent.ActiveViewIndex = 0;
        }

       

        //View 1 Guardar Dados
        protected void btn_guardarAll_Click(object sender, EventArgs e)
        {
            string user = (string)Session["utilizador"];
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);


            byte[] existingImgBinaryData = null;
            string existingContentType = null;

            myConn.Open();
            SqlCommand cmd = new SqlCommand("SELECT dadosBinarios, contentType FROM tb_users WHERE email = @email", myConn);
            cmd.Parameters.AddWithValue("@email", user);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                if (reader["dadosBinarios"] != DBNull.Value)
                {
                    existingImgBinaryData = (byte[])reader["dadosBinarios"];
                }
                else
                {
                    existingImgBinaryData = null;
                }

                if (reader["contentType"] != DBNull.Value)
                {
                    existingContentType = reader["contentType"].ToString();
                }
                else
                {
                    existingContentType = null;
                }
            }
            myConn.Close();

            SqlCommand myCommand = new SqlCommand();
            if (FileUpload1.HasFile)
            {
                Stream imgStream = FileUpload1.PostedFile.InputStream;
                string contentType = FileUpload1.PostedFile.ContentType;
                int imgLength = FileUpload1.PostedFile.ContentLength;
                byte[] imgBinaryData = new byte[imgLength];
                imgStream.Read(imgBinaryData, 0, imgLength);

                myCommand.Parameters.AddWithValue("@ct", contentType);
                myCommand.Parameters.AddWithValue("@ficheiroBinario", imgBinaryData);
            }
            else
            {
                myCommand.Parameters.AddWithValue("@ct", existingContentType);
                myCommand.Parameters.AddWithValue("@ficheiroBinario", existingImgBinaryData);
            }

            myCommand.Parameters.AddWithValue("@email", user);
            myCommand.Parameters.AddWithValue("@user", string.IsNullOrWhiteSpace(tb_user.Text) ? (object)DBNull.Value : tb_user.Text);
           
            myCommand.Parameters.AddWithValue("@nomeCompleto", string.IsNullOrWhiteSpace(tb_nome.Text) ? (object)DBNull.Value : tb_nome.Text);
    
            myCommand.Parameters.AddWithValue("@data_nasc", string.IsNullOrWhiteSpace(tb_data_nasc.Text) ? (object)DBNull.Value : tb_data_nasc.Text);
            myCommand.Parameters.AddWithValue("@telemovel", string.IsNullOrWhiteSpace(tb_telemovel.Text) ? (object)DBNull.Value : tb_telemovel.Text);
           
            myCommand.Parameters.AddWithValue("@sobreSi", string.IsNullOrWhiteSpace(tb_descricao.Text) ? (object)DBNull.Value : tb_descricao.Text);

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "update_admin";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();
            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            if (resposta == 1)
            {
                Response.Redirect("Admin.aspx");
                lbl_msg.Text = "Dados Atualizados";

            }
            else
            {
                lbl_msg.Text = "Erro ao Atualizar dados";

            }
            myConn.Close();
        }

        //View 2 users/psicologos
        protected void lnkUsers_Click(object sender, EventArgs e)
        {
            // Mostrar a visualização de Users
            mvMainContent.ActiveViewIndex = 1;

            string query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil,CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user where u.cod_perfil LIKE '%" + 1 + "%'";
            
            sql_all(query);
            
        }
        
        //Class users/psicologos View2
        public class pauta_notas
        {

            public string utilizador { get; set; }
            public string email { get; set; }
            public string perfil { get; set; }
            public Boolean ativo { get; set; }
            public Boolean? verificado { get; set; }

        }

        //SQL da View 2 users/psicologos
        public void sql_all(string query)
        { 
            List<pauta_notas> lst_pauta = new List<pauta_notas>();
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand(query, myConn);
            myConn.Open();
            SqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                pauta_notas notaObj = new pauta_notas();
                notaObj.utilizador = reader.GetString(0);
                notaObj.email = reader.GetString(1);
                notaObj.ativo = reader.GetBoolean(2);             
                notaObj.perfil = reader.GetString(4);
                notaObj.verificado = reader.IsDBNull(5) ? (bool?)null : reader.GetBoolean(5);
                
                lst_pauta.Add(notaObj);
            }
            reader.Close();
            myConn.Close();
            rpt_users.DataSource = lst_pauta;
            rpt_users.DataBind();
        }

        //Botao filtrar view View 2 users/psicologos
        protected void btn_filtrar_Click(object sender, EventArgs e)
        {
            string query;

            if (string.IsNullOrEmpty(tb_text_email.Text))
            {
                query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil,CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user where u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%'";
            }
            else
            {
                query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil,CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user where u.email LIKE '%" + tb_text_email.Text + "%'";
            }

            // Execute a consulta SQL
            sql_all(query);

            // Chame o evento de mudança da ddl_escolha para aplicar a ordenação
            ddl_escolha_SelectedIndexChanged(sender, e);
        }

        //ddl_escolha da View2 users/psicologos
        protected void ddl_escolha_SelectedIndexChanged(object sender, EventArgs e)
                {
                    // Declare a variável query fora do switch
                    string query;

                    // Verifique a escolha do usuário e ajuste a ordem conforme necessário
                    switch (ddl_escolha.SelectedValue)
                    {
                        
                        case "Crescente":
                            query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil, CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%' ORDER BY u.utilizador ASC";
                            break;
                        case "Decrecente":
                            query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil, CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%' ORDER BY u.utilizador DESC";
                            break;
                        case "Ativos":
                            query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil, CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%' and u.ativo='true'";
                            break;
                        case "Desativos":
                            query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil, CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%' and u.ativo='false'";
                            break;
                        case "Bloqueados":
                            query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil, CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%' and u.banir='true'";
                            break;
                        default:
                   
                            query = "SELECT u.utilizador, u.email, u.ativo, u.nif, p.perfil, CASE WHEN u.cod_perfil = 2 THEN pr.verificado ELSE NULL END AS verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil LIKE '%" + ddl_users.SelectedValue + "%'";
                            break;
                    }

                    sql_all(query);
                }

        //View 3 psicologos
        protected void lnkPsicologo_Click(object sender, EventArgs e)
        {
            mvMainContent.ActiveViewIndex = 2;
            string query = "SELECT u.utilizador, u.email, u.ativo, p.perfil, pr.cp, pr.verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil = 2 and pr.verificado='false'";

            sql_psicologos(query);
        }

        //Class psicologos View3
        public class psicologo
        {

            public string utilizador { get; set; }
            public string email { get; set; }
            public string perfil { get; set; }
            public Boolean ativo { get; set; }
            public int? cp { get; set; }
            public Boolean? verificado { get; set; }
            public Boolean? Bt_visivel { get; set; }

        }

        //SQL da View 3 psicologos
        public void sql_psicologos(string query)
        {
            List<psicologo> lst_pauta = new List<psicologo>();
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand(query, myConn);
            myConn.Open();
            SqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                psicologo notaObj = new psicologo();
                notaObj.utilizador = reader.GetString(0);
                notaObj.email = reader.GetString(1);
                notaObj.ativo = reader.GetBoolean(2);
                notaObj.perfil = reader.GetString(3);
                notaObj.cp = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                notaObj.verificado = reader.IsDBNull(5) ? (bool?)null : reader.GetBoolean(5);
                notaObj.Bt_visivel = !notaObj.verificado;
                lst_pauta.Add(notaObj);
            }
            reader.Close();
            myConn.Close();
            rpt_psicologos.DataSource = lst_pauta;
            rpt_psicologos.DataBind();
        }
        
        protected void btn_Verificar_Click(object sender, EventArgs e)
        {
            Button btn_verificar = (Button)sender;

            btn_verificar.Text = btn_verificar.CommandArgument;
            string email = btn_verificar.CommandArgument;
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand("UPDATE tb_profissionais SET verificado = 1 FROM tb_profissionais INNER JOIN tb_users AS u ON tb_profissionais.cod_user = u.id WHERE u.email = @email", myConn);

            myCommand.Parameters.AddWithValue("@email", email);


            myConn.Open();
            myCommand.ExecuteNonQuery();
            myConn.Close();

            mvMainContent.ActiveViewIndex = 2;
            string query = "SELECT u.utilizador, u.email, u.ativo, p.perfil, pr.cp, pr.verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil = 2";

            sql_psicologos(query);

        }

        //Botao filtrar View 3 psicologos
        protected void btn_filtrar_psicologos_Click(object sender, EventArgs e)
        {
        string query = "SELECT u.utilizador, u.email, u.ativo, p.perfil, pr.cp, pr.verificado FROM tb_users u LEFT JOIN tb_tipo_perfil p ON u.cod_perfil = p.id LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.cod_perfil = 2 and u.email LIKE '%" + tb_text_psicologo.Text + "%'";
        sql_psicologos(query);
        }

       
   
    }


}