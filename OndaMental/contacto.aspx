<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="contacto.aspx.cs" Inherits="OndaMental.contacto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
    <main id="main">
        <section class="breadcrumbs" style="background-color: #fff;">

        <section class="container my-4 text-center">
            <div class="row">
                <div class="col-lg-12">
                    <h3 class="text-center">Fale Conosco</h3>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-lg-12">
                    <p><a href="mailto:info@ondamental.com" class="email-link">info@ondamental.com</a></p>
                    <p><a href="mailto:adm@ondamental.com" class="email-link">adm@ondamental.com</a></p>
                </div>
            </div>

            <div class="row mt-4">
                <div class="col-lg-12">
                    <h3>Horário de Funcionamento</h3>
                    <p>Segunda a Sexta, das 10h às 22h</p>
                </div>
            </div>

          
        </section>
        </section>
    </main>
    
    
    <style>
        .email-link {
            color: #255d3b;; 
            text-decoration: none; 
            transition: color 0.3s ease; 
        }

        .email-link:hover {
            color: #14ef62;
        }
    </style>
    

</asp:Content>
