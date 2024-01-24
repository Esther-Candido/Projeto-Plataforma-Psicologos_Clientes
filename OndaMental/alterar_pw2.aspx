<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="alterar_pw2.aspx.cs" Inherits="OndaMental.alterar_pw2" %>
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
              <h2 class="text-uppercase text-center mb-5">Alterar password</h2>

              <div>
                 
                <div class="form-outline mb-4">
                    <!--Senha atual -->
                  <label class="form-label" for="form3Example3cg">Digite Password Atual</label>
                  <asp:TextBox ID="tb_pw_atual" runat="server" class="form-control form-control-lg"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tb_pw_atual" ErrorMessage="&quot;Palavra-Passe antiga é obrigatorio&quot;" ForeColor="Red">*</asp:RequiredFieldValidator>
                </div>

                  <!--senha nova -->
                <label class="form-label" for="form3Example4cg">Digite Password Nova</label>
                <div class="form-outline mb-4">
                  <asp:TextBox ID="tb_pw_nova" type="password" runat="server" class="form-control form-control-lg"></asp:TextBox>
                  
                </div>


                  <!--Confirmar PW nova -->
                  <div class="form-outline mb-4">
                  <label class="form-label" for="form3Example4cg">Confirme a Password nova</label>
                  <asp:TextBox ID="tb_pw_nova_confirm" type="password" runat="server" class="form-control form-control-lg"></asp:TextBox>
                      <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Palavra-Passe nova / confirma são diferentes!" ControlToCompare="tb_pw_nova_confirm" ControlToValidate="tb_pw_nova"></asp:CompareValidator>
                  
                </div>
    


                  <!--labeel mensagem -->
                 <asp:Label ID="lbl_mensagem" runat="server" ></asp:Label>
               <asp:Label ID="lbl_mensagemFRACA" runat="server" ></asp:Label>
                  <asp:Label ID="lbl_senharepetida" runat="server" ></asp:Label>


                <!--Botao confirmar senha -->
                <div class="d-flex justify-content-center">
                <asp:Button ID="tb_alterar_pw" runat="server" Text="Confirmar" class="btn btn-success" OnClick="tb_alterar_pw_Click" />
                </div>


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
