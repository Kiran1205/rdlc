using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rdlc_report.Models
{
    public class EmployeeModelService
    {

        public List<vemp> GetEmployeeAttendanceInfo(int userid,DateTime startdate,DateTime endate)
        {
          
            using (rdlcdbEntities dc = new rdlcdbEntities())
            {
                var data = dc.vemps.Where(x => x.Logintime >= startdate && x.logouttime <= endate);
                if (userid != -1)
                {
                    data = data.Where(x => x.EmpID == userid);
                }
                return data.ToList();
            }
            
        }

        public List<Employee> GetEmployeeInfo(int userid)
        {
            List<Employee> cm = new List<Employee>();
            using (rdlcdbEntities dc = new rdlcdbEntities())
            {
                
                cm = dc.Employees.Where(x => x.Id == userid || userid == -1).ToList();
            }
            return cm;
        }
    }
}