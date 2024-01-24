<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Info_users_psicologos.aspx.cs" Inherits="OndaMental.Info_users_psicologos" %>
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
                            <div class="card-body text-center">
                                <asp:Image ID="img_user" runat="server" class="rounded-circle img-fluid" Style="width: 150px;" />
                                <br />
                                <br />
                                <asp:FileUpload ID="FileUpload1" runat="server" Width="140px" />
                                <br />
                                <asp:TextBox ID="tb_user" class="my-3" runat="server" placeholder="Insira seu nome User" BorderStyle="None" Style="text-align: center;"></asp:TextBox>
                                <br />
                                <asp:Label ID="lbl_bloqueado" runat="server" Text='Bloqueado' ForeColor="Red" Visible="False"></asp:Label> 
                            </div>
                        </div>
                        <div class="col">
                            <div class="d-flex justify-content-center mb-2">
                                <asp:Button ID="btn_Bloqueiar" class="btn btn-danger  ms-1" runat="server" Text="Bloqueiar" Visible="False" OnClick="btn_Bloqueiar_Click"/>
                                <asp:Button ID="btn_ativar" class="btn btn-success  ms-1" runat="server" Text="Ativar" Visible="False" OnClick="btn_ativar_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="card mb-4 mb-lg-0">
                            <div class="card-body p-0">
                                <ul class="list-group list-group-flush rounded-3">

                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                        <i class="fab fa-instagram fa-lg" style="color: #ac2bac;"></i>
                                        <asp:TextBox ID="tb_instagram" class="mb-0" runat="server" BorderStyle="None" placeholder="Insira seu instagram" Style="text-align: right;"></asp:TextBox>
                                    </li>
                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                        <i class="fab fa-linkedin fa-lg" style="color: #3b5998;"></i>
                                        <asp:TextBox ID="tb_linkedin" class="mb-0" runat="server" BorderStyle="None" placeholder="Insira seu Linkedin" Style="text-align: right;"></asp:TextBox>
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
                                        <asp:TextBox ID="tb_nome" runat="server" BorderStyle="None" Width="600px" placeholder="Insira seu nome"></asp:TextBox>
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
                                        <p class="mb-0">Morada</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_morada" runat="server" BorderStyle="None" Width="372px" TextMode="MultiLine" placeholder="Insira a sua morada"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">NIF</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_nif" runat="server" BorderStyle="None" Width="372px" placeholder="Insira seu NIF"></asp:TextBox>
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
                        </div>


                    </div>
                </div>
                <br />
                <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                <div class="row">
                    <div class="col">
                        <div class="d-flex justify-content-center mb-2">
                            <asp:Button ID="btn_guardarAll" runat="server" class="btn btn-primary" Text="Guardar" OnClick="btn_guardarAll_Click"/>
                            <asp:Button ID="btn_voltar" runat="server" class="btn btn-danger ms-1"  Text="Voltar" OnClick="btn_voltar_Click"/>
                            
                        </div>
                    </div>
                </div>
            </div>
            <!--Fim dados user-->
            </section>
            </main>
</asp:Content>
