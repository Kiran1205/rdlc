using Microsoft.Reporting.WebForms;
using rdlc_report.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace rdlc_report.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Report()
        {
            ViewBag.Message = "";
            ReportParam objmodel = new ReportParam();
            objmodel.StartDate = DateTime.Now.AddMonths(-1).Date;
            objmodel.EndDate = DateTime.Now.Date;
            using (rdlcdbEntities obj = new rdlcdbEntities())
            {
                objmodel.EmployeesList = obj.Employees.ToList();
                objmodel.EmpName = "kiran";
                objmodel.EmployeesList.Add(new Employee() { Id = -1, Name = "All Employee" });
            }
           
            return View(objmodel);
            
        }
        public void GenerateReport(ReportParam rp)
        {
          
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Report"), "mainreport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;               
            }

            EmployeeModelService objEmpModelService = new EmployeeModelService();
            var cm = objEmpModelService.GetEmployeeInfo(rp.EmpListID);
            var data = cm.FirstOrDefault(x => x.Id == rp.EmpListID);
            var employeename = data == null ? "All" : data.Name;
            string filename = employeename+"_" + rp.StartDate.ToString("yyyyMMdd") + "_" + rp.EndDate.ToString("yyyyMMdd") + ".zip";
            using (var compressedFileStream = new MemoryStream())
            {
                //Create an archive and store the stream in memory.
                using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                {
                    foreach (var user in cm.Select(x => new { id = x.Id, name = x.Name }).Distinct())
                    {
                        
                        //Create a zip entry for each attachment
                        var zipEntry = zipArchive.CreateEntry(user.name + ".pdf");
                        var dataset = cm.Where(x => x.Id == user.id).ToList();
                        ReportDataSource rd = new ReportDataSource("DataSet1", dataset);
                        lr.SetParameters(new ReportParameter("startdate", rp.StartDate.ToString()));
                        lr.SetParameters(new ReportParameter("enddate", rp.EndDate.ToString()));
                        lr.DataSources.Clear();
                        lr.DataSources.Add(rd);
                        lr.SubreportProcessing += new SubreportProcessingEventHandler(Lr_SubreportProcessing); ;
                        string reportType = "PDF";
                        string mimeType;
                        string encoding;
                        string fileNameExtension;

                        Warning[] warning;
                        string[] streams;
                        byte[] renderedBytes;

                        renderedBytes = lr.Render(
                            reportType,
                            null,
                            out mimeType,
                            out encoding,
                            out fileNameExtension,
                            out streams,
                            out warning);

                        //Get the stream of the attachment
                        using (var originalFileStream = new MemoryStream(renderedBytes))
                        {
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                //Copy the attachment stream to the zip entry stream
                                originalFileStream.CopyTo(zipEntryStream);
                            }
                        }
                    }
                }
                sendOutZIP(compressedFileStream.ToArray(), filename);
            }

        }

        private void Lr_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            EmployeeModelService objEmpModelService = new EmployeeModelService();
            var ID = Convert.ToInt32(e.Parameters[0].Values[0]);
            var StartDate = Convert.ToDateTime(e.Parameters[1].Values[0]);
            var EndDate = Convert.ToDateTime(e.Parameters[2].Values[0]);
            e.DataSources.Add(new ReportDataSource("DataSet1", objEmpModelService.GetEmployeeAttendanceInfo(ID, StartDate, EndDate)));
        }

        private void sendOutZIP(byte[] zippedFiles, string filename)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/x-compressed";
            Response.Charset = string.Empty;
            Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.BinaryWrite(zippedFiles);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();
        }


    }
}