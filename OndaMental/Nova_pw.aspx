<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Nova_pw.aspx.cs" Inherits="OndaMental.Nova_pw" %>
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
              <h2 class="text-uppercase text-center mb-5">Defenir nova password</h2>

              <div>
                 
                <label class="form-label" for="form3Example4cg">Digite Password Nova</label>
                <div class="form-outline mb-4">
                  <asp:TextBox ID="tb_pw_nova" type="password" runat="server" class="form-control form-control-lg"></asp:TextBox>
                  
                </div>

                  <div class="form-outline mb-4">
                  <label class="form-label" for="form3Example4cg">Confirme a Password nova</label>
                  <asp:TextBox ID="tb_pw_nova_confirm" type="password" runat="server" class="form-control form-control-lg"></asp:TextBox>
                  
                </div>
   


                 <asp:Label ID="lbl_msg" runat="server" ></asp:Label>
               

                
                <div class="d-flex justify-content-center">
                <asp:Button ID="tb_alterar_pw" runat="server" Text="Confirmar" class="btn btn-success" OnClick="tb_alterar_pw_Click" />
                </div>

                <p class="text-center text-muted mt-5 mb-0">Deseja voltar ao Login? <a href="User.aspx"
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
