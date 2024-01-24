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
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using Microsoft.Owin;
using Microsoft.Owin.Security;

using Owin;
using System.Globalization;

namespace OndaMental
{
    public partial class Psicologo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Verifica a Session do site
            if (Session["perfil"] == null || !Session["perfil"].ToString().Equals("Psicologo", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("Login.aspx");
            }

            //SQL para mostrar dados do psicologo
            string user = (string)Session["utilizador"];
            if (!IsPostBack)
            {

                PreencherHorarios(); //PREENCHER os horarios disponibilidade dos psicologo na dropDownlist 


                SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);


                string query = "select u.*, p.* from tb_users u left join tb_profissionais p ON u.id = p.cod_user where email= @userEmail";
                SqlCommand myCommand = new SqlCommand(query, myConn);
                myCommand.Parameters.AddWithValue("@userEmail", user);
                myConn.Open();
                SqlDataReader reader = myCommand.ExecuteReader();
                while (reader.Read())
                {
                    tb_nome.Text = reader["utilizador"].ToString();
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


                     if (reader["verificado"].ToString() == "True")
                    {
                        lbl_verificado.Visible = true;
                        lbl_naoverificado.Visible = false;
                    }


                    tb_instagram.Text = reader["instagram"].ToString();
                    tb_linkedin.Text = reader["linkedin"].ToString();
                    tb_valor_hora.Text = reader["valor_hora"].ToString();
                    tb_descricao_psicologo.Text = reader["descricao"].ToString();
                    tb_nome.Text = reader["utilizador"].ToString();
                    lbl_email.Text = reader["email"].ToString();
                    lbl_cp.Text = reader["cp"].ToString();
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


                    // leitura dos dias disponiveis da BD
                    string diasDisponiveisDB = reader["dias_disponiveis"].ToString();
                    List<string> diasSelecionadosDB = diasDisponiveisDB.Split(',').Select(d => d.Trim()).ToList(); // irá dividir a string em uma lista de dias
                    foreach (ListItem item in cblDiasDisponiveis.Items)   // marca na checklist os dias correspondentes
                    {
                        item.Selected = diasSelecionadosDB.Contains(item.Value);
                    }

                    // atribui os valores de horas_inicio e fim a dropDownList q definimos para inicio e fim
                    TimeSpan horaInicioDB = (TimeSpan)reader["horas_inicio"];
                    TimeSpan horaFimDB = (TimeSpan)reader["horas_fim"];
                    ddlHoraInicio.SelectedValue = horaInicioDB.ToString("hh\\:mm");
                    ddlHoraFim.SelectedValue = horaFimDB.ToString("hh\\:mm");


                }
                reader.Close();
                myConn.Close();

                BotaoAgenda();

            }
        }

        private void BotaoAgenda()
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
            btnCriarAgenda.Visible = !tokenExists;
            btn_agenda.Visible = tokenExists;
        }

        //Botao Guardar alteraçoes de psicologo
        protected void btn_guardarAll_Click(object sender, EventArgs e)
        {

            string user = (string)Session["utilizador"];
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);

            //horas disponivel psicologo
            TimeSpan horaInicio = TimeSpan.Parse(ddlHoraInicio.SelectedValue);
            TimeSpan horaFim = TimeSpan.Parse(ddlHoraFim.SelectedValue);

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
            myCommand.Parameters.AddWithValue("@user", string.IsNullOrWhiteSpace(tb_nome.Text) ? (object)DBNull.Value : tb_nome.Text);
            myCommand.Parameters.AddWithValue("@instagram", string.IsNullOrWhiteSpace(tb_instagram.Text) ? (object)DBNull.Value : tb_instagram.Text);
            myCommand.Parameters.AddWithValue("@linkedin", string.IsNullOrWhiteSpace(tb_linkedin.Text) ? (object)DBNull.Value : tb_linkedin.Text);
            myCommand.Parameters.AddWithValue("@valor_hora", Decimal.TryParse(tb_valor_hora.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal valorHora) ? (object)valorHora : DBNull.Value);
            myCommand.Parameters.AddWithValue("@descricao", string.IsNullOrWhiteSpace(tb_descricao_psicologo.Text) ? (object)DBNull.Value : tb_descricao_psicologo.Text);
            myCommand.Parameters.AddWithValue("@sexo", rtn_sexo.Visible ? rtn_sexo.SelectedValue : lbl_sexo.Text);
            myCommand.Parameters.AddWithValue("@data_nasc", string.IsNullOrWhiteSpace(tb_data_nasc.Text) ? (object)DBNull.Value : tb_data_nasc.Text);
            myCommand.Parameters.AddWithValue("@telemovel", string.IsNullOrWhiteSpace(tb_telemovel.Text) ? (object)DBNull.Value : tb_telemovel.Text);
            myCommand.Parameters.AddWithValue("@morada", string.IsNullOrWhiteSpace(tb_morada.Text) ? (object)DBNull.Value : tb_morada.Text);
            myCommand.Parameters.AddWithValue("@nif", string.IsNullOrWhiteSpace(tb_nif.Text) ? (object)DBNull.Value : tb_nif.Text);
            myCommand.Parameters.AddWithValue("@sobreSi", string.IsNullOrWhiteSpace(tb_descricao.Text) ? (object)DBNull.Value : tb_descricao.Text);
            myCommand.Parameters.AddWithValue("@horas_inicio", horaInicio);
            myCommand.Parameters.AddWithValue("@horas_fim", horaFim);

            //salvar dias disponivel psicologo do checkboxlist e criar uma string separada por virgulas
            List<string> diasSelecionados = cblDiasDisponiveis.Items.Cast<ListItem>()
                                               .Where(li => li.Selected)
                                               .Select(li => li.Value)
                                               .ToList();
            string diasDisponiveis = string.Join(",", diasSelecionados);

            myCommand.Parameters.AddWithValue("@diasDisponiveis", string.IsNullOrWhiteSpace(diasDisponiveis) ? (object)DBNull.Value : diasDisponiveis);

            SqlParameter valor = new SqlParameter();
            valor.ParameterName = "@retorno";
            valor.Direction = ParameterDirection.Output;
            valor.SqlDbType = SqlDbType.Int;
            myCommand.Parameters.Add(valor);

            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.CommandText = "update_psicologo";

            myCommand.Connection = myConn;
            myConn.Open();
            myCommand.ExecuteNonQuery();
            int resposta = Convert.ToInt32(myCommand.Parameters["@retorno"].Value);
            if (resposta == 1)
            {
                Response.Redirect("Psicologo.aspx");
                lbl_msg.Text = "Dados Atualizados";  //depois tirar isso para carregar no pageload
            }
            else
            {
                lbl_msg.Text = "Erro ao Atualizar dados";

            }
            myConn.Close();
        }

        //horarios disponiveis psicologo (24horas)
        private void PreencherHorarios()
        {
            for (int hora = 0; hora <= 23; hora++)
            {
                ddlHoraInicio.Items.Add(new ListItem(hora.ToString("D2") + ":00"));
                ddlHoraFim.Items.Add(new ListItem(hora.ToString("D2") + ":00"));
            }
        }



        //Botao de cancelar alteraçoes nos dados psicologo
        protected void btn_cancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }



        //encryptacao
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

        protected void btnCriarAgenda_Click(object sender, EventArgs e)
        {
            //CONFIGURAÇÃO CRIAÇAO DE AGENDA DO GOOGLE...
            string clientId = "1064229684992-fas3lf2ni6qblevb4e8r7codhejp51ac.apps.googleusercontent.com";
            string redirectUri = "https://localhost:44337/Default.aspx";
            string scopes = Google.Apis.Calendar.v3.CalendarService.Scope.Calendar;

    
            string authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&scope={scopes}&access_type=offline&prompt=consent";
           
            Response.Redirect(authorizationUrl);
        }

        protected void btn_agenda_Click(object sender, EventArgs e)
        {
            Response.Redirect("individual_agendaPSI.aspx");
        }
    }


}