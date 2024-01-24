<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="individual_agendaPSI.aspx.cs" Inherits="OndaMental.individual_agendaPSI" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" rel="stylesheet">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <main id="main  ">
        <section>
           
      
            <br />
            <br />
            <br />
            <br />
            <br />


            <svg style="display: none;">
                <defs>
                    <symbol id="arrow" viewBox="0 0 12 12">
                    </symbol>
                    <symbol id="dot" viewBox="0 0 12 12">
                    </symbol>
                </defs>
            </svg>

            <section class="section flexy__item flexy--column flexy--items-center">
                <header class="header accessible-hide">
                    <h2>Infinite Calendar</h2>
                </header>
                <article class="section__article flexy__item flexy--column flexy--items-center">
                    <header class="header accessible-hide">
                        <h3>A liquid & 'light-weight' calendar without any library.</h3>
                    </header>
                    <div class="calendar">
                        <div class="calendar__month">
                            <ul class="calendar__controls flexy__item flexy--between space--small" id="CALENDAR__CONTROLS"></ul>
                            <table class="calendar__table flexy__item flexy--column" id="CALENDAR__TABLE" width="100%" height="100%">
                                <thead class="calendar__thead" id="CALENDAR__THEAD"></thead>
                                <tbody class="calendar__tbody" id="CALENDAR__TBODY"></tbody>
                            </table>
                        </div>
                    </div>
                </article>



            </section>


            <div class="col-12" style="display: flex;">
                <div class="col-4" id="eventosContainer" style=" display: flex; text-align: center;">
                    <table id="tabelaEventos">
                        <tbody>
                            <!-- Os dados serão inseridos aqui -->
                        </tbody>
                    </table>
                </div>

                <div class="col-4 progress-ct">

                    <div class="progress-bg">
                        <div class="progress-bar">
                            <span id="valorArrecadado" class="valorArrecadado"></span>
                        </div>
                        <span id="meta" class="meta"></span>
                    </div>
                </div>
                <div class="col-4"></div>
            </div>
            <script>



                window.onload = function () {
                    renderDayNames(dayNames);
                    renderMonth(year, month);

                }


                function atualizarProgresso(valorArrecadado, meta) {
                    var porcentagem = (valorArrecadado / meta) * 100;
                    document.querySelector('.progress-bar').style.width = porcentagem + '%';
                    document.getElementById('meta').innerText = 'Meta: ' + meta.toLocaleString() + '€';
                    document.getElementById('valorArrecadado').innerText = valorArrecadado.toLocaleString() + '€';
                }

                function preencherTabelaEventos(thisYear, thisMonth) {
                    var tabela = document.getElementById("tabelaEventos").getElementsByTagName('tbody')[0];

                    // Limpar tabela
                    while (tabela.firstChild) {
                        tabela.removeChild(tabela.firstChild);
                    }

                    var eventosMes = 0;
                    var eventosPassadosMes = 0;
                    var hoje = new Date();
                    hoje.setHours(0, 0, 0, 0); // Comparar apenas as datas

                    // Calcular eventos totais e eventos passados do mês
                    dadosEventos.forEach(function (evento) {
                        var eventoDate = new Date(evento.Data);
                        if (eventoDate.getFullYear() === thisYear && eventoDate.getMonth() === thisMonth) {
                            eventosMes += evento.QuantidadeEventos;


                            if (eventoDate < hoje) {
                                eventosPassadosMes += evento.QuantidadeEventos;
                            }
                        }
                    });


                    $.ajax({
                        type: "POST",
                        url: "individual_agendaPSI.aspx/CalcularTotalMensal",
                        data: JSON.stringify({ eventosMes: eventosMes }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var meta = extrairValorNumerico(response.d);
                            var valorArrecadado = (meta / eventosMes) * eventosPassadosMes ? (meta / eventosMes) * eventosPassadosMes : 0;

                            var row = tabela.insertRow();

                            var cellQuantidade = row.insertCell(0);

                            cellQuantidade.className = 'td-mes';
                            cellQuantidade.innerHTML = 'Evento no Mês: ' + eventosMes;

                            atualizarProgresso(valorArrecadado, meta);
                        },
                        error: function (error) {
                            console.log(error);
                        }
                    });
                }


                function extrairValorNumerico(valor) {
                    var numero = valor.replace(/[^\d,]/g, '').replace(',', '.');
                    return parseFloat(numero);
                }

                function formatarValorMoeda(valor) {
                    return valor.toFixed(2).replace('.', ',') + ' €';
                }






                var date = new Date(),
                    year = date.getFullYear(),
                    month = date.getMonth(),
                    yearCounter = year,
                    monthCounter = month,
                    calendarThead = document.getElementById('CALENDAR__THEAD'),
                    calendarTbody = document.getElementById('CALENDAR__TBODY'),
                    calendarControls = document.getElementById('CALENDAR__CONTROLS'),
                    monthNames = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                    dayNames = ['Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab', 'Dom'],
                    formatMonth;

                var getDaysInMonth = function (month, year) {
                    return new Date(year, month + 1, 0).getDate();
                };

                function monthStartDay(thisYear, thisMonth) {
                    var date = new Date(thisYear, thisMonth, 1);
                    var startDay = date.getDay();

                    if (startDay == 0) {
                        startDay = 7;
                    }

                    return startDay;
                }

                function renderControls(target, year, month) {
                    var controlLi = document.createElement('li'),
                        prevBtn = document.createElement('img'),
                        nextLi = document.createElement('li'),
                        nextBtn = document.createElement('img'),
                        todayBtn = document.createElement('img'),
                        titleLi = document.createElement('li'),
                        heading = document.createElement('header'),
                        title = document.createElement('h3');

                    prevBtn.className = 'btn-img-alt';
                    prevBtn.src = 'img/setas/Esquerda.png';

                    nextBtn.className = 'btn-img-alt';
                    nextBtn.src = 'img/setas/Direita.png';

                    todayBtn.className = 'btn-img-alt';
                    todayBtn.src = 'img/setas/Voltar.png';


                    titleLi.className = 'flexy__child';
                    prevBtn.id = 'CALENDAR__CONTROLS__PREV';
                    nextBtn.id = 'CALENDAR__CONTROLS__NEXT';
                    controlLi.className = 'flexy__item flexy--items-center';
                    nextLi.className = 'flexy__item flexy--items-center';

                    controlLi.appendChild(prevBtn);
                    controlLi.appendChild(todayBtn);
                    controlLi.appendChild(nextBtn);

                    title.innerHTML = '<span class="calendar__controls__button__year">' + year + ' .</span>' + monthNames[month];
                    heading.appendChild(title);
                    titleLi.appendChild(heading);

                    target.appendChild(titleLi);
                    target.appendChild(controlLi);

                    prevBtn.addEventListener('click', changeMonth);
                    nextBtn.addEventListener('click', changeMonth);
                    todayBtn.addEventListener('click', goToday);
                }


                function renderDayNames(namesArray) {
                    var namesRow = document.createElement('tr');

                    namesRow.setAttribute('class', 'calendar__month__week flexy__item');

                    for (var i = 0; i < namesArray.length; i++) {
                        var thDay = document.createElement('th');
                        thDay.setAttribute('class', 'calendar__month__day');
                        thDay.innerHTML = namesArray[i];
                        namesRow.appendChild(thDay);
                    }

                    calendarThead.appendChild(namesRow);
                }



                function goToday() {
                    calendarTbody.setAttribute('class', 'calendar__tbody calendar__tbody--animate');
                    setTimeout(function () {
                        newMonth(year, month);
                    }, 450);
                    setTimeout(function () {
                        calendarTbody.setAttribute('class', 'calendar__tbody');
                    }, 900);
                    yearCounter = year;
                    monthCounter = month;
                }

                function changeMonth() {
                    if (this.id.split('NEXT').length > 1) {
                        if (monthCounter < 11) {
                            monthCounter++;
                        } else {
                            monthCounter = 0;
                            yearCounter++;
                        }
                    } else if (this.id.split('PREV').length > 1) {
                        if (monthCounter > 0) {
                            monthCounter--;
                        } else {
                            monthCounter = 11;
                            yearCounter--;
                        }
                    }
                    calendarTbody.setAttribute('class', 'calendar__tbody calendar__tbody--animate');
                    setTimeout(function () {
                        newMonth(yearCounter, monthCounter);
                    }, 450);
                    setTimeout(function () {
                        calendarTbody.setAttribute('class', 'calendar__tbody');
                    }, 900);
                }



                function renderMonth(thisYear, thisMonth) {
                    // Obter o número de dias no mês e o dia da semana em que o mês começa
                    var days = getDaysInMonth(thisMonth, thisYear),
                        startDay = monthStartDay(thisYear, thisMonth),
                        monthRow = document.createElement('tr'); // Criar uma linha da tabela para o mês
                    monthRow.setAttribute('id', 'CALENDAR__ROW');
                    monthRow.setAttribute('class', 'calendar__month__week flexy__item');

                    // Iterar sobre todos os dias do mês
                    for (var i = 1; i < (days + startDay); i++) {
                        var cellDay = document.createElement('td'); // Criar uma célula para cada dia
                        cellDay.setAttribute('class', 'calendar__month__day');

                        if (i >= startDay) { // Se for um dia válido do mês (não um espaço em branco)
                            var day = i - startDay + 1; // Calcular o número do dia
                            var formattedDate = thisYear + '-' + (thisMonth < 9 ? '0' : '') + (thisMonth + 1) + '-' + (day < 10 ? '0' + day : day);

                            // Verificar se existe algum evento para a data
                            var hasEvent = dadosEventos.some(evento => evento.Data === formattedDate);


                            if (hasEvent) { // Se houver eventos na data
                                var eventCount = dadosEventos.reduce((count, evento) => evento.Data === formattedDate ? count + evento.QuantidadeEventos : count, 0);

                                var link = document.createElement('a'); // Criar um link
                                link.href = 'consulta_diaPSI.aspx?ano=' + thisYear + '&mes=' + (thisMonth + 1) + '&dia=' + day; // Definir o destino do link
                                cellDay.appendChild(link); // Adicionar o link à célula

                                var circleDiv = document.createElement('div'); // Criar um div para o círculo do evento
                                circleDiv.className = 'day-circle';
                                var numberSpan = document.createElement('span'); // Criar um span para o número de eventos
                                numberSpan.className = 'event-number';
                                numberSpan.textContent = eventCount;
                                circleDiv.appendChild(numberSpan); // Adicionar o número de eventos ao círculo
                                link.appendChild(circleDiv); // Adicionar o círculo ao link

                                var timeTag = document.createElement('time'); // Criar um elemento time para a data
                                timeTag.setAttribute('datetime', formattedDate);
                                timeTag.innerHTML = day;
                                link.appendChild(timeTag); // Adicionar o elemento time ao link
                            } else { // Se não houver eventos
                                cellDay.setAttribute('class', 'calendar__month__day dia__normal');
                                var timeTag = document.createElement('time'); // Criar um elemento time para a data
                                timeTag.setAttribute('datetime', formattedDate);
                                timeTag.innerHTML = day;
                                cellDay.appendChild(timeTag); // Adicionar o elemento time à célula
                            }
                            cellDay.setAttribute('id', 'CALENDAR__DAY--' + day); // Definir o ID da célula
                        } else {
                            cellDay.innerHTML = '-'; // Para os espaços em branco antes do início do mês
                        }

                        monthRow.appendChild(cellDay); // Adicionar a célula à linha do mês
                    }

                    calendarTbody.appendChild(monthRow);
                    renderControls(calendarControls, thisYear, thisMonth);

                    // preenche quantos eventos existe aquele mes
                    preencherTabelaEventos(thisYear, thisMonth);

                    var today = document.getElementById('CALENDAR__DAY--' + (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()));

                    if (thisMonth === date.getMonth() && thisYear === date.getFullYear()) {
                        setTimeout(function () {
                            today.setAttribute('class', 'calendar__month__day today');
                        }, 200);
                    }
                }

                function newMonth(year, month) {
                    var controls = document.getElementById('CALENDAR__CONTROLS'),
                        row = document.getElementById('CALENDAR__ROW');

                    while (controls.firstChild) {
                        controls.removeChild(controls.firstChild);
                    }

                    calendarTbody.removeChild(row);
                    renderMonth(year, month);
                }


            </script>


            <style>
                .td-mes {
                    font-size: x-small;
                    color: black;
                }

                .conteudo {
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    width: 100%;
                }



                .espaco-vazio {
                    flex-grow: 1; /* Cresce para empurrar a barra de progresso para o centro */
                }

                .progress-ct {
                    display: flex;
                    justify-content: center;
                }

                .progress-bg {
                    position: relative;
                    width: 400px;
                    height: 30px;
                    background-color: #eee;
                    border-radius: 10px;
                    box-shadow: inset 0 0 10px #ccc;
                    text-align: right;
                }

                .progress-bar {
                    width: 0%;
                    height: 30px;
                    background-color: #4caf50;
                    border-radius: 10px;
                    transition: width 2s;
                    display: flex;
                    align-items: center;
                    padding-left: 10px; /* Espaço para o texto dentro da barra */
                }

                .valorArrecadado, .meta {
                    color: #fff;
                    font-weight: bold;
                }

                .meta {
                    font-size: x-small;
                    position: absolute;
                    right: 10px;
                    top: 78%;
                    transform: translateY(-50%);
                    color: black;
                }


                html, body, div, span, applet, object, iframe,
                h1, h2, h3, h4, h5, h6, p, blockquote, pre,
                a, abbr, acronym, address, big, cite, code,
                del, dfn, em, img, ins, kbd, q, s, samp,
                small, strike, strong, sub, sup, tt, var,
                b, u, i, center,
                dl, dt, dd, ol, ul, li,
                fieldset, form, label, legend,
                table, caption, tbody, tfoot, thead, tr, th, td,
                article, aside, canvas, details, embed,
                figure, figcaption, footer, header, hgroup,
                menu, nav, output, ruby, section, summary,
                time, mark, audio, video {
                    margin: 0;
                    padding: 0;
                    border: 0;
                    font: inherit;
                    font-size: 100%;
                    vertical-align: baseline;
                }

                html {
                    line-height: 1;
                }

                ol, ul {
                    list-style: none;
                }

                table {
                    border-collapse: collapse;
                    border-spacing: 0;
                }

                caption, th, td {
                    text-align: left;
                    font-weight: normal;
                    vertical-align: middle;
                }

                q, blockquote {
                    quotes: none;
                }

                    q:before, q:after, blockquote:before, blockquote:after {
                        content: "";
                        content: none;
                    }

                a img {
                    border: none;
                }

                article, aside, details, figcaption, figure, footer, header, hgroup, main, menu, nav, section, summary {
                    display: block;
                }


                /* COLORS */
                /* ACELERATIONS */
                /* TIMING */
                /* SIZES */
                /* FLEXY BEHAVIOR */
                .flexy__item {
                    display: flex;
                    flex-wrap: nowrap;
                }

                .flexy__child {
                    flex-grow: 1;
                    flex-shrink: 1;
                    flex-basis: 0;
                }

                .flexy__child--fill {
                    flex-basis: 100%;
                }

                .flexy__item, .flexy_child {
                    box-sizing: border-box;
                }

                .flexy--row {
                    flex-direction: row;
                }

                .flexy--column {
                    flex-direction: column;
                }

                .flexy--between {
                    justify-content: space-between;
                }

                .flexy--around {
                    justify-content: space-around;
                }

                .flexy--justify-start {
                    justify-content: flex-start;
                }

                .flexy--justify-end {
                    justify-content: flex-end;
                }

                .flexy--justify-center {
                    justify-content: center;
                }

                .flexy--items-center {
                    align-items: center;
                }

                .flexy--self-end {
                    align-self: flex-end;
                }

                .flexy--reverse-column {
                    flex-direction: column-reverse;
                }

                .flexy--reverse-row {
                    flex-direction: row-reverse;
                }

                html,
                body,
                main {
                    height: 100vh;
                    width: 100vw;
                    box-sizing: border-box;
                    -webkit-overflow-scrolling: touch;
                }

                body {
                    color: #FFFFFF;
                    font-family: 'Sintony', sans-serif;
                    -webkit-font-smoothing: antialiased;
                    -webkit-tap-highlight-color: transparent;
                    line-height: 1.4em;
                    background-image: linear-gradient(135deg, #1089cf, #ffffff);
                    overflow: hidden;
                }

                main {
                    position: relative;
                    padding: 2.8em;
                    z-index: 2;
                    overflow: auto;
                }

                @media (max-width: 40em) {
                    main {
                        padding: 0;
                    }
                }

                .section {
                    width: 100%;
                }

                    .section > .header {
                        margin-bottom: 1.4em;
                    }

                .section__article {
                    width: 100%;
                    box-sizing: border-box;
                    color: #404040;
                }

                em,
                strong {
                    font-weight: 700;
                }

                h1, h2, h3, h4, h5 {
                    color: #FFFFFF;
                    line-height: 1.4em;
                    font-family: 'Playfair Display';
                    font-style: italic;
                    font-weight: 700;
                }

                h2 {
                    font-size: 3em;
                }

                h3 {
                    font-size: 1.5em;
                }

                a {
                    text-decoration: none;
                }

                main > .header {
                    order: 3;
                    text-align: center;
                }

                .accessible-hide {
                    position: absolute;
                    overflow: hidden;
                    height: 0;
                    text-indent: -900em;
                }

                @-webkit-keyframes logoAnim {
                    45%, 55% {
                        stroke-dashoffset: 10em;
                    }
                }

                @keyframes logoAnim {
                    45%, 55% {
                        stroke-dashoffset: 10em;
                    }
                }

                .logo-tadaima {
                    height: 3.75em;
                }

                .logo-tadaima--animated {
                    stroke-dasharray: 10em .2em 10em .2em 10em .2em 10em;
                    stroke-dashoffset: 0;
                    -webkit-animation: logoAnim 9s cubic-bezier(0.54, -0.24, 0.46, 1.28) infinite;
                    animation: logoAnim 9s cubic-bezier(0.54, -0.24, 0.46, 1.28) infinite;
                }

                .svg-bg {
                    width: 100%;
                    height: 100%;
                    position: fixed;
                    left: 0;
                    top: 0;
                    z-index: 1;
                }

                .svg-icon {
                    width: 1em;
                    height: 1em;
                    transform-origin: 50% 50%;
                }

                .svg-icon__sprite {
                    display: none;
                }

                .svg-icon--rotated-180 {
                    transform: rotate(180deg);
                }

                .space--small {
                    margin-bottom: 0.7em;
                }

                @media (max-width: 75em) {
                    .space--small {
                        margin-bottom: 0.518em;
                    }
                }

                .space--normal {
                    margin-bottom: 1.4em;
                }

                @media (max-width: 75em) {
                    .space--normal {
                        margin-bottom: 1.036em;
                    }
                }

                .space--medium {
                    margin-bottom: 2.8em;
                }

                @media (max-width: 75em) {
                    .space--medium {
                        margin-bottom: 2.072em;
                    }
                }

                .space--big {
                    margin-bottom: 4.2em;
                }

                @media (max-width: 75em) {
                    .space--big {
                        margin-bottom: 3.108em;
                    }
                }

                .space--huge {
                    margin-bottom: 5.6em;
                }

                @media (max-width: 75em) {
                    .space--huge {
                        margin-bottom: 4.144em;
                    }
                }

                .color--white {
                    color: #FFFFFF;
                }

                .fill--white {
                    fill: #FFFFFF;
                }

                .stroke--white {
                    stroke: #FFFFFF;
                }

                .color--black {
                    color: #000000;
                }

                .fill--black {
                    fill: #000000;
                }

                .stroke--black {
                    stroke: #000000;
                }

                .color--red {
                    color: #FF5572;
                }

                .fill--red {
                    fill: #FF5572;
                }

                .stroke--red {
                    stroke: #FF5572;
                }

                .color--coral {
                    color: #FF6960;
                }

                .fill--coral {
                    fill: #FF6960;
                }

                .stroke--coral {
                    stroke: #FF6960;
                }

                .color--orange {
                    color: #FF7555;
                }

                .fill--orange {
                    fill: #FF7555;
                }

                .stroke--orange {
                    stroke: #FF7555;
                }

                .color--gray {
                    color: #404040;
                }

                .fill--gray {
                    fill: #404040;
                }

                .stroke--gray {
                    stroke: #404040;
                }

                @-webkit-keyframes changeMonth {
                    50% {
                        transform: translate(0, 120%);
                    }
                }

                @keyframes changeMonth {
                    50% {
                        transform: translate(0, 120%);
                    }
                }

                .calendar {
                    width: 35em;
                    height: 21.75em;
                    position: relative;
                    font-size: x-large;
                }

                @media (max-width: 40em) {
                    .calendar {
                        width: 100%;
                    }
                }

                .calendar__table {
                    width: 100%;
                    height: 23.875em;
                }

                    .calendar__table tr {
                        width: 100%;
                        font-size: x-large;
                    }

                .calendar__tbody, .calendar__thead {
                    display: inline-block;
                    position: relative;
                    box-sizing: border-box;
                    width: 100%;
                }

                .calendar__thead {
                    height: 3.813em;
                    padding: 0.7em;
                    z-index: 2;
                    background-color: #000000;
                    color: #FFFFFF;
                    box-shadow: 0 0.1em 1em 0 rgba(0, 0, 0, 0.25), 0 0.1em 0.5em 1px rgba(0, 0, 0, 0.35), 0 1em 2em 1px rgba(0, 0, 0, 0.15);
                    font-size: large;
                }

                    .calendar__thead .calendar__month__day {
                        font-size: .75em;
                        text-transform: uppercase;
                    }

                .calendar__tbody {
                    height: 19.438em;
                    background-color: #ffffff;
                    padding: 1.4em 0.7em;
                    transform: translateY(0);
                    z-index: 1;
                    box-shadow: 0 0.1em 1em 0 rgba(0, 0, 0, 0.25), 0 0.1em 0.5em 1px rgba(0, 0, 0, 0.35), 0 1em 2em 1px rgba(0, 0, 0, 0.15);
                    font-size: 1.1em;
                }

                .calendar__tbody--animate {
                    -webkit-animation: changeMonth 0.9s ease-in-out forwards;
                    animation: changeMonth 0.9s ease-in-out forwards;
                }

                .calendar__month {
                    width: 100%;
                    box-sizing: border-box;
                    padding: 0 2.8em;
                    overflow: hidden;
                    position: relative;
                    font-size: large;
                }

                    .calendar__month:after {
                        content: '';
                        width: 100%;
                        height: 3em;
                        border-radius: 10%;
                        position: absolute;
                        z-index: 3;
                        bottom: -3em;
                        left: 0;
                        box-shadow: 0 -1em 1em -1em rgba(0, 0, 0, 0.6), 0 0 4em -1em rgba(0, 0, 0, 0.2);
                    }

                .calendar__month__day {
                    border: none;
                    display: inline-block;
                    padding: 0.7em 0;
                    text-align: center;
                    box-sizing: border-box;
                    flex-basis: 14.28571%;
                    background-image: radial-gradient(#ff7555 50%, rgba(0, 0, 0, 0) 53%);
                    background-size: 0 0;
                    background-repeat: no-repeat;
                    background-position: 50% 50%;
                    transition: all 0.3s cubic-bezier(0.54, -0.24, 0.46, 1.28);
                    transition-delay: 0.3s;
                }

                    .calendar__month__day time {
                        display: inline-block;
                    }

                    .calendar__month__day.today {
                        color: #FFFFFF;
                        background-size: 3.1em 3.1em;
                    }

                @media (max-width: 40em) {
                    .calendar__month__day.today {
                        background-size: 3em 3em;
                    }
                }

                .calendar__month__week {
                    flex-wrap: wrap;
                }

                .calendar__controls {
                    color: #FFFFFF;
                    font-size: x-large;
                }

                .calendar__controls__button {
                    padding: 0.7em 0.35em;
                    cursor: pointer;
                }

                .calendar__controls__button__year {
                    top: -.5em;
                    position: relative;
                    opacity: .6;
                    font-size: .85em;
                    padding-right: 0.28em;
                }

                .day-circle {
                    width: 13px;
                    height: 13px;
                    border-radius: 50%;
                    background-color: #24e100;
                    display: inline-block;
                    margin-left: -13px;
                }

                .event-number {
                    top: -70%;
                    transform: translate(-50%, -50%);
                    font-size: x-small;
                    position: relative;
                }

                .dia__normal {
                    background-color: #ffffff;
                    color: gray;
                }

                .btn-img-alt:hover {
                    border: inset;
                    border-color: skyblue;
                }

                .btn-img-alt {
                    width: 21px;
                    height: 21.6px;
                    margin-left: 4px;
                    cursor: pointer;
                }
            </style>






        </section>
    </main>

</asp:Content>
