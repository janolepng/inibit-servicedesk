using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InibitServiceDesk.Models
{
    [Table("Kunde")]   // Tabelle heißt in der DB "Kunde", nicht "Kundes"
    public class Kunde
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ansprechpartner { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Ort { get; set; }
        public bool Aktiv { get; set; }

        public virtual ICollection<Anlage> Anlagen { get; set; }
    }
}