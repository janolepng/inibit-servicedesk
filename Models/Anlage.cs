using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace InibitServiceDesk.Models
{
    [Table("Anlage")]
    public class Anlage
    {
        public int Id { get; set; }
        public int KundeId { get; set; }
        public string Bezeichnung { get; set; }
        public string Seriennummer { get; set; }
        public string Standort { get; set; }
        public DateTime? Inbetriebnahme { get; set; }
        public byte Kritikalitaet { get; set; }   // TINYINT in SQL = byte in C#

        public virtual Kunde Kunde { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}