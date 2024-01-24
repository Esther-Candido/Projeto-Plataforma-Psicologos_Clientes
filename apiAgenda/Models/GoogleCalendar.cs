using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiAgenda.Models
{
    public class GoogleCalendar
    {
        public string Summary { get; set; } //titulo
        public string Description { get; set; } //decriçao
        public string Location { get; set; } //localizaçao
        public DateTime Start { get; set; } //tempo inicio
        public DateTime End { get; set; } //tempo final
        public List<EventAttendee> Attendees { get; set; }  //convidado da agenda
        public ConferenceData ConferenceData { get; set; } //cria uma url para o GOOGLE MEET
    }


    public class GoogleQuickCalendar
    {
        public string Summary { get; set; } = string.Empty; //titulo do evento
    }

}