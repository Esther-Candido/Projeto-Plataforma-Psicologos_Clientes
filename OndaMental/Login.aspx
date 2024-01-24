<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OndaMental.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main  ">
	
     <section class="breadcrumbs vh-100 bg-image">

  <div class="mask d-flex align-items-center h-100 gradient-custom-3">
    <div class="container h-100">
      <div class="row d-flex justify-content-center align-items-center h-100">
        <div class="col-12 col-md-9 col-lg-7 col-xl-6">
          <div class="card" style="border-radius: 15px;">
            <div class="card-body p-5">
              <h2 class="text-uppercase text-center mb-5">Login</h2>

              <div>
                 
                  <asp:Panel ID="Panel1" runat="server" DefaultButton="tb_login">
                      <div class="form-outline mb-4">
                          <label class="form-label" for="form3Example3cg">E-mail</label>
                          <asp:TextBox ID="tb_email" runat="server" class="form-control form-control-lg"></asp:TextBox>
                      </div>

                      <div class="form-outline mb-4">
                          <label class="form-label" for="form3Example4cg">Palavra-Passe</label><asp:TextBox ID="tb_pw" type="password" runat="server" class="form-control form-control-lg"></asp:TextBox>
                      </div>

                      <asp:Label ID="lbl_msg" runat="server" ForeColor="Red"></asp:Label>
                
                      <div class="d-flex justify-content-center">
                          <asp:Button ID="tb_login" runat="server" Text="Login" class="btn btn-success" OnClick="btn_login_Click" />
                      </div>
                  </asp:Panel>


                <p class="text-center text-muted mt-5 mb-0">Se esqueceu da password? <a href="Selecionar_email.aspx"
                    class="fw-bold text-body"><u>Recupere</u></a></p>

                <p class="text-center text-muted mt-5 mb-0">Ainda nao tem conta? <a href="Selecionar_registro.aspx"
                    class="fw-bold text-body"><u>Registrar</u></a></p>
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
