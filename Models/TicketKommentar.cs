using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InibitServiceDesk.Models
{
    [Table("TicketKommentar")]
    public class TicketKommentar
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Autor { get; set; }
        public string Text { get; set; }
        public DateTime ErstelltAm { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}