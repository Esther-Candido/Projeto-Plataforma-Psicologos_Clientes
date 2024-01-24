<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Index.aspx.cs" Inherits="OndaMental.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main  ">
        <section class="breadcrumbs ">

            <!-- Carousel Psicologos -->
            <asp:Repeater ID="rpt_card_psicologos" runat="server" OnItemCommand="rpt_card_psicologos_ItemCommand">
                <HeaderTemplate>
                    <section class="pt-5 pb-5">
                        <div class="container">
                            <div class="row">
                                <div class="col-6"> 
                                    <h3 class="mb-3">Top Psicólogos</h3>
                                </div>
                                <!--
                            <div class="col-6 text-right">
                                <a class="btn btn-primary mb-3 mr-1" href="#carouselExampleIndicators2" role="button" data-slide="prev">
                                    <i class="fa fa-arrow-left"></i>
                                </a>
                                <a class="btn btn-primary mb-3 " href="#carouselExampleIndicators2" role="button" data-slide="next">
                                    <i class="fa fa-arrow-right"></i>
                                </a>
                            </div>  -->
                                <div class="col-12">
                                    <div id="carouselExampleIndicators2" class="carousel slide" data-ride="carousel">
                                        <div class="carousel-inner">
                </HeaderTemplate>


                <ItemTemplate>
                    <%# Container.ItemIndex % 4 == 0 ? "<div class='carousel-item " + (Container.ItemIndex == 0 ? "active" : "") + "'><div class='row'>" : "" %>
                    <div class="col-md-3 mb-3">
                        <div class="card">
                            <div class="card-img-container">
                                <img class="card-img-top img-fluid px-3 pt-3" alt="100%x280" src='<%# (Eval("dadosBinarios") as byte[]) != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("dadosBinarios")) : "img/semfoto.png" %>'>
                            </div>
                          <div class="card-body">
                            <h4 class="card-title text-center"><%#Eval("utilizador") %></h4>
                            <p class="card-text text-center"><%#Eval("descricao") %></p>
                                <asp:LinkButton ID="lnk_informacao" CssClass="btn btn-primary" runat="server" Text="Mais Informações" CommandName="MostrarDetalhes" CommandArgument='<%#Eval("id") %>' />
                            </div>
                        </div>
                    </div>
                    <%# (Container.ItemIndex + 1) % 4 == 0 ? "</div></div>" : "" %>
                </ItemTemplate>



                <FooterTemplate>
                    </div>
               <div class="row">
                   <div class="col-6">
                       <h3 class=""></h3>
                   </div>
               </div>
                    </div>
                    </div>
                </section>
                </FooterTemplate>
            </asp:Repeater>
            <!-- Fim carousel-->


             <!-- Noticias da saude aqui-->
            <asp:Xml ID="Xml1" runat="server" TransformSource="~/noticias_saude.xslt"></asp:Xml>


         
            <!-- Seção para Listar Todos os Psicólogos -->
            <div class="container mt-5">
                <h2>Mais Profissionais</h2>
                <div class="row">
                    <asp:Repeater ID="rptTodosPsicologos" runat="server" OnItemCommand="rptTodosPsicologos_ItemCommand">
                        <ItemTemplate>
                            <div class="col-md-3 mb-3">
                                <!-- Alterado de col-md-3 para col-md-4 -->
                                <div class="card">
                                    <div class="card-img-container">
                                        <img class="card-img-top img-fluid px-3 pt-3" alt="Psicólogo" src='<%# (Eval("dadosBinarios") as byte[]) != null ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])Eval("dadosBinarios")) : "img/semfoto.png" %>'>
                                    </div>
                                    <div class="card-body">
                                        <h4 class="card-title text-center"><%#Eval("utilizador") %></h4>
                                        <p class="card-text text-center"><%#Eval("descricao") %></p>
                                        <asp:LinkButton ID="lnkInfo_todosPsico" CssClass="btn btn-primary" runat="server" Text="Mais Informações" CommandName="MostrarDetalhes_todos" CommandArgument='<%# Eval("id") %>' />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>



             <!--JAVASCRIPT DO CHAT TALK.TO-->
            <script type="text/javascript">
                var Tawk_API = Tawk_API || {}, Tawk_LoadStart = new Date();
                (function () {
                    var s1 = document.createElement("script"), s0 = document.getElementsByTagName("script")[0];
                    s1.async = true;
                    s1.src = 'https://embed.tawk.to/654e8933958be55aeaae82df/1hetcfsh3';
                    s1.charset = 'UTF-8';
                    s1.setAttribute('crossorigin', '*');
                    s0.parentNode.insertBefore(s1, s0);
                })();
            </script>
             <!--FIM JAVASCRIPT DO CHAT TALK.TO-->





                <!--todos os psicologo-->
                <style>
                    .card-img-container {
                        height: 250px; /* Aumentado de 200px para 250px */
                        overflow: hidden;
                    }


                    .card-img-top {
                        width: 100%;
                        height: 100%;
                        object-fit: cover;
                    }


                    .card-img-container img {
                        max-height: 100%; /* Certifica-se de que a imagem não exceda a altura do container */
                        max-width: 100%; /* Certifica-se de que a imagem não exceda a largura do container */
                    }

                    .card {
                        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                    }

                    .container h2 {
                        margin-bottom: 20px;
                    }


                    /*style abaixo para as noticias*/
                    .card h5.card-title {
                        font-size: 1.25rem;
                    }

                    .card p.card-text {
                        font-size: 1rem;
                        overflow: hidden;
                        text-overflow: ellipsis;
                        display: -webkit-box;
                        -webkit-line-clamp: 3;
                        -webkit-box-orient: vertical;
                    }

                    .card .btn-primary {
                        margin-top: auto; /* Coloca o botão na parte inferior do card */
                    }

                    .card {
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        height: 100%; /* Define a altura do card para 100% do container */
    }

                        .card-body {
        display: flex;
        flex-direction: column;
        height: calc(100% - 250px); /* Ajusta a altura para subtrair a altura da imagem */
    }

    .card-title {
        margin-bottom: 10px;
        height: 48px; /* Altura fixa para o título */
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
    }

    .card-text {
        flex-grow: 1; /* Faz o texto ocupar o espaço disponível */
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 3; /* Limita a 3 linhas de texto */
    }

    .btn {
        margin-top: auto; /* Alinha o botão na parte inferior do card */
    }


                </style>





                <!-- Pertence ao carousel-->
            <link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
            <link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css">
            <script type="text/javascript" src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
            <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.0/umd/popper.min.js"></script>
            <script type="text/javascript" src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>
        </section>
        <!-- End Contact Section -->
    </main>
    <!-- End #main -->
</asp:Content>
