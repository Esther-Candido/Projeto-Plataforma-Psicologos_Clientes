<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="OndaMental.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <main id="main  ">

        <section class="breadcrumbs " style="background-color: #eee;">
            <div class="container">
                <div class="row">

                    <!-- Sidebar/Menu -->
                    <div id="sidebar" class="col-md-3 col-lg-2 d-md-block bg-light sidebar">
                        <div class="card-body text-center">
                            <asp:Image ID="img_user" runat="server" class="rounded-circle img-fluid pt-1" Style="width: 150px; height: 160px; object-fit: cover;" />
                            <br />
                            <br />
                            <asp:Label ID="lbl_user" runat="server" Text="" Style="text-align: center;"></asp:Label>
                            <br />
                            <asp:Label ID="lbl_administrador" runat="server" Text='<i class="fa-solid fa-crown"></i> Administrador' ForeColor="#00CC00"></asp:Label>
                        </div>
                        <hr />
                        <div class="card mb-4 mb-lg-0 ">
                            <div class="card-body p-0 ">
                                <ul class="list-group list-group-flush rounded-3">
                                    <li class="list-group-item d-flex justify-content-center align-items-center p-3">
                                        <asp:Button ID="btn_mudar_pw" runat="server" Text="Mudar Palavra-Passe" CssClass="form-control"  OnClientClick="openInNewTab();" />
                                    </li>
                                    <script type="text/javascript">
                                        function openInNewTab() {
                                            window.open('alterar_pw2.aspx', '_blank');
                                        }
                                    </script>
                                </ul>
                            </div>
                        </div>
                        <hr />
                        <div class="position-sticky">
                            <ul class="nav flex-column">
                                <li class="nav-item">
                                    <asp:LinkButton ID="lnkDashboard" runat="server" CssClass="nav-link" OnClick="lnkDashboard_Click">
                                    Dados Pessoais
                                    </asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lnkUsers" runat="server" CssClass="nav-link" OnClick="lnkUsers_Click">
                                    Pacientes/Psicologos
                                    </asp:LinkButton>
                                </li>
                                <li class="nav-item">
                                    <asp:LinkButton ID="lnkPsicologo" runat="server" CssClass="nav-link" OnClick="lnkPsicologo_Click">
                                    Verificar Psicólogos
                                    </asp:LinkButton>
                                </li>
                               

                            </ul>
                        </div>
                    </div>

                    <div class="col-md-9 ms-sm-auto col-lg-10 px-md-4">
                        <asp:MultiView ID="mvMainContent" runat="server" ActiveViewIndex="0">

                            <!-- View 1: Dashboard -->
                            <asp:View ID="viewDashboard" runat="server">
                                <div class="card-body bg-white p-3">
                                    <!-- Seção para mudar a foto de perfil -->
                                    <div class="row">
                                        <div class="col-sm-4">
                                            Mudar foto de perfil:
                        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" Width="140px" />
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <p class="mb-0">Utilizador</p>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="tb_user" runat="server" placeholder="Insira seu nome" BorderStyle="None"></asp:TextBox>
                                        </div>
                                    </div>
                                    <hr />
                                   
                                       
                                    <asp:TextBox ID="tb_nome" runat="server" visible="False" BorderStyle="None" Width="600px" placeholder="Insira seu nome"></asp:TextBox>
                                  
                                  
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <p class="mb-0">Email</p>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:Label ID="lbl_email" runat="server"></asp:Label>

                                        </div>
                                    </div>
                                    <hr>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <p class="mb-0">Data Nascimento</p>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="tb_data_nasc" runat="server" BorderStyle="None" placeholder="dd-mm-aaaa"></asp:TextBox>

                                        </div>
                                    </div>
                                    <hr>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <p class="mb-0">Telemovel</p>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="tb_telemovel" runat="server" BorderStyle="None" Width="372px" placeholder="Insira seu nª telemovel"></asp:TextBox>
                                        </div>
                                    </div>
                                    <hr>

                                    <div class="row">
                                        <div class="col-sm-2">
                                            <p class="mb-0">Sobre si</p>
                                        </div>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="tb_descricao" runat="server" BorderStyle="None" Width="372px" TextMode="MultiLine" placeholder="Escreva algo sobre si"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <asp:Label ID="lbl_msg" runat="server" Text=""></asp:Label>
                                <!-- Botão para guardar alterações -->
                                <div class="row">
                                    <div class="col">
                                        <div class="d-flex justify-content-center mb-2">
                                            <asp:Button ID="btn_guardarAll" class="btn btn-primary" runat="server" Text="Guardar Alteraçoes" OnClick="btn_guardarAll_Click" />
                                        </div>
                                    </div>
                                </div>

                            </asp:View>


                            <!-- View 2: Lista de Users/Psicologos -->
                            <asp:View ID="viewUsers" runat="server">
                                <h2>Lista de Users / Psicologos</h2>
                                <br />
                                <div class="row">
                                    <!-- Filtros e botão de filtrar -->
                                    <div class="col-md-4">
                                        <asp:TextBox ID="tb_text_email" runat="server" CssClass="form-control" placeholder="Insira Email"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddl_users" runat="server" CssClass="btn btn-light dropdown-toggle" DataSourceID="SqlDataSource1" DataTextField="perfil" DataValueField="id"></asp:DropDownList>
                                    </div>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:SiteOndaMentalConnectionString %>' SelectCommand="SELECT * FROM [tb_tipo_perfil]"></asp:SqlDataSource>
                                    <div class="col-md-4">
                                        <asp:Button ID="btn_filtrar" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btn_filtrar_Click" />
                                    </div>
                                    <div class="col-md-2 ml-auto text-right">
                                        <asp:DropDownList ID="ddl_escolha" runat="server" CssClass="btn btn-light dropdown-toggle" OnSelectedIndexChanged="ddl_escolha_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem>Tudo</asp:ListItem>
                                            <asp:ListItem>Crescente</asp:ListItem>
                                            <asp:ListItem>Decrecente</asp:ListItem>
                                            <asp:ListItem>Ativos</asp:ListItem>
                                            <asp:ListItem>Desativos</asp:ListItem>
                                            <asp:ListItem>Bloqueados</asp:ListItem>
                                        </asp:DropDownList>


                                    </div>

                                </div>
                                <br />
                                <!-- Repeater para exibir dados -->
                                <asp:Repeater ID="rpt_users" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped">
                                            <thead class="thead-light">
                                                <tr>
                                                    <th>Utilizador</th>
                                                    <th>Email</th>
                                                    <th>Perfil</th>
                                                    <th>Ativo</th>
                                                    <%# ddl_users.SelectedValue == "2" ? "<th>Verificado</th>" : string.Empty %>
                                                    <th>Opções</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("utilizador") %></td>
                                            <td><%#Eval("email") %></td>
                                            <td><%#Eval("perfil") %></td>
                                            <td><%#Eval("ativo") %></td>
                                            <%# Eval("verificado") != null ? "<td>" + Eval("verificado") + "</td>" : string.Empty %>
                                            <td>
                                                <asp:HyperLink ID="lnk_info" CssClass="btn btn-primary" runat="server" Text="Info" NavigateUrl='<%# "Info_users_psicologos.aspx?email=" + Server.UrlEncode(EncryptString(Eval("email").ToString())) %>' />

                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr>
                                            <td><%#Eval("utilizador") %></td>
                                            <td><%#Eval("email") %></td>
                                            <td><%#Eval("perfil") %></td>
                                            <td><%#Eval("ativo") %></td>
                                            <%# Eval("verificado") != null ? "<td>" + Eval("verificado") + "</td>" : string.Empty %>
                                            <td>
                                                <asp:HyperLink ID="lnk_info" CssClass="btn btn-primary" runat="server" Text="Info" NavigateUrl='<%# "Info_users_psicologos.aspx?email=" + Server.UrlEncode(EncryptString(Eval("email").ToString())) %>' />

                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </asp:View>

                            <!-- View 3 e Psicologos (Imcompleto)-->
                            <asp:View ID="viewPsicologo" runat="server">
                                <h2>Lista de Psicologos</h2>
                                <br />
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:TextBox ID="tb_text_psicologo" runat="server" CssClass="form-control" placeholder="Insira Email"></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btn_filtrar_psicologos" runat="server" Text="Filtrar" CssClass="btn btn-primary" OnClick="btn_filtrar_psicologos_Click" />
                                    </div>

                                </div>
                                <br />
                                <asp:Repeater ID="rpt_psicologos" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-striped">
                                            <thead class="thead-light">

                                                <tr>
                                                    <th><b>Utilizador</b></th>
                                                    <th><b>Email</b></th>
                                                    <th><b>Perfil</b></th>
                                                    <th><b>Ativo</b></th>
                                                    <th><b>CP</b></th>
                                                    <th><b>Verificado</b></th>
                                                    <th><b>Opções</b></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("utilizador") %></td>
                                            <td><%#Eval("email") %></td>
                                            <td><%#Eval("perfil") %></td>
                                            <td><%#Eval("ativo") %></td>
                                            <td><%#Eval("cp") %></td>
                                            <td><%#Eval("verificado") %></td>
                                            <td>

                                                <asp:Button ID="btn_verificar" runat="server" class="btn btn-success" Text="Verificar" CommandArgument='<%#Eval("email") %>' Visible='<%#Eval("Bt_visivel") %>' OnClick="btn_Verificar_Click" />
                                                <asp:HyperLink ID="lnk_info" CssClass="btn btn-primary" runat="server" Text="Info" NavigateUrl='<%# "Info_users_psicologos.aspx?email=" + Server.UrlEncode(EncryptString(Eval("email").ToString())) %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr>
                                            <td><%#Eval("utilizador") %></td>
                                            <td><%#Eval("email") %></td>
                                            <td><%#Eval("perfil") %></td>
                                            <td><%#Eval("ativo") %></td>
                                            <td><%#Eval("cp") %></td>
                                            <td><%# Eval("verificado") %></td>

                                            <td>

                                                <asp:Button ID="btn_verificar" runat="server" class="btn btn-success" Text="Verificar" CommandArgument='<%#Eval("email") %>' Visible='<%#Eval("Bt_visivel") %>' OnClick="btn_Verificar_Click" />
                                                <asp:HyperLink ID="lnk_info" CssClass="btn btn-primary" runat="server" Text="Info" NavigateUrl='<%# "Info_users_psicologos.aspx?email=" + Server.UrlEncode(EncryptString(Eval("email").ToString())) %>' />

                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <FooterTemplate>
                                        </tbody>
                                    </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </asp:View>

                            <!-- Adicione mais views conforme necessário -->
                        </asp:MultiView>
                    </div>
                </div>
            </div>




        </section>
    </main>
</asp:Content>
