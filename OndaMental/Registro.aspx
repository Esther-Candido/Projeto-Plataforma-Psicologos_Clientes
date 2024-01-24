<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="OndaMental.Registro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main  ">
	
     <section class="breadcrumbs bg-image" >

  <div class="mask d-flex align-items-center h-100 gradient-custom-3">
    <div class="container h-100">
      <div class="row d-flex justify-content-center align-items-center h-100">
        <div class="col-12 col-md-9 col-lg-7 col-xl-6">
          <div class="card" style="border-radius: 15px;">
            <div class="card-body p-5">
              <h2 class="text-uppercase text-center mb-5">Crie Conta</h2>

              <div>
                   <!--Nome-->
                <div class="form-outline mb-4">
                 <label class="form-label" for="form3Example1cg">Nome Completo</label>
                 <asp:TextBox ID="tb_nome" runat="server" class="form-control form-control-lg" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nome é obrigatório" ControlToValidate="tb_nome"></asp:RequiredFieldValidator>
                </div>

                  <!--Email-->
                <div class="form-outline mb-4">
                  <label class="form-label" for="form3Example3cg">E-mail</label>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ControlToValidate="tb_email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                  <asp:TextBox ID="tb_email" runat="server" class="form-control form-control-lg" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="E-mail é obrigatório" ControlToValidate="tb_email"></asp:RequiredFieldValidator>
                </div>

                   <!--Senha-->
                <div class="form-outline mb-4">
                   <label class="form-label" for="form3Example4cg">Palavra-Passe</label>
                  <asp:TextBox ID="tb_pw" type="password" runat="server" class="form-control form-control-lg" ></asp:TextBox>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Palavra-Passe é obrigatório" ControlToValidate="tb_pw"></asp:RequiredFieldValidator>
                </div>

                   <!--Senha confirme-->
                <div class="form-outline mb-4">
                   <label class="form-label" for="form3Example4cdg">Confirme Palavra-Passe</label>
                  <asp:TextBox ID="tb_pw_confirm" type="password" runat="server" class="form-control form-control-lg" ></asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Atenção: Palavra-Passe são diferentes" ControlToCompare="tb_pw_confirm" ControlToValidate="tb_pw"></asp:CompareValidator>
                </div>

                 
                        <!--avisos ativação-->
                         <asp:Label ID="lbl_msg" runat="server" class="form-check-label ms-1"></asp:Label>

                   <!-- Inicio Politica de privacidade-->
                <div class="form-check d-flex justify-content-center mb-5">
                   <asp:CheckBox ID="cb_termos" runat="server" />
                    <asp:Label ID="politicaprivacidade" runat="server" Text="Label"  class="form-check-label ms-1" for="form2Example3g">
                         Concorda com a nossa <a  href="Politica_privacidade.aspx" target="_blank" class="text-body"><u>Politica de privacidade</u></a>
                    </asp:Label>
                    </div>
                   <!--FIM Politica de privacidade-->
             

                
                <div class="d-flex justify-content-center">
                <asp:Button ID="tb_registrar" runat="server" Text="Registrar" class="btn btn-success" OnClick="btn_register_Click"/>
                </div>

                <p class="text-center text-muted mt-5 mb-0">Já tem conta? <a href="Login.aspx"
                    class="fw-bold text-body"><u>Iniciar</u></a></p>

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
