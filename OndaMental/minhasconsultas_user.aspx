<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="minhasconsultas_user.aspx.cs" Inherits="OndaMental.minhasconsultas_user" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <main id="main  ">
        <section class="breadcrumbs" style="background-color: #eee;">
            


            <div class="container">
                <asp:GridView ID="GridViewConsultas" runat="server" AutoGenerateColumns="False"
                              CssClass="table table-striped table-hover">
                    <Columns>
                        <asp:BoundField DataField="EmailPsicologo" HeaderText="E-mail Psicólogo" />
                        <asp:BoundField DataField="Titulo" HeaderText="Título" />
                        <asp:BoundField DataField="Data" HeaderText="Data" />
                        <asp:BoundField DataField="HoraInicio" HeaderText="Inicio" />
                        <asp:BoundField DataField="HoraFim" HeaderText="Fim" />
                        <asp:TemplateField HeaderText="Vídeo">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonVideo" runat="server" CssClass="btn btn-primary"
                                    OnClientClick='<%# "return abrirURLvideo(\"" + Eval("UrlMeet") + "\");" %>'>
                                     <img src="img/meetplay.png" alt="Play" style="width: 30px; height: auto"/>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                       <asp:TemplateField HeaderText="Solicitação">
                         <ItemTemplate>
                    <asp:LinkButton ID="btnCancelarConsulta" runat="server" OnClick="btnCancelarConsulta_Click1" 
                                    CssClass="btn btn-danger" CommandArgument='<%# Eval("UrlEvento") + ";" + Eval("Titulo") + ";" + Eval("Data") + ";" + Eval("HoraInicio") + ";" + Eval("EmailPsicologo") %>'>
                        <i class="bi bi-x-circle"></i> Cancelar
                    </asp:LinkButton>
                </ItemTemplate>
                    </asp:TemplateField>

                    </Columns>
                </asp:GridView>
                 <asp:Button ID="btnAtualizar"  CssClass="btn btn-success mb-3" runat="server" Text="Atualizar Consultas" OnClick="btnAtualizar_Click" />
                <br />
                <asp:Label ID="lbl_mensagem_cancelamento" runat="server" Visible="false" ForeColor="Red"></asp:Label>
            </div>

        


          <!--Javascript para abrir urlmeet em outra pagina-->
            <script type="text/javascript">
                function abrirURLvideo(urlMeet) {
                    window.open(urlMeet, "_blank");
                    return false;
                }
            </script>

        </section>
    </main>



</asp:Content>
