<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Psicologo.aspx.cs" Inherits="OndaMental.Psicologo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main  ">

        <section class="breadcrumbs " style="background-color: #eee;">


            <div class="container ">
                <div class="row">
                    <div class="col-lg-4">
                        <div class="card mb-4">
                            <div class="card-body text-center" style="display: grid; justify-content: center;">
                                <asp:Image ID="img_user" runat="server" class="rounded-circle img-fluid" Style="width: 150px; height: 160px; object-fit: cover;" />
                            
                                <br />
                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control" Width="140px" />
                                <br />
                                
                                <asp:Label ID="lbl_verificado" runat="server" Text='<i class="fa-solid fa-square-check"></i> Conta Verificada' Visible="False" ForeColor="#00CC00"></asp:Label>

                                <asp:Label ID="lbl_naoverificado" runat="server" Text='<i class="fa-solid fa-circle-pause"></i> CP em analise' Visible="True" ForeColor="Red"></asp:Label>
                            </div>
                        </div>

                        <div class="card mb-4 mb-lg-0">
                            <div class="card-body p-0">
                                <ul class="list-group list-group-flush rounded-3">
                                    <li class="list-group-item d-flex justify-content-center align-items-center p-3">
                                    <asp:Button ID="btnCriarAgenda" runat="server" Text="Criar Agenda" CssClass="form-control" OnClick="btnCriarAgenda_Click" />
                                        <asp:Button ID="btn_agenda" runat="server" Text="Agenda" Visible="False" CssClass="form-control" OnClick="btn_agenda_Click"  />
                                        </li>  
                                </ul>
                                <ul class="list-group list-group-flush rounded-3">
                                    <li class="list-group-item d-flex justify-content-center align-items-center p-3">
                                        <asp:Button ID="btn_mudar_pw" runat="server" Text="Mudar Password" CssClass="form-control" PostBackUrl="~/alterar_pw2.aspx" />
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
                        <br />
                        <div class="card mb-4 mb-lg-0">
                            <div class="card-body p-0">
                                    <div class="col">
                                        <p class="p-2 text-center text-bg-secondary">PERFIL PROFISSIONAL</p>
                                    </div>
                                    <hr />
                                <ul class="list-group list-group-flush rounded-3">
                                  
                                    
                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                    <div class="col-sm-3">
                                        <p class="mb-0 text-center">Valor cobrado</p>
                                    </div>
                                    <div class="col-sm-9">
                                         <asp:TextBox ID="tb_valor_hora"  runat="server" BorderStyle="None" CssClass="form-control" Width="250px" placeholder="Insira Valor Hora"></asp:TextBox>
                                    </div>
                                    </li>


                                     <!--Dias para trabalhar-->
                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                    <div class="col-sm-3">
                                        <p class="mb-0 text-center">Dias:</p>
                                    </div>
                                    <div class="col-sm-9">
                                      <asp:CheckBoxList ID="cblDiasDisponiveis" runat="server" CssClass="form-check form-control" RepeatLayout="Flow">
                                            <asp:ListItem Text="Segunda-feira" Value="Monday" />
                                            <asp:ListItem Text="Terça-feira" Value="Tuesday" />
                                            <asp:ListItem Text="Quarta-feira" Value="Wednesday" />
                                            <asp:ListItem Text="Quinta-feira" Value="Thursday" />
                                            <asp:ListItem Text="Sexta-feira" Value="Friday" />
                                            <asp:ListItem Text="Sábado" Value="Saturday" />
                                            <asp:ListItem Text="Domingo" Value="Sunday" />
                                        </asp:CheckBoxList>
                                    </div>
                                    </li>

                                   <!--Horas para trabalhar-->
                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                        <div class="col-sm-3">
                                            <p class="mb-0 text-center">Horas:</p>
                                        </div>
                                        <div class="col-sm-9">
                                            Inicio:<asp:DropDownList ID="ddlHoraInicio" runat="server" CssClass="form-control" AppendDataBoundItems="false"></asp:DropDownList>
                                          
                                            Fim:<asp:DropDownList ID="ddlHoraFim" runat="server" CssClass="form-control" AppendDataBoundItems="false"></asp:DropDownList>
                                        </div>
                                    </li>


                                    <li class="list-group-item d-flex justify-content-between align-items-center p-3">
                                    <div class="col-sm-3">
                                        <p class="mb-0 text-center">Descriçao Comercial</p>
                                    </div>
                                    <div class="col-sm-9">
                                         <asp:TextBox ID="tb_descricao_psicologo" CssClass="form-control" runat="server" BorderStyle="None" Width="250px"  TextMode="MultiLine" placeholder="Breve Descriçao para o seu serviço" style="resize: none;" Height="80px"></asp:TextBox>
                                    </div>
                                    </li>
                                    
                                </ul>
                            </div>
                        </div>


                    </div>
                    <div class="col-lg-8">
                        <div class="card mb-4">
                                     <div class="col">
                                        <p class="p-2 text-center text-bg-secondary">PERFIL PESSOAL</p>
                                    </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Nome Completo</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_nome" runat="server" BorderStyle="None" Width="600px" CssClass="form-control" placeholder="Insira seu nome"></asp:TextBox>
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
                                        <p class="mb-0">Carteira profissional</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:Label ID="lbl_cp" runat="server"></asp:Label>
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
                                        <asp:TextBox ID="tb_data_nasc" runat="server" BorderStyle="None" placeholder="dd-mm-aaaa" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                        
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Telemovel</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_telemovel" runat="server" BorderStyle="None" Width="372px" CssClass="form-control" placeholder="Insira seu nª telemovel"></asp:TextBox>
                                    </div>
                                </div>

                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Morada</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_morada" runat="server" BorderStyle="None" Width="372px" CssClass="form-control" TextMode="MultiLine" placeholder="Insira a sua morada" style="resize: none;" Height="80px"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">NIF</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_nif" runat="server" BorderStyle="None" Width="372px" CssClass="form-control" placeholder="Insira seu NIF"></asp:TextBox>
                                    </div>
                                </div>
                                <hr>
                                <div class="row">
                                    <div class="col-sm-2">
                                        <p class="mb-0">Descrição pessoal</p>
                                    </div>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="tb_descricao" runat="server" BorderStyle="None" Width="372px" CssClass="form-control" TextMode="MultiLine" placeholder="Escreva algo sobre si" style="resize: none;" Height="80px"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                             <!--aviso gerais-->
                             <asp:Label ID="lbl_msg" runat="server"></asp:Label>
                    </div>
                </div>
                <br />
             


                  <!--Botoes-->
                <div class="row">
                    <div class="col">
                        <div class="d-flex justify-content-center mb-2">
                            <asp:Button ID="btn_guardarAll" class="btn btn-primary" runat="server" Text="Guardar Alteraçoes" OnClick="btn_guardarAll_Click" />
                            <asp:Button ID="btn_cancelar" class="btn btn-outline-primary ms-1" runat="server" Text="Cancelar" OnClick="btn_cancelar_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </main>
</asp:Content>
