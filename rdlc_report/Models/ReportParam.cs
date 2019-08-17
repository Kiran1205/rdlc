using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace rdlc_report.Models
{
    public class ReportParam
    {

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Select Employee")]
        public int EmpListID { get; set; }

        [Display(Name = "Select Employee")]
        public string EmpName{ get; set; }

        [Display(Name = "EmployeesList")]
        public IList<Employee> EmployeesList { get; set; }


       
    }
}