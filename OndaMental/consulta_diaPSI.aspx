<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="consulta_diaPSI.aspx.cs" Inherits="OndaMental.consulta_diaPSI" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <br />
    <br />
    <main id="main  ">
        <section>


            <asp:ImageButton ID="btn_voltar_pag" runat="server" ImageUrl="img/setas/voltarPG.png" CssClass="btn-voltar" OnClick="btn_voltar_pag_Click" Width="30px" />

            <asp:Repeater ID="EventosRepeater" runat="server" OnItemCommand="EventosRepeater_ItemCommand">
                <HeaderTemplate>
                    <div class="container">
                        <table>
                            <thead>
                                <tr>
                                    <th>Foto</th>
                                    <th>Email</th>
                                    <th>Data</th>
                                    <th>Inicio</th>
                                    <th>Fim</th>
                                    <th>Video</th>
                                    <th>Funções</th>
                                </tr>
                            </thead>
                            <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:Image ID="ClientImage" runat="server"
                                ImageUrl='<%# Eval("ImgBinario") != null ? "data:image;base64," + Convert.ToBase64String((byte[])Eval("ImgBinario")) : "img/semfoto.png" %>'
                                AlternateText="Imagem do Cliente" CssClass="img_cliente" Width="30px" /></td>
                        <td><%# DataBinder.Eval(Container.DataItem, "EmailClient") %></td>
                        <td>
                            <asp:Label ID="lbl_data" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate", "{0:dd-MM-yyyy}") %>'></asp:Label>

                            <asp:TextBox ID="tb_StartDate" runat="server" Visible="False" TextMode="Date" CssClass="form-control" AutoPostBack="true" OnTextChanged="tb_StartDate_TextChanged" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_horario" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "StartDate", "{0:HH:mm}") %>'></asp:Label>

                            <asp:DropDownList ID="ddlHorarios" runat="server" Visible="False" CssClass="ddl-horarios" />

                        </td>
                        <td>
                            <asp:Label ID="lbl_fim" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FimDate", "{0:HH:mm}") %>'></asp:Label>
                            <asp:Label ID="lbl_fim2" runat="server" Visible="False"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton ID="btn_video" runat="server" ImageUrl="img/video1.png" CssClass="funcao-btn-video" OnClientClick='<%# "window.open(\"" + Eval("url") + "\"); return false;" %>' />

                        </td>
                        <td data-label="Funções">
                            <asp:ImageButton ID="btn_editar" runat="server" ImageUrl="img/editar.png" CommandArgument='<%# Container.ItemIndex %>' CommandName="Editar" CssClass="funcoes_btn" />
                            <asp:ImageButton ID="btn_salvar" runat="server" ImageUrl="img/salvar.png" CommandArgument='<%# Eval("IdEvento") %>' CommandName="Salvar" Visible="False" CssClass="funcoes_btn" />
                            <asp:ImageButton ID="btn_cancelar" runat="server" ImageUrl="img/cancelar.png" CommandArgument='<%# Container.ItemIndex %>' CommandName="Cancelar" Visible="False" CssClass="funcoes_btn" />
                            <asp:ImageButton ID="btn_excluir" runat="server" ImageUrl="img/excluir.png" CommandArgument='<%# Eval("IdEvento") %>' OnClientClick="return confirm('Tem certeza que deseja excluir esta consulta?');" CommandName="Excluir" CssClass="funcoes_btn" />
                    </tr>
                </ItemTemplate>

                <FooterTemplate>
                    </tbody>
        </table>
        </div>
                </FooterTemplate>
            </asp:Repeater>


            <style>
                .btn-voltar {
                    margin-left: 44px;
                    margin-top: 10px;
                }

                .funcao-btn-video {
                    width: 90px !important;
                }

                .funcoes_btn {
                    width: 20px;
                    heigth: 20px;
                }

                body {
                    background-color: #eff3f6;
                }

                .EntregaFeita {
                    color: #0097ff;
                    border-bottom-style: dotted;
                    border-radius: 30%;
                }

                .EntregaNaoFeita {
                    color: #ff0000;
                    border-bottom-style: dotted;
                    border-radius: 30%;
                }




                table {
                    width: 100%;
                    border-collapse: separate;
                    border-spacing: 0px 8px;
                }

                th {
                    text-align: center;
                    padding: 5px;
                    text-transform: uppercase;
                    font-weight: 100;
                    font-size: 11px;
                    color: #aab3bb;
                }

                tr {
                    box-shadow: 1px 1px 1px rgba(228, 228, 228, 0.25);
                }

                    tr thead {
                        background: transparent !important;
                        border-color: transparent !important;
                    }

                    tr td {
                        text-align: center;
                        background-color: #ffffff;
                        border-bottom: 1px solid #e7e7e7;
                        font-size: 12px;
                        font-weight: bold;
                    }

                td {
                    padding: 13px;
                }

                img {
                    height: 30px;
                }

                /* Icons */
                .fa {
                    padding: 0px 9px;
                    color: #c1c4c9;
                    font-size: 1.1em;
                }

                .arrow-right {
                    font-size: 1.9em;
                    float: right;
                }



                .img_cliente {
                    border-radius: 100%;
                    width: 50px;
                    height: 50px;
                }
            </style>
        </section>
    </main>

</asp:Content>
