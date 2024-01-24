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

namespace OndaMental
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_login_Click(object sender, EventArgs e)
        {
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@pw", EncryptString(tb_pw.Text));

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            SqlParameter valor2 = new SqlParameter();
            valor2.ParameterName = "@retorno_perfil";
            valor2.Direction = ParameterDirection.Output;
            valor2.SqlDbType = SqlDbType.VarChar;
            valor2.Size = 50;
            myCommand.Parameters.Add(valor2);

            // esse retorno pega o ID do user logado
            SqlParameter valor3 = new SqlParameter();
            valor3.ParameterName = "@retorno_id";
            valor3.Direction = ParameterDirection.Output;
            valor3.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor3);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "login";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();

            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

            // ID user
            int ID_user = (myCommand.Parameters["@retorno_id"].Value != DBNull.Value) ? Convert.ToInt32(myCommand.Parameters["@retorno_id"].Value) : 0;

            String resposta_perfil = myCommand.Parameters["@retorno_perfil"].Value.ToString();
            
            if (resposta == 1 )
            {
               
                Session["logado"] = "Sim";
                Session["utilizadorID"] = ID_user; // armazenar o ID do utilizador da sessão
                Session["utilizador"] = tb_email.Text;


                // verificação para ver se tem um agendamento pendente
                if (Session["data_agendamento"] != null && Session["hora_agendamento"] != null)
                {
                   
                    string psicologoSelecionado = Session["id_psicologo"].ToString();

                    // redirecionar de volta para a página de agendamento do psicologo escolhido
                   Response.Redirect($"Detalhes_psicologo.aspx?id={psicologoSelecionado}");
                }
                else
                {
                    if (resposta_perfil == "Utilizador")
                    {
                        Session["perfil"] = resposta_perfil;
                        Response.Redirect("index.aspx");
                    }
                    else if (resposta_perfil == "Psicologo")
                    {
                        Session["perfil"] = resposta_perfil;
                        Response.Redirect("Psicologo.aspx");
                    }
                    else
                    {
                        Session["perfil"] = resposta_perfil;
                        Response.Redirect("Admin.aspx");
                    }
                }  
            }
            else if (resposta == 2)
            {
                lbl_msg.Text = "Cadastro Inativo";
            }
            else if (resposta == 3)
            {
                lbl_msg.Text = "Cadastro Bloqueado, entre em contacto com o admin!";
            }
            else
            {
                lbl_msg.Text = "E-mail / Palavra-Passe incorreta!";
            }
            myConn.Close();

        }

        //encryptaçao
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
    }
}

