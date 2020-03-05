using DailyPlanner.Models;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;

namespace DailyPlanner.Controllers
{

    public class CreatEditDeleteController : Controller
    {

        DiaryContext db = new DiaryContext();

        private Diary TryParseDate(Diary diary, string strStartDate, string strEndDate)
        {
            bool startDate_fr_FR = DateTime.TryParseExact(strStartDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime StartDate_fr_FR);
            bool startDate_de_DE = DateTime.TryParseExact(strStartDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime StartDate_de_DE);
            bool EndDate_fr_FR = DateTime.TryParseExact(strEndDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ExpirationDate_fr_FR);
            bool EndDate_de_DE = DateTime.TryParseExact(strEndDate, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ExpirationDate_de_DE);
            if (!(startDate_fr_FR || startDate_de_DE))
                ModelState.AddModelError("StartDate", "Не соответствует формату ввода!");
            if (diary.TypeEntries == "Памятка")
            {
                if (startDate_fr_FR || startDate_de_DE)
                    diary.StartDate = startDate_fr_FR == true ? StartDate_fr_FR : StartDate_de_DE;
                return diary;
            }
            if (!(EndDate_fr_FR || EndDate_de_DE))
                ModelState.AddModelError("ExpirationDate", "Не соответствует формату ввода!");
            if ((startDate_fr_FR || startDate_de_DE) && (EndDate_fr_FR || EndDate_de_DE))
            {
                diary.StartDate = startDate_fr_FR == true ? StartDate_fr_FR : StartDate_de_DE;
                diary.ExpirationDate = EndDate_fr_FR == true ? ExpirationDate_fr_FR : ExpirationDate_de_DE;
                if (diary.StartDate > diary.ExpirationDate)
                    ModelState.AddModelError("ExpirationDate", "Дата конечная должна быть позднее начальной даты!");
            }
            return diary;
        }

        [HttpGet]
        public ActionResult Creat(string TypeEntries)
        {
            ViewBag.TypeEntries = TypeEntries;
            return View();
        }
        [HttpPost]
        public ActionResult Creat(Diary diary, string start_date, string end_date)
        {
            diary = TryParseDate(diary, start_date, end_date);
            if (ModelState.IsValid)
            {
                db.Diarys.Add(diary);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TypeEntries = diary.TypeEntries;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id, string type_entries)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Diary diary = db.Diarys.Find(id);
            if (diary != null)
            {
                return View(diary);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult Edit(Diary diary, string StartDate, string ExpirationDate)
        {
            diary = TryParseDate(diary, StartDate, ExpirationDate);
            if (ModelState.IsValid)
            {
                db.Entry(diary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(diary);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Diary diary = db.Diarys.Find(id);
            if (diary == null)
                return HttpNotFound();
            return View(diary);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Diary diary = db.Diarys.Find(id);
            if (diary == null)
                return HttpNotFound();
            db.Diarys.Remove(diary);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}