using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;

namespace OndaMental
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            buscar_noticias();

            //SQL do carousel psicologos
            if (!IsPostBack)
            {

                rpt_card_psicologos.ItemCommand += rpt_card_psicologos_ItemCommand;
                rptTodosPsicologos.ItemCommand += rptTodosPsicologos_ItemCommand;


                CarregarTodosPsicologos();
                CarregarCarrosselPsicologos();
            }
        }

        //Classe de carousel
        public class PsicologoViewModel
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

        protected void rptTodosPsicologos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MostrarDetalhes_todos")
            {
                string psicologoId = e.CommandArgument.ToString();
                Response.Redirect($"Detalhes_psicologo.aspx?id={psicologoId}");
            }
        }


        private void CarregarCarrosselPsicologos()
        {
            List<PsicologoViewModel> lst_pauta = new List<PsicologoViewModel>();
            SqlConnection myConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString);
            string query = "SELECT TOP 4 u.id, u.utilizador, pr.descricao, u.dadosBinarios FROM tb_users u LEFT JOIN tb_profissionais pr ON u.id = pr.cod_user WHERE u.id = pr.cod_user AND u.cod_perfil = 2 AND pr.verificado = 'true' and u.ativo='true' ORDER BY NEWID();";

            SqlCommand myCommand = new SqlCommand(query, myConn);
            myConn.Open();
            SqlDataReader reader = myCommand.ExecuteReader();
            while (reader.Read())
            {
                PsicologoViewModel notaObj = new PsicologoViewModel();
                notaObj.id = reader.GetInt32(0);
                notaObj.utilizador = reader.GetString(1);
                notaObj.descricao = reader.IsDBNull(2) ? null : reader.GetString(2);
                notaObj.dadosBinarios = reader.IsDBNull(3) ? null : (byte[])reader[3];

                lst_pauta.Add(notaObj);
            }
            reader.Close();
            myConn.Close();
            rpt_card_psicologos.DataSource = lst_pauta;
            rpt_card_psicologos.DataBind();

        }

        //xml das noticias que busca de site externo
        public void buscar_noticias()
        {
            XmlDocument url = new XmlDocument();
            url.Load("https://www.noticiasaominuto.com/rss/ultima-hora");
            //xm1 buscar os dados a URL acima..
            Xml1.Document = url;
        }


        private void CarregarTodosPsicologos()
        {
            List<PsicologoViewModel> listaPsicologos = new List<PsicologoViewModel>();

            string connectionString = ConfigurationManager.ConnectionStrings["SiteOndaMentalConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT u.id, u.utilizador, u.dadosBinarios, pr.descricao
                    FROM tb_users u
                    JOIN tb_profissionais pr ON u.id = pr.cod_user
                    WHERE EXISTS (
                        SELECT 1 FROM tb_token WHERE psicologo_id = u.id
                    ) AND u.banir != 'true' AND u.ativo = 'true'
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PsicologoViewModel psicologo = new PsicologoViewModel
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                utilizador = reader.GetString(reader.GetOrdinal("utilizador")),
                                descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString(reader.GetOrdinal("descricao")),
                                dadosBinarios = reader.IsDBNull(reader.GetOrdinal("dadosBinarios")) ? null : (byte[])reader["dadosBinarios"]
                            };
                            listaPsicologos.Add(psicologo);
                        }
                    }
                }
            }

            rptTodosPsicologos.DataSource = listaPsicologos;
            rptTodosPsicologos.DataBind();
        }


    }
}