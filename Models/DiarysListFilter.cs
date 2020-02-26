using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyPlanner.Models
{
    public class DiarysListFilter
    {
        public DiarysListFilter(List<string> TypeEntries, List<string> ModeDisplay, string search_string, string start_date, string type_entries, string mode_display)
        {
            TypesEntries = new SelectList(new List<string>(TypeEntries));
            ModesDisplay = new SelectList(new List<string>(ModeDisplay));
            SelectTypeEntries = type_entries;
            SelectModeDisplay = mode_display;
            SearchString = search_string;
            StartDate = start_date;
        }
        public SelectList TypesEntries { get; private set; }
        public SelectList ModesDisplay { get; private set; }
        public string SelectTypeEntries { get; private set; }
        public string SelectModeDisplay { get; private set; }
        public string SearchString { get; private set; }
        public string StartDate { get; private set; }
    }
}