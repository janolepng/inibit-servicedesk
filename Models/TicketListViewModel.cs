using System;
using System.Collections.Generic;

namespace InibitServiceDesk.Models
{
    public class TicketListViewModel
    {
        public List<Ticket> Tickets { get; set; }

        // aktuelle Filter, damit die View sie wieder anzeigen kann
        public byte? Status { get; set; }
        public byte? Prioritaet { get; set; }
        public int? KundeId { get; set; }

        // Paging
        public int Seite { get; set; }
        public int ProSeite { get; set; }
        public int Gesamt { get; set; }

        public int Seiten
        {
            get { return (int)Math.Ceiling((double)Gesamt / ProSeite); }
        }
    }
}