<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Politica_privacidade.aspx.cs" Inherits="OndaMental.Politica_privacidade" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="components">
                <h1 class="col-12"> Política de Privacidade da OndaMental
                </h1>
                <p class="col-12">
                   
                    A OndaMental respeita a privacidade de seus usuários e está comprometida em proteger suas informações pessoais. Esta política de privacidade explica como coletamos, usamos e compartilhamos suas informações pessoais no contexto dos nossos serviços de psicologia online.
                </p>

                <p class="col-12">
                    Coleta de Dados
                    Coletamos informações que você nos fornece diretamente quando se registra e utiliza nossos serviços. Isso inclui:
                </p>

                <p class="col-12">
                    Dados de identificação pessoal, como nome e data de nascimento.
                    Informações de contato, incluindo endereço de e-mail e número de telefone.
                    Dados de saúde relacionados às sessões de psicologia.
                    Informações de pagamento para processamento de serviços pagos.
                    Uso dos Dados
                    Utilizamos suas informações para:
                </p>

                <p class="col-12">
                    Facilitar o agendamento e a realização de sessões de psicologia.
                    Melhorar e personalizar sua experiência em nossa plataforma.
                    Comunicar informações importantes sobre sua conta e nossos serviços.
                    Processar pagamentos e manter registros financeiros.
                    Compartilhamento de Dados
                    Não compartilhamos suas informações pessoais com terceiros, exceto quando necessário para a prestação de nossos serviços (como processamento de pagamentos) ou quando exigido por lei.
                    Todas as informações compartilhadas entre cliente e psicólogo são mantidas em estrita confidencialidade.
                    Segurança dos Dados
                    Implementamos medidas de segurança para proteger suas informações pessoais contra acesso não autorizado, alteração, divulgação ou destruição.
                    Embora nos esforcemos para proteger suas informações, nenhum sistema de segurança é infalível.
                    Alterações na Política de Privacidade
                    Reservamo-nos o direito de modificar esta política de privacidade a qualquer momento. Quaisquer mudanças serão comunicadas de forma clara em nossa plataforma.
                    Contato
                    Em caso de dúvidas ou preocupações sobre esta política de privacidade, entre em contato conosco através do e-mail suporte@OndaMental.com.br
                </p>
            </div>
        </div>

        <style>
            :root {
                --primary-light: #8abdff;
                --primary: #6d5dfc;
                --primary-dark: #5b0eeb;
                --white: #FFFFFF;
                --greyLight-1: #E4EBF5;
                --greyLight-2: #c8d0e7;
                --greyLight-3: #bec8e4;
                --greyDark: #9baacf;
            }

            $shadow: .3rem .3rem .6rem var(--greyLight-2),
            -.2rem -.2rem .5rem var(--white);
            $inner-shadow: inset .2rem .2rem .5rem var(--greyLight-2),
            inset -.2rem -.2rem .5rem var(--white);

            *, *::before, *::after {
                margin: 0;
                padding: 0;
                box-sizing: inherit;
            }

            html {
                box-sizing: border-box;
                font-size: 62.5%;
                // 1rem = 10px 100% = 16px overflow-y: scroll;
                background: var(--greyLight-1);

                @media screen and (min-width: 900px) {
                    font-size: 75%;
                }
            }

            .container {
                min-height: 100vh;
                display: flex;
                justify-content: center;
                align-items: center;
                font-family: 'Poppins', sans-serif;
                background: var(--greyLight-1);
            }

            .components {
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



            p {
                width: 66.4rem;
                border: none;
                border-radius: 1rem;
                font-size: 1.4rem;
                padding-left: 1.4rem;
               
                background: none;
                font-family: inherit;
                color: black;
            }
        </style>
    </form>
</body>
</html>
