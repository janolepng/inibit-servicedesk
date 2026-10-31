using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using InibitServiceDesk.Models;

namespace InibitServiceDesk.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        // GET /Tickets?status=1&prioritaet=2&kundeId=3&seite=2
        public ActionResult Index(byte? status, byte? prioritaet, int? kundeId,
                                  int seite = 1, int proSeite = 10)
        {
            // Eingaben aus der URL nie blind vertrauen
            if (seite < 1) seite = 1;
            if (proSeite < 1 || proSeite > 100) proSeite = 10;

            IQueryable<Ticket> query = db.Tickets.Include(t => t.Anlage.Kunde);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);
            if (prioritaet.HasValue)
                query = query.Where(t => t.Prioritaet == prioritaet.Value);
            if (kundeId.HasValue)
                query = query.Where(t => t.Anlage.KundeId == kundeId.Value);

            var gesamt = query.Count();   // Anzahl NACH dem Filtern

            var tickets = query
                .OrderBy(t => t.Prioritaet)              // 1 = kritisch zuerst
                .ThenByDescending(t => t.ErstelltAm)
                .Skip((seite - 1) * proSeite)
                .Take(proSeite)
                .ToList();                               // erst hier geht die Abfrage an die DB

            var vm = new TicketListViewModel
            {
                Tickets = tickets,
                Status = status,
                Prioritaet = prioritaet,
                KundeId = kundeId,
                Seite = seite,
                ProSeite = proSeite,
                Gesamt = gesamt
            };

            ViewBag.StatusListe = StatusListe(status);
            ViewBag.PrioritaetListe = PrioritaetListe(prioritaet);
            ViewBag.KundenListe = new SelectList(
                db.Kunden.OrderBy(k => k.Name).Select(k => new { k.Id, k.Name }).ToList(),
                "Id", "Name", kundeId);

            return View(vm);
        }

        // ---------- Hilfsmethoden für Dropdowns ----------

        private static SelectList StatusListe(byte? ausgewaehlt)
        {
            return new SelectList(new[]
            {
                new { Wert = StatusWerte.Neu,           Text = "Neu" },
                new { Wert = StatusWerte.InBearbeitung, Text = "In Bearbeitung" },
                new { Wert = StatusWerte.Wartet,        Text = "Wartet auf Kunde" },
                new { Wert = StatusWerte.Erledigt,      Text = "Erledigt" }
            }, "Wert", "Text", ausgewaehlt);
        }

        private static SelectList PrioritaetListe(byte? ausgewaehlt)
        {
            return new SelectList(new[]
            {
                new { Wert = PrioritaetsWerte.Kritisch, Text = "Kritisch" },
                new { Wert = PrioritaetsWerte.Hoch,     Text = "Hoch" },
                new { Wert = PrioritaetsWerte.Mittel,   Text = "Mittel" },
                new { Wert = PrioritaetsWerte.Niedrig,  Text = "Niedrig" }
            }, "Wert", "Text", ausgewaehlt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}