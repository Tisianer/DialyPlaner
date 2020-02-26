using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyPlanner.Models
{
    public class IndexViewModel
    {
        public IEnumerable<Diary> Diarys { get; set; }
        public DiarysListFilter DiarysListFilter { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}