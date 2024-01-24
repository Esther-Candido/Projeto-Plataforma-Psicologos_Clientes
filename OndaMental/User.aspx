<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="OndaMental.User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main id="main  ">

        <section class="breadcrumbs " style="background-color: #eee;">

            <!--Inicio dados user-->
            <div class="container ">
                <div class="row">
                    <div class="col-lg-4">
                        <div class="card mb-4">
                            <div class="card-body text-center" style="display: grid; justify-content: center;">
                                <asp:Image ID="img_user" onerror="this.src='http://logz.io/wp-content/uploads/2016/02/stack-overflow-logo.png';" runat="server" class="rounded-circle img-fluid" Style="width: 150px; height: 160px; object-fit: cover;" Width="150px" Height="160px" />
                                <br />
                                <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" Width="135px" />
                                <br />
                               

                            </div>
                        </div>

                        <div class="card mb-4 mb-lg-0">
                            <div class="card-body p-0">
                                <ul class="list-group list-group-flush rounded-3">
                                    <li class="list-group-item d-flex justify-content-center align-items-center p-3">
                                        <asp:Button ID="btn_mudar_pw" runat="server" CssClass="form-control"  Text="Mudar Palavra-Passe" PostBackUrl="~/alterar_pw2.aspx" />
                                    </li>
                                </ul>
                            </div>
                        </div>


                        <br />
                        <div class="card mb-4 mb-lg-0">
                            <div class="card-body p-0">
                                <ul class="list-group list-group-flush rounded-3">

                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                        <i class="fab fa-instagram fa-lg" style="color: #ac2bac;"></i>
                                        <asp:TextBox ID="tb_instagram" class="mb-0 form-control" runat="server" BorderStyle="None" placeholder="Insira seu instagram" Style="text-align: right;"></asp:TextBox>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                        <i class="fab fa-linkedin fa-lg" style="color: #3b5998;"></i>
                                        <asp:TextBox ID="tb_linkedin" class="mb-0 form-control" runat="server" BorderStyle="None" placeholder="Insira seu Linkedin" Style="text-align: right;"></asp:TextBox>
                                    </li>
                                </ul>
                            </div>
                        </div>


                    </div>
                    <div class="col-lg-8">
                        <div class="card mb-4">
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Nome Completo</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_nome" runat="server" BorderStyle="None" CssClass="form-control" Width="600px" placeholder="Insira seu nome"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
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
                                        <p class="mb-0">Sexo</p>
                                    </div>

                                    <div class="col-sm-10">
                                        <asp:Label ID="lbl_sexo" runat="server" Visible="False"></asp:Label>
                                        <asp:RadioButtonList ID="rtn_sexo" runat="server" Visible="False">
                                            <asp:ListItem> Masculino</asp:ListItem>
                                            <asp:ListItem> Feminino</asp:ListItem>
                                            <asp:ListItem> Outro</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Data Nascimento</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_data_nasc" runat="server" BorderStyle="None" CssClass="form-control" placeholder="dd-mm-aaaa" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Telemovel</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_telemovel" runat="server" BorderStyle="None" CssClass="form-control" Width="372px" placeholder="Insira seu nª telemovel"></asp:TextBox>
                                    </div>
                                </div>

                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Morada</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_morada" runat="server" BorderStyle="None" CssClass="form-control" Width="372px" TextMode="MultiLine" placeholder="Insira a sua morada"  style="resize: none;" Height="80px"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">NIF</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_nif" runat="server" BorderStyle="None" CssClass="form-control" Width="372px" placeholder="Insira seu NIF"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Sobre si</p>
                                    </div>
                                    <div class="col-sm-10"> 
                                        <asp:TextBox ID="tb_descricao" runat="server" BorderStyle="None" CssClass="form-control" Width="372px" TextMode="MultiLine" placeholder="Escreva algo sobre si"  style="resize: none;" Height="80px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>


                    </div>
                </div>
                <br />
                <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                <div class="row">
                    <div class="col">
                        <div class="d-flex justify-content-center mb-2">
                            <asp:Button ID="btn_guardarAll" class="btn btn-primary" runat="server" Text="Guardar Alteraçoes" OnClick="btn_guardarAll_Click" />
                            <asp:Button ID="btn_cancelar" class="btn btn-outline-primary ms-1" runat="server" Text="Cancelar" OnClick="btn_cancelar_Click"/>
                        </div>
                    </div>
                </div>
            </div>
            <!--Fim dados user-->

           <!--Inicio do carousel -->
            <asp:Repeater ID="rpt_card_psicologos" runat="server"  OnItemCommand="rpt_card_psicologos_ItemCommand">
            <HeaderTemplate>
                <section class="pt-5 pb-5">
                    <div class="container">
                        <div class="row">
                            <div class="col-6">
                                <h3 class="mb-3">Top Psicologos</h3>
                            </div>
                            <div class="col-6 text-right">
                                <a class="btn btn-primary mb-3 mr-1" href="#carouselExampleIndicators2" role="button" data-slide="prev">
                                    <i class="fa fa-arrow-left"></i>
                                </a>
                                <a class="btn btn-primary mb-3 " href="#carouselExampleIndicators2" role="button" data-slide="next">
                                    <i class="fa fa-arrow-right"></i>
                                </a>
                            </div>
                            <div class="col-12">
                                <div id="carouselExampleIndicators2" class="carousel slide" data-ride="carousel">
                                    <div class="carousel-inner">
            </HeaderTemplate>
            <ItemTemplate>
                <%# Container.ItemIndex % 4 == 0 ? "<div class='carousel-item " + (Container.ItemIndex == 0 ? "active" : "") + "'><div class='row'>" : "" %>
                    <div class="col-md-3 mb-3">
                        <div class="card">
                             <img class="img-fluid px-3 pt-3" alt="100%x280" src='<%# (Eval("dadosBinarios") as byte[]) != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("dadosBinarios")) : "img/semfoto.png" %>'>
                            <div class="card-body">
                                <h4 class="card-title text-center"><%#Eval("utilizador") %></h4>
                                <p class="card-text text-center"><%#Eval("descricao") %></p>
                                 <asp:LinkButton ID="lnk_informacao" CssClass="btn btn-primary" runat="server" Text="Mais Informações" CommandName="MostrarDetalhes" CommandArgument='<%#Eval("id") %>' />




                            </div>
                        </div>
                    </div>
                <%# (Container.ItemIndex + 1) % 4 == 0 ? "</div></div>" : "" %>
            </ItemTemplate>
            <FooterTemplate>
            </div>

               <div class="row">
                  <div class="col-6">
                       <h3 class=""></h3>
                  </div>
                        <div class="col-6 text-right">
                            <asp:Button ID="bt_lista_psicologos" class="btn btn-primary" runat="server" Text="Todos Psicologos" OnClick="bt_lista_psicologos_Click"/>
                        </div> 
                        </div>
                        </div>
                    </div>
                </section>
            </FooterTemplate>
        </asp:Repeater>
        <!--FIM do carousel -->

        </section>
    </main>

    <!-- Pertence ao carousel-->
<link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
<link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
<script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>     
<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"></script>
<script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>
</asp:Content>
