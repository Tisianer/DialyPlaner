using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DailyPlanner.ValidAttribute;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyPlanner.Models
{
    public class Diary
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Display(Name = "Тип записи")]
        public string TypeEntries { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [Display(Name = "Заголовок")]
        [StringLength(100, ErrorMessage = "Количество символов не должно превышать 100")]
        public string Topic { get; set; }
        [Required(ErrorMessage = "Обязательно для заполнения")]
        [Column(TypeName = "DateTime2")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата и время начала")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "DateTime2")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата и время окончания")]
        public DateTime? ExpirationDate { get; set; }
        [Display(Name = "Место")]
        [RequiredPlace("TypeEntries")] // кастомный attribute валидации. проверяет на null, если тип Встреча.
        [StringLength(70, ErrorMessage = "Количество символов не должно превышать 70")]
        public string Place { get; set; }
        [Display(Name = "Выполннено / Не выполннено")]
        public bool Performed { get; set; }
    }
}