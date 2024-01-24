<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" CodeBehind="Detalhes_psicologo.aspx.cs" Inherits="OndaMental.Detalhes_psicologo" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main">


        <section class="breadcrumbs" style="background-color: hsl(213.23deg 13.35% 60.92% / 86%);">
            <asp:ImageButton ID="btn_voltar" runat="server" ImageUrl="img/setas/voltarPG.png" CssClass="btn-voltar" OnClick="btn_voltar_Click" Width="30px" />

            <asp:Label ID="lb_mensagem" runat="server" Text="Id do Psicologo = " Visible="false"></asp:Label>
            <div class="container">
                <div class="row align-items-center profile-header-container">
                    <div class="col-auto">
                        <asp:Image ID="img_user" runat="server" CssClass="profile-header-img rounded-circle" ImageUrl="~/path/to/default-profile.jpg" Style="height: 150px; width: 150px;" />
                    </div>
                    <div class="col">
                        <h3 class="profile-verificado">
                            <asp:Label ID="lbl_nomePSI" runat="server" Text="Nome do Psicólogo" />
                            <asp:Label ID="lbl_verificado" runat="server" Style="font-size: 15px;" CssClass="badge bg-success ms-2" Text='<i class="fas fa-check-circle"></i> Verificado' Visible="False"></asp:Label>
                            <asp:Label ID="lbl_naoverificado" runat="server" Style="font-size: 15px;" CssClass="badge bg-danger ms-2" Text='Em análise' Visible="True"></asp:Label>
                        </h3>
                        <p class="profile-info">
                            <asp:Label ID="lbl_descrProfissionalPSI" runat="server" CssClass="text-muted" Text="Descrição Profissional" />
                        </p>
                        <p class="profile-info">
                            <i class="fas fa-envelope me-2"></i>
                            <asp:Label ID="lbl_emailPSI" runat="server" Text="email@example.com" />
                        </p>
                        <p class="profile-info">
                            <i class="fas fa-phone me-2"></i>
                            <asp:Label ID="lbl_telemovelPSI" runat="server" Text="(11) 99999-9999" />
                        </p>


                        <p class="profile-info">
                            <i class="fas fa-euro-sign me-2"></i>
                            <asp:Label ID="lbl_precoPSI" runat="server" />
                        </p>
                        <p class="profile-info">
                            <i class="fas fa-user me-2"></i>
                            <asp:Label ID="lbl_descrPessoalPSI" runat="server" />
                        </p>
                        <p class="profile-info">
                            <i class="fas fa-id-badge me-2"></i>
                            <asp:Label ID="lbl_cpPSI" runat="server" />
                        </p>

                        <div class="profile-header-socials mt-3">
                            <i class="fab fa-instagram fa-lg"></i>
                            <asp:Label ID="lbl_linkPSI" runat="server"></asp:Label>

                            <i class="fab fa-linkedin fa-lg m-2"></i>
                            <asp:Label ID="lbl_instaPSI" runat="server"></asp:Label>
                        </div>


                    </div>

                    <!-- DropDownList para datas Disponíveis -->
                    <div class="form-group">
                        <asp:Label ID="lblData" runat="server" CssClass="form-label" Text="Escolha a Data: "></asp:Label>
                        <asp:DropDownList ID="ddlDatasDisponiveis" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDatasDisponiveis_SelectedIndexChanged"></asp:DropDownList>
                    </div>
   
                    <!-- DropDownList para hora de Início -->
                    <div class="form-group">
                        <asp:Label ID="lblInicio" runat="server" CssClass="form-label" Text="Horário de Início: "></asp:Label>
                        <asp:DropDownList ID="ddlInicio" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>


                    <!-- Label para Mensagem de Status -->
                    <asp:Label ID="lblStatusAgendamento" runat="server" CssClass="text-muted texto-label" Visible="false"></asp:Label>
                    

                    <div class="col-auto">
                        <asp:Button ID="btnAgendar" runat="server" Text="Marcar consulta" CssClass="btn btn-primary" OnClick="btnAgendar_Click" />
                        
                        <asp:Button ID="btn_marcarOutra" runat="server" Text="Marcar Novamente" CssClass="btn btn-primary" Visible="False" OnClick="btn_marcarOutra_Click"  />
                    </div>

                   
                </div>
            </div>







            <style>.texto-label{
    padding: 10px;
    margin-bottom: 23px;
    font-size: x-large;
    font-family: fantasy;
    text-align: center;
}
                   .btn-voltar {
       margin-left: 44px;
       margin-top: 10px;

   }
                .form-group {
                    margin-bottom: 15px;
                }

                .form-label {
                    font-weight: bold;
                    display: block;
                    margin-bottom: 5px;
                }

                .form-control {
                    width: 100%;
                    padding: 8px;
                    font-size: 16px;
                    border: 1px solid #ced4da; /* Adiciona uma borda sutil */
                    border-radius: 5px;
                }

                    /* Estilos específicos para as DropDownList */
                    .form-control.ddl {
                        height: 38px; /* Ajusta a altura das DropDownList */
                    }

                        /* Estilo quando a DropDownList está desabilitada (se necessário) */
                        .form-control.ddl:disabled {
                            background-color: #f8f9fa; /* Cor de fundo quando desabilitada */
                        }

                        /* Estilo para as opções da DropDownList (se necessário) */
                        .form-control.ddl option {
                            font-size: 16px;
                        }

                .profile-header-container {
                    background-color: #f8f9fa;
                    padding: 20px;
                    border-radius: 10px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    margin-bottom: 20px;
                }

                .profile-header-img {
                    object-fit: cover;
                }

                .profile-name {
                    font-size: 24px;
                    font-weight: bold;
                    margin-bottom: 5px;
                }

                .profile-verificado {
                    font-size: 35px;
                    font-weight: bold;
                    margin-bottom: 5px;
                }

                .profile-info {
                    font-size: 20px;
                    margin-bottom: 10px;
                }

                    .profile-info i {
                        width: 24px;
                        text-align: center;
                    }

                .badge {
                    font-size: 0.8em;
                }
            </style>


        </section>
    </main>


</asp:Content>
