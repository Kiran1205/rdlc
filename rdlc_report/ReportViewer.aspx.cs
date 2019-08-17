using Microsoft.Reporting.WebForms;
using rdlc_report.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace rdlc_report
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        EmployeeModelService objEmpModelService = new EmployeeModelService();
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!IsPostBack)
        //        {
        //            DateTime Sdt = Convert.ToDateTime(Request.QueryString["startdate"].ToString());
        //            DateTime Edt = Convert.ToDateTime(Request.QueryString["enddate"].ToString());
        //            int useridd = Convert.ToInt32(Request.QueryString["userid"].ToString());
        //            ReportViewer1.Reset();
        //            ReportViewer1.LocalReport.ReportPath = "Report/empReport.rdlc";
        //            ReportViewer1.LocalReport.SetParameters(new ReportParameter("EmpID", useridd.ToString()));
        //            ReportViewer1.LocalReport.DataSources.Clear();
        //            ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objEmpModelService.GetEmployeeAttendanceInfo(useridd, Sdt, Edt)));
        //            ReportViewer1.DataBind();

        //            this.ReportViewer1.LocalReport.Refresh();
        //        }
        //        ReportViewer1.Drillthrough += new DrillthroughEventHandler(ReportViewer1_Drillthrough); ;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DateTime Sdt = Convert.ToDateTime(Request.QueryString["startdate"].ToString());
                    DateTime Edt = Convert.ToDateTime(Request.QueryString["enddate"].ToString());
                    int userid = Convert.ToInt32(Request.QueryString["userid"].ToString());
                    ReportViewer1.Reset();
                    ReportViewer1.ShowPrintButton = false;                   
                    ReportViewer1.LocalReport.ReportPath = "Report/mainreport.rdlc";
                    ReportViewer1.LocalReport.DataSources.Clear();                    
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("startdate", Sdt.ToString()));
                    ReportViewer1.LocalReport.SetParameters(new ReportParameter("enddate", Edt.ToString()));
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", objEmpModelService.GetEmployeeInfo(userid)));                   
                    ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                    ReportViewer1.DataBind();                   
                    this.ReportViewer1.LocalReport.Refresh();
                }
                ReportViewer1.Drillthrough += new DrillthroughEventHandler(ReportViewer1_Drillthrough);
            }
            catch (Exception ex)
            {
            }
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            var ID = Convert.ToInt32(e.Parameters[0].Values[0]);
            var StartDate = Convert.ToDateTime(e.Parameters[1].Values[0]);
            var EndDate = Convert.ToDateTime(e.Parameters[2].Values[0]);            
            e.DataSources.Add(new ReportDataSource("DataSet1", objEmpModelService.GetEmployeeAttendanceInfo(ID, StartDate,EndDate)));
         

        }

        private void ReportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            int userid = 0;
            LocalReport report = (LocalReport)e.Report;
            foreach (ReportParameter rp in ((LocalReport)e.Report).OriginalParametersToDrillthrough)
            {
                userid = Convert.ToInt32(rp.Values[0]);
            }
                //Binding the DataTable to the Child report dataset.  
                //The name DataSet1 which can be located from,   
                //Go to Design view of Child.rdlc, Click View menu -> Report Data  
                //You'll see this name under DataSet2.  
                report.DataSources.Add(new ReportDataSource("DataSet1", objEmpModelService.GetEmployeeInfo(userid)));
        }

      


    }
}