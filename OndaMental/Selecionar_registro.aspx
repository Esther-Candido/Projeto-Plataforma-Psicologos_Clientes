<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Selecionar_registro.aspx.cs" Inherits="OndaMental.Selecionar_registro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main  ">
        <section class="breadcrumbs ">
            <div class="mask d-flex align-items-center h-100 gradient-custom-3">
                <div class="container h-100">
                    <div class="row d-flex justify-content-center align-items-center h-100 py-5">
                        <div class="col-12 col-md-9 col-lg-7 col-xl-6 ">
                            <div class="card" style="border-radius: 15px;">
                                <div class="card-body">
                                    <p class="text-center">Paciente</p>
                                    <a href="Registro.aspx">
                                    <asp:Image src="assets/img/img_selecionar_user.jpg" class="img-fluid" runat="server" ></asp:Image>
                                </a>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-9 col-lg-7 col-xl-6">
                            <div class="card" style="border-radius: 15px;">
                                <div class="card-body ">
                                    <p class="text-center">Psicologo</p>
                                   <a href="Registro_psicologo.aspx">
                                    <asp:Image src="assets/img/img_selecionar_psicologo.jpg" class="img-fluid" runat="server" ></asp:Image>
                                </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </section>
    </main>
</asp:Content>
