using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DailyPlanner.Models;
using System.Data.Entity;
using System.Globalization;

namespace DailyPlanner.Controllers
{
    public class HomeController : Controller
    {
        DiaryContext db = new DiaryContext();
        IndexViewModel viewModel = new IndexViewModel();
        List<string> TypesEntries = new List<string>() { null, "Встреча", "Дело", "Памятка" };
        List<string> ModesDisplay = new List<string>() { "Все время", "День", "Неделя", "Месяц" };

        [HttpGet]
        public ActionResult Index(string search_string, string start_date, string type_entries, string mode_display , int page = 1)
        {
            //Фильтрации.
            int pageSize = 2; // количество объектов на страницу.
            bool Date_fr_FR = DateTime.TryParseExact(start_date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt_fr_FR);
            bool Date_de_DE = DateTime.TryParseExact(start_date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt_de_DE);
            DateTime Date = Date_fr_FR == true ? dt_fr_FR : dt_de_DE;
            IQueryable<Diary> diarys = db.Diarys;
            if (!String.IsNullOrEmpty(type_entries))
                diarys = diarys.Where(type => type.TypeEntries == type_entries); // Выборка по заданному типу.
            if (!String.IsNullOrEmpty(mode_display))
            {
                if (mode_display == "День")
                    diarys = diarys.Where(s => DbFunctions.TruncateTime(s.StartDate) == DateTime.Today.Date); // Выборка по текущему дню.
                else if (mode_display == "Неделя")
                {
                    var StartWeek = DateTime.Today.AddDays(-((double)DateTime.Today.DayOfWeek - 1));//получение даты начала недели.
                    var EndtWeek = DateTime.Today.AddDays(+((double)DateTime.Today.DayOfWeek - 1));//получение даты конца недели.
                    diarys = diarys.Where(s => DbFunctions.TruncateTime(s.StartDate) >= StartWeek.Date && DbFunctions.TruncateTime(s.StartDate) <= EndtWeek.Date); // Выборка по текущей неделе.
                }
                else if (mode_display == "Месяц")
                    diarys = diarys.Where(s => s.StartDate.Month == DateTime.Today.Month && s.StartDate.Year == DateTime.Today.Year); // Выборка по текущему месяцу и году
            }
            if (!String.IsNullOrEmpty(start_date) && (Date_fr_FR || Date_de_DE))
            {
                diarys = diarys.Where(s => DbFunctions.TruncateTime(s.StartDate) == Date.Date); // Выборка по заданной дате.
            }
            else
                ModelState.AddModelError("StartDate", "Дата должна быть формата \"дд/мм/гггг\" или \"дд.мм.гггг\"!");
            if (!String.IsNullOrEmpty(search_string))
                diarys = diarys.Where(top => top.Topic.ToUpper().Contains(search_string.ToUpper())); // Поиск по заголовку.
            // Пагинация.
            PageInfo pageInfo = new PageInfo(page, pageSize, diarys.Count());
            //Собирание модели.
            viewModel.DiarysListFilter = new DiarysListFilter ( TypesEntries, ModesDisplay, search_string,
                ((Date_fr_FR || Date_de_DE)  ? Date.ToString("dd.MM.yyyy") : null), //для отображения даты без времени при пагинации.
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
            return RedirectToAction("Creat", "CreatEditDelete", new { TypeEntries });
        }

    }
}