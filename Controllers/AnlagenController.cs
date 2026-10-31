using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using InibitServiceDesk.Models;

namespace InibitServiceDesk.Controllers
{
    public class AnlagenController : Controller
    {
        private readonly AppDbContext db = new AppDbContext();

        public ActionResult Index()
        {
            var anlagen = db.Anlagen
                            .Include(a => a.Kunde)       // Kunde gleich mitladen
                            .OrderBy(a => a.Bezeichnung)
                            .ToList();
            return View(anlagen);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();   // DB-Verbindung freigeben
            base.Dispose(disposing);
        }
    }
}