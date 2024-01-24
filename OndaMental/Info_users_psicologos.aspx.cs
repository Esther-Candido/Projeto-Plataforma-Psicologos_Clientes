using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;


namespace OndaMental
{
    public partial class Info_users_psicologos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["logado"] == null || Session["logado"] != "Sim")
            {
                Response.Redirect("Login.aspx", false);

            }
            else if (Session["perfil"] == null || !Session["perfil"].ToString().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("index.aspx", false);

            }

            string user = DecryptString(Request.QueryString["email"]);


            if (!IsPostBack)
            {
                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);

                string query = "select * from tb_users where email= @userEmail";
                SqlCommand myCommand = new SqlCommand(query, myConn);
                myCommand.Parameters.AddWithValue("@userEmail", user);
                myConn.Open();
                SqlDataReader reader = myCommand.ExecuteReader();

                if (reader.Read()) 
                {
                    tb_user.Text = reader["utilizador"].ToString();

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
                    tb_instagram.Text = reader["instagram"].ToString();
                    tb_linkedin.Text = reader["linkedin"].ToString();
                    tb_nome.Text = reader["nomeCompleto"].ToString();
                    lbl_email.Text = reader["email"].ToString();

                    if (!string.IsNullOrEmpty(reader["sexo"].ToString()))
                    {
                        lbl_sexo.Visible = true;
                        lbl_sexo.Text = reader["sexo"].ToString();
                    }
                    else
                    {
                        rtn_sexo.Visible = true;
                    }

                    tb_data_nasc.Text = reader["data_nasc"].ToString();
                    tb_telemovel.Text = reader["telemovel"].ToString();
                    tb_morada.Text = reader["morada"].ToString();
                    tb_nif.Text = reader["nif"].ToString();
                    tb_descricao.Text = reader["sobreSi"].ToString();

                    if (Convert.ToBoolean(reader["banir"]) == true)
                    {
                        btn_ativar.Visible = true;
                        lbl_bloqueado.Visible = true;
                    }
                    else
                    {
                        btn_Bloqueiar.Visible = true;
                    }
                }

                reader.Close();
                myConn.Close();
            }
        }

        public static string DecryptString(string Message)
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



            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;



            // Step 4. Convert the input string to a byte[]



            Message = Message.Replace("KKK", "+");
            Message = Message.Replace("JJJ", "/");
            Message = Message.Replace("III", "\\");





            byte[] DataToDecrypt = Convert.FromBase64String(Message);



            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }



            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        protected void btn_guardarAll_Click(object sender, EventArgs e)
        {
            string user = DecryptString(Request.QueryString["email"]);
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

                myCommand.Parameters.AddWithValue("@ficheiroBinario", imgBinaryData);
                myCommand.Parameters.AddWithValue("@ct", contentType);
            }
            else
            {
                myCommand.Parameters.AddWithValue("@ficheiroBinario", existingImgBinaryData);
                myCommand.Parameters.AddWithValue("@ct", existingContentType);
            }

            myCommand.Parameters.AddWithValue("@email", user);
            myCommand.Parameters.AddWithValue("@user", string.IsNullOrWhiteSpace(tb_user.Text) ? (object)DBNull.Value : tb_user.Text);
            myCommand.Parameters.AddWithValue("@instagram", string.IsNullOrWhiteSpace(tb_instagram.Text) ? (object)DBNull.Value : tb_instagram.Text);
            myCommand.Parameters.AddWithValue("@linkedin", string.IsNullOrWhiteSpace(tb_linkedin.Text) ? (object)DBNull.Value : tb_linkedin.Text);
            myCommand.Parameters.AddWithValue("@sexo", rtn_sexo.Visible ? rtn_sexo.SelectedValue : lbl_sexo.Text);
            myCommand.Parameters.AddWithValue("@data_nasc", string.IsNullOrWhiteSpace(tb_data_nasc.Text) ? (object)DBNull.Value : tb_data_nasc.Text);
            myCommand.Parameters.AddWithValue("@telemovel", string.IsNullOrWhiteSpace(tb_telemovel.Text) ? (object)DBNull.Value : tb_telemovel.Text);
            myCommand.Parameters.AddWithValue("@morada", string.IsNullOrWhiteSpace(tb_morada.Text) ? (object)DBNull.Value : tb_morada.Text);
            myCommand.Parameters.AddWithValue("@nif", string.IsNullOrWhiteSpace(tb_nif.Text) ? (object)DBNull.Value : tb_nif.Text);
            myCommand.Parameters.AddWithValue("@sobreSi", string.IsNullOrWhiteSpace(tb_descricao.Text) ? (object)DBNull.Value : tb_descricao.Text);

            SqlParameter retorno = new SqlParameter();
            retorno.ParameterName = "@retorno";
            retorno.Direction = ParameterDirection.Output;
            retorno.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(retorno);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "update_user";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();
            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            if (resposta == 1)
            {
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                lbl_msg.Text = "Erro ao Atualizar dados";
            }
            myConn.Close();
        }

        protected void btn_voltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Admin.aspx");
        }

        protected void btn_Bloqueiar_Click(object sender, EventArgs e)
        {
            string user = DecryptString(Request.QueryString["email"]);
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand("update tb_users set banir = 'true' where email = @user", myConn);

            myCommand.Parameters.AddWithValue("@user", user);

            myConn.Open();
            myCommand.ExecuteNonQuery();
            myConn.Close();

            Response.Redirect(Request.RawUrl);
        }

        protected void btn_ativar_Click(object sender, EventArgs e)
        {
            string user = DecryptString(Request.QueryString["email"]);
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand("update tb_users set banir = 'false' where email = @user", myConn);

            myCommand.Parameters.AddWithValue("@user", user);

            myConn.Open();
            myCommand.ExecuteNonQuery();
            myConn.Close();

            Response.Redirect(Request.RawUrl);
        }
    }
}