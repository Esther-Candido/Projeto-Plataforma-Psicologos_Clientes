<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Selecionar_email.aspx.cs" Inherits="OndaMental.Selecionar_email" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main  ">
	
     <section class="breadcrumbs vh-100 bg-image" style="background-image: url('assets/img/about.jpg');">

  <div class="mask d-flex align-items-center h-100 gradient-custom-3">
    <div class="container h-100">
      <div class="row d-flex justify-content-center align-items-center h-100">
        <div class="col-12 col-md-9 col-lg-7 col-xl-6">
          <div class="card" style="border-radius: 15px;">
            <div class="card-body p-5">
              <h2 class="text-uppercase text-center mb-5">Recuperaçao de Conta</h2>

              <div>
                 
                <div class="form-outline mb-4">
                    <label class="form-label" for="form3Example3cg">Insira o seu Email</label>
                  <asp:TextBox ID="tb_email" runat="server" class="form-control form-control-lg"></asp:TextBox>                 
                </div>

                 <asp:Label ID="lbl_msg" runat="server" ></asp:Label>

                
                <div class="d-flex justify-content-center">
                <asp:Button ID="tb_enviar" runat="server" Text="Enviar" class="btn btn-success" OnClick="tb_enviar_Click"/>
                </div>

                <p class="text-center text-muted mt-5 mb-0">Deseja voltar a trás? <a href="Login.aspx"
                    class="fw-bold text-body"><u>Voltar</u></a></p>         
              </div>

            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</section>
		
		</main>
</asp:Content>
