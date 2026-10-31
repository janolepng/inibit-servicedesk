using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InibitServiceDesk.Models
{
    [Table("Ticket")]
    public class Ticket
    {
        public int Id { get; set; }
        public int AnlageId { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public byte Prioritaet { get; set; }   // 1 = Kritisch ... 4 = Niedrig
        public byte Status { get; set; }       // 1 = Neu ... 4 = Erledigt
        public string ZugewiesenAn { get; set; }
        public DateTime? FaelligAm { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime? ErledigtAm { get; set; }

        public virtual Anlage Anlage { get; set; }
        public virtual ICollection<TicketKommentar> Kommentare { get; set; }
    }
}