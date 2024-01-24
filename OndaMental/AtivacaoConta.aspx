<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="AtivacaoConta.aspx.cs" Inherits="OndaMental.AtivacaoConta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main  ">
     <section class="breadcrumbs">
         
         
        <div class="container-meu">
          <div class="components-meu">
              <asp:Label ID="lbl_text" runat="server" CssClass="palavra-ativacao"></asp:Label>
          </div>
      </div>

  <style>
   

      .container-meu {
            min-height: 40vh;
          display: flex;
          justify-content: center;
          align-items: center;
          font-family: 'Poppins', sans-serif;
          background: var(--greyLight-1);
      }

      .components-meu {
          border-radius: 3rem;
          box-shadow: 0.8rem 0.8rem 1.4rem var(--greyLight-2), -0.2rem -0.2rem 1.8rem var(--white);
          padding: 4rem;
          display: grid;
          grid-template-rows: repeat(autofit, min-content);
          grid-column-gap: 5rem;
          grid-row-gap: 2.5rem;
          justify-content: center;
          justify-items: center;
      }



      .palavra-ativacao {
          width: 66.4rem;
          border: none;
          border-radius: 1rem;
          font-size: 1.4rem;
          padding-left: 1.4rem;
          box-shadow: $inner-shadow;
          background: none;
          font-family: inherit;
          color: black;
      }
  </style>

     </section>
   </main>
</asp:Content>
