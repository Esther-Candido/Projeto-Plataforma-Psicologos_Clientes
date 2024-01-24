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
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace OndaMental
{
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_register_Click(object sender, EventArgs e)
        {
            //pwConfirm() == false ||
            if ( condicoesAceites() == false)
            {
                return;
            }
            else
            {
                sqlregister();
            }
        }
        public bool condicoesAceites()
        {
            bool check = true;
            if (cb_termos.Checked)
            {
                return check;

            }
            else
            {
                check = false;
                lbl_msg.Text = "Selecione os termos de privacidade";
                return check;
            }
        }
        public bool pwConfirm()
        {
            Regex maiusculas = new Regex("[A-Z]");
            Regex minusculas = new Regex("[a-z]");
            Regex numeros = new Regex("[0-9]");
            Regex especiais = new Regex("[^A-Za-z0-9]");
            Regex plica = new Regex("'");
            bool forte = true;
            if (tb_pw.Text.Length < 6)
            {
                forte = false;
            }
            if (maiusculas.Matches(tb_pw.Text).Count == 0)
            {
                forte = false;
            }
            if (minusculas.Matches(tb_pw.Text).Count == 0)
            {
                forte = false;
            }
            if (numeros.Matches(tb_pw.Text).Count == 0)
            {
                forte = false;
            }
            if (especiais.Matches(tb_pw.Text).Count == 0)
            {
                forte = false;
            }
            if (plica.Matches(tb_pw.Text).Count > 0)
            {
                forte = false;
            }
            if (tb_pw.Text != tb_pw_confirm.Text)
            {
                forte = false;
            }
            if (forte == false)
            {
                lbl_msg.Text = $"Insira uma passoword Forte ";

            }

            return forte;
        }
        public void sqlregister()
        {
        
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            SqlCommand myCommand = new SqlCommand();
            myCommand.Parameters.AddWithValue("@user", tb_nome.Text);
            myCommand.Parameters.AddWithValue("@pw", EncryptString(tb_pw.Text));
            myCommand.Parameters.AddWithValue("@email", tb_email.Text);
            myCommand.Parameters.AddWithValue("@cod_perfil", 1);
           


            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "inserir_utilizador";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();
            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);

            if (resposta == 0)
            {
                lbl_msg.Text = "utilizador já existe";
            }
            else if(resposta == 1)
            {
                confirmMail();
                politicaprivacidade.Visible = false;
                cb_termos.Visible = false;
                tb_registrar.Visible = false;
                lbl_msg.Text = "Cadastro realizado, foi enviado email para ativaçao da conta";   
            }


            myConn.Close();
        }
       
        public void confirmMail()
        {
            MailMessage mail = new MailMessage();
            SmtpClient servidor = new SmtpClient();

            mail.From = new MailAddress("ruben.alves.t0123774@edu.atec.pt", "OndaMental");
            mail.To.Add(new MailAddress(tb_email.Text));
            mail.Subject = $"Registro de Utilizador - Ativaçao de conta";
            mail.IsBodyHtml = true;
            mail.Body = "Para ativar a sua conta clique <a href='https://localhost:44337/AtivacaoConta.aspx?utilizador=" + EncryptString(tb_nome.Text) + "'> aqui </a>";


            servidor.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
            servidor.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_PORT"]);

            string utilizador = ConfigurationManager.AppSettings["SMTP_USER"];
            string passowrd = ConfigurationManager.AppSettings["SMTP_PASSWORD"];


            servidor.Credentials = new NetworkCredential(utilizador, passowrd);
            servidor.EnableSsl = true;

            servidor.Send(mail);
           
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
    }
}