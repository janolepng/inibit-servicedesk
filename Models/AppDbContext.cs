using System.Data.Entity;

namespace InibitServiceDesk.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection") { }   // Name aus Web.config

        public DbSet<Kunde> Kunden { get; set; }
        public DbSet<Anlage> Anlagen { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketKommentar> TicketKommentare { get; set; }
    }
}