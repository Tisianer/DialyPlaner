using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DailyPlanner.Models
{
    public class PageInfo
    {
        public PageInfo ( int PageNumber, int PageSize, int TotalItems)
        {
            this.PageNumber = PageNumber;
            this.PageSize = PageSize;
            this.TotalItems = TotalItems;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } 
        public int TotalItems { get; set; } 
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
}