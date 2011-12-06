using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTracking.Models
{
    public class EmployeeTotalsViewModel
    {
        public Employee Employee { get; set; }
        public decimal DayHours { get; set; }
        public decimal NightHours { get; set; }
        public decimal TotalHours
        {
            get
            {
                return DayHours + NightHours;
            }
        }
    }
}