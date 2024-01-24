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
    public partial class User : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //Verifica a Session do site
            if (Session["perfil"] == null || !Session["perfil"].ToString().Equals("Utilizador", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("Login.aspx");
            }

            //SQL para mostrar dados do user
            string user = (string)Session["utilizador"];
            if (!IsPostBack)
            {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            
            string query = "select * from tb_users where email= @userEmail";
            SqlCommand myCommand = new SqlCommand(query, myConn);
            myCommand.Parameters.AddWithValue("@userEmail", user);
            myConn.Open();
            SqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {               
                tb_nome.Text = reader["utilizador"].ToString();
                if (reader["dadosBinarios"] != DBNull.Value)
                {
                    byte[] dadosBinarios = (byte[])reader["dadosBinarios"];
                    img_user.ImageUrl = "data:image/jpeg;base64," + Convert.ToBase64String(dadosBinarios);
                }
                else
                {
                    img_user.ImageUrl="img/semfoto.png";
                }
                

                //ira ler da bd os seguintes campos
                tb_instagram.Text = reader["instagram"].ToString();
                tb_linkedin.Text = reader["linkedin"].ToString();
                tb_nome.Text = reader["utilizador"].ToString();                
                lbl_email.Text = reader["email"].ToString();
                if(!string.IsNullOrEmpty(reader["sexo"].ToString()))
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
            }
            reader.Close();
            myConn.Close();


            //SQL do carousel psicologos
            rpt_card_psicologos.ItemCommand += rpt_card_psicologos_ItemCommand;

            List<pauta_notas> lst_pauta = new List<pauta_notas>();
            SqlConnection myConn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            string query2 = "SELECT TOP 12 u.id, u.utilizador, pr.descricao, u.dadosBinarios FROM tb_users u LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.id = pr.cod_user AND u.cod_perfil = 2 AND pr.verificado = 'true' and u.ativo='true' ORDER BY NEWID();";

            SqlCommand myCommand2 = new SqlCommand(query2, myConn2);
            myConn2.Open();
            SqlDataReader reader2 = myCommand2.ExecuteReader();
            while (reader2.Read())
            {
                pauta_notas notaObj = new pauta_notas();
                notaObj.id = reader2.GetInt32(0);
                notaObj.utilizador = reader2.GetString(1);
                notaObj.descricao = reader2.IsDBNull(2) ? null : reader2.GetString(2);
                notaObj.dadosBinarios = reader2.IsDBNull(3) ? null : (byte[])reader2[3];

                lst_pauta.Add(notaObj);
            }
            reader2.Close();
            myConn2.Close();
            rpt_card_psicologos.DataSource = lst_pauta;
            rpt_card_psicologos.DataBind();
            }

        }
        
        //Botao Guardar alteraçoes de user
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


            //campos para editar usuario.. apos preenchido ira para a BD nos seguintes campos
            myCommand.Parameters.AddWithValue("@email", user);
            myCommand.Parameters.AddWithValue("@user", string.IsNullOrWhiteSpace(tb_nome.Text) ? (object)DBNull.Value : tb_nome.Text);
            myCommand.Parameters.AddWithValue("@instagram", string.IsNullOrWhiteSpace(tb_instagram.Text) ? (object)DBNull.Value : tb_instagram.Text);
            myCommand.Parameters.AddWithValue("@linkedin", string.IsNullOrWhiteSpace(tb_linkedin.Text) ? (object)DBNull.Value : tb_linkedin.Text);
            myCommand.Parameters.AddWithValue("@sexo", rtn_sexo.Visible ? rtn_sexo.SelectedValue : lbl_sexo.Text);
            myCommand.Parameters.AddWithValue("@data_nasc", string.IsNullOrWhiteSpace(tb_data_nasc.Text) ? (object)DBNull.Value : tb_data_nasc.Text);
            myCommand.Parameters.AddWithValue("@telemovel", string.IsNullOrWhiteSpace(tb_telemovel.Text) ? (object)DBNull.Value : tb_telemovel.Text);
            myCommand.Parameters.AddWithValue("@morada", string.IsNullOrWhiteSpace(tb_morada.Text) ? (object)DBNull.Value : tb_morada.Text);
            myCommand.Parameters.AddWithValue("@nif", string.IsNullOrWhiteSpace(tb_nif.Text) ? (object)DBNull.Value : tb_nif.Text);
            myCommand.Parameters.AddWithValue("@sobreSi", string.IsNullOrWhiteSpace(tb_descricao.Text) ? (object)DBNull.Value : tb_descricao.Text);

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "update_user";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();
            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            if (resposta == 1)
            {
                Response.Redirect("User.aspx");
                lbl_msg.Text = "Dados Atualizados";  //tirar isso para depois carregar no pageload

            }
            else
            {
                lbl_msg.Text = "Erro ao Atualizar dados";

            }
            myConn.Close();
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

        //Botao de cancelar alteraçoes nos dados user 
        protected void btn_cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        //Classe de carousel
        public class pauta_notas
        {
            public int id { get; set; }
            public string utilizador { get; set; }
            public string descricao { get; set; }
            public byte[] dadosBinarios { get; set; }
        }

        //Botao detalhes psicologo
        protected void rpt_card_psicologos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MostrarDetalhes")
            {
                
                string psicologoId = e.CommandArgument.ToString();

                
                Response.Redirect($"Detalhes_psicologo.aspx?id={psicologoId}");
            }
        }

        //Botao Lista Completa de Psicologos
        protected void bt_lista_psicologos_Click(object sender, EventArgs e)
        {
            Response.Redirect("Lista_psicologos.aspx");
        }
    }
}