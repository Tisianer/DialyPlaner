using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DailyPlanner.Models
{
    public class DiaryContext : DbContext
    {
        public DiaryContext() : base("DialyDd") { }

        public DbSet<Diary> Diarys { get; set; }
    }
    public class DiaryDbInitializer : DropCreateDatabaseAlways<DiaryContext>
    {
        protected override void Seed(DiaryContext db)
        {
            db.Diarys.Add(new Diary { TypeEntries = "Встреча", Topic = "Забрать аттестат", StartDate = DateTime.Parse("10.10.2020"), ExpirationDate = DateTime.Parse("11.10.2020"), Place = "Школа" });
            db.Diarys.Add(new Diary { TypeEntries = "Дело", Topic = "Встреча с другом", StartDate = DateTime.Parse("10.10.2020"), ExpirationDate = DateTime.Parse("11.10.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("10.10.2020"), Performed = true });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("09.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("10.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("11.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("12.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("13.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("14.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("15.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("16.02.2020") });
            db.Diarys.Add(new Diary { TypeEntries = "Памятка", Topic = "Поспать", StartDate = DateTime.Parse("17.02.2020") });

            base.Seed(db);
        }
    }
}