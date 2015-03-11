using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AutofacOwinAuth.WebAPI.Models
{
    public class DateModel
    {
        [Display(Name="TestDate")]
        [NeedSetDate(ErrorMessage="{0} must set a date.")]
        public DateTime Date { get; set; }
    }
    //http://stackoverflow.com/questions/20773320/data-annotations-range-validator-for-date
    public class NeedSetDateAttribute : ValidationAttribute
    {
        private readonly DateTime _minValue = DateTime.MinValue;

        public override bool IsValid(object value)
        {
            var val = (DateTime)value;
            return val > _minValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessage, name);
        }
    }
}