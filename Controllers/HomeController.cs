using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DailyPlanner.Models;
using System.Data.Entity;
using System.Globalization;
using System.Threading.Tasks;

namespace DailyPlanner.Controllers
{
    public class HomeController : Controller
    {
        DiaryContext db = new DiaryContext();
        IndexViewModel viewModel = new IndexViewModel();
        List<string> TypesEntries = new List<string>() { null, "Встреча", "Дело", "Памятка" };
        List<string> ModesDisplay = new List<string>() { "Все время", "День", "Неделя", "Месяц" };

        private DateTime TryParseDate(string strDate)
        {
            bool Date_fr_FR = DateTime.TryParse(strDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt_fr_FR);
            if (!Date_fr_FR)
                ModelState.AddModelError("StartDate", "Дата и время должны быть формата \"дд/мм/гггг\" или \"дд.мм.гггг\"!");
            return dt_fr_FR;
        }

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
        public ActionResult Index(string search_string, string start_date, string type_entries, string mode_display , int page = 1)
        {
            //Фильтрации.
            int pageSize = 2; // количество объектов на страницу.
            DateTime Date = TryParseDate(start_date);
            IQueryable<Diary> diarys = db.Diarys;
            if (!String.IsNullOrEmpty(type_entries))
                diarys = diarys.Where(type => type.TypeEntries == type_entries); // Выборка по заданному типу.
            if (!String.IsNullOrEmpty(mode_display))
            {
                if (mode_display == "День")
                    diarys = diarys.Where(s => s.StartDate == DateTime.Today); // Выборка по текущему дню.
                else if (mode_display == "Неделя")
                {
                    var StartWeek = DateTime.Today.AddDays(-((double)DateTime.Today.DayOfWeek - 1));//получение даты начала недели.
                    var EndtWeek = DateTime.Today.AddDays(+((double)DateTime.Today.DayOfWeek - 1));//получение даты конца недели.
                    diarys = diarys.Where(s => s.StartDate >= StartWeek && s.StartDate <= EndtWeek); // Выборка по текущей неделе.
                }
                else if (mode_display == "Месяц")
                    diarys = diarys.Where(s => s.StartDate.Month == DateTime.Today.Month && s.StartDate.Year == DateTime.Today.Year); // Выборка по текущему месяцу и году
            }
            if (!String.IsNullOrEmpty(start_date))
            {
                diarys = diarys.Where(s => s.StartDate == Date); // Выборка по заданной дате.
            }
            if (!String.IsNullOrEmpty(search_string))
                diarys = diarys.Where(top => top.Topic.ToUpper().Contains(search_string.ToUpper())); // Поиск по заголовку.
            // Пагинация.
            PageInfo pageInfo = new PageInfo(page, pageSize, diarys.Count());
            //Собирание модели.
            viewModel.DiarysListFilter = new DiarysListFilter ( TypesEntries, ModesDisplay, search_string,
                (String.IsNullOrEmpty(start_date) == true ? null : Date.ToString("dd.MM.yyyy")), //для отображения даты без времени при пагинации.
                type_entries, mode_display );
            viewModel.PageInfo = pageInfo;
            viewModel.Diarys = diarys.ToList().Skip((page - 1) * pageSize).Take(pageSize); // Подсчет пропускаемых объектов на страницу пагинации.
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Index(string TypeEntries)
        {

            if (String.IsNullOrEmpty(TypeEntries))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Creat", "Home", new { TypeEntries });
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
                return RedirectToAction("Index");
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
        public ActionResult Edit(Diary diary, string start_date, string end_date)
        {
            diary = TryParseDate(diary, start_date, end_date);
            if (ModelState.IsValid)
            {
                db.Entry(diary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return RedirectToAction("Index");
        }

    }
}