using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using OfficeOpenXml;
using System.IO;
using iTextSharp.text;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AttendanceListController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: AttendanceList
        public ActionResult Index()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd9 = new SqlCommand("select top 4 * from Assetrequest order by RequestID desc");
            cmd9.Connection = sqlConnection;
            SqlDataAdapter data = new SqlDataAdapter(cmd9);
            DataTable dataTable1 = new DataTable();
            data.Fill(dataTable1);
            List<adminNotification> list1 = new List<adminNotification>();
            foreach (DataRow row in dataTable1.Rows)
            {
                adminNotification adminNotification = new adminNotification
                {
                    EmpName = row["EmployeeName"].ToString(),
                    EmpTeam = row["EmpTeam"].ToString(),
                    RequestDate = row["RequestDate"].ToString()
                };
                list1.Add(adminNotification);
            }
            ViewBag.MyList = list1;
            return View();
        }
        public ActionResult UploadAttendanceExcelFile(HttpPostedFileBase fileupload)
        {
            if (fileupload != null && fileupload.ContentLength > 0)
            {
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.Commercial;
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package1 = new ExcelPackage(new FileInfo("MonthlySummaryDetails.xlsx")))
                {
                    using (var package = new ExcelPackage(fileupload.InputStream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row < rowCount + 1; row++)
                        {
                            var EmployeeCode = worksheet.Cells[row, 1].Text;
                            var PresentDays = worksheet.Cells[row, 2].Text;
                            var AbsentDays = worksheet.Cells[row, 3].Text;
                            var NormalWorkingHours = worksheet.Cells[row, 4].Text;
                            var OTHours = worksheet.Cells[row, 5].Text;
                            var OTDays = worksheet.Cells[row, 6].Text;
                            var CL = worksheet.Cells[row, 7].Text;
                            var PL = worksheet.Cells[row, 8].Text;
                            var SL = worksheet.Cells[row, 9].Text;
                            var TotalLeave = worksheet.Cells[row, 10].Text;
                            var LateComingDays = worksheet.Cells[row, 11].Text;
                            var LateComingHours = worksheet.Cells[row, 12].Text;
                            var EarlyGoingDays = worksheet.Cells[row, 13].Text;
                            var EarlyGoingHours = worksheet.Cells[row, 14].Text;
                            var WeeklyOff = worksheet.Cells[row, 15].Text;
                            var WeeklyOffPresent = worksheet.Cells[row, 16].Text;
                            var Holiday = worksheet.Cells[row, 17].Text;
                            var HolidayPresent = worksheet.Cells[row, 18].Text;

                            using (var connection = new SqlConnection(constr))
                            {
                                connection.Open();
                                using (var command = new SqlCommand("INSERT INTO EmployeeAttedanceList VALUES (@EmployeeCode,@PresentDays,@AbsentDays,@NormalWorkingHours,@OTHours,@OTDays,@CL,@PL,@SL,@TotalLeave,@LateComingDays,@LateComingHours,@EarlyGoingDays,@EarlyGoingHours,@WeeklyOff,@WeeklyOffPresent,@Holiday,@HolidayPresent)", connection))
                                {
                                    command.Parameters.AddWithValue("@EmployeeCode", EmployeeCode);
                                    command.Parameters.AddWithValue("@PresentDays", PresentDays);
                                    command.Parameters.AddWithValue("@AbsentDays", AbsentDays);
                                    command.Parameters.AddWithValue("@NormalWorkingHours", NormalWorkingHours);
                                    command.Parameters.AddWithValue("@OTHours", OTHours);
                                    command.Parameters.AddWithValue("@OTDays", OTDays);
                                    command.Parameters.AddWithValue("@CL", CL);
                                    command.Parameters.AddWithValue("@PL", PL);
                                    command.Parameters.AddWithValue("@SL", SL);
                                    command.Parameters.AddWithValue("@TotalLeave", TotalLeave);
                                    command.Parameters.AddWithValue("@LateComingDays", LateComingDays);
                                    command.Parameters.AddWithValue("@LateComingHours", LateComingHours);
                                    command.Parameters.AddWithValue("@EarlyGoingDays", EarlyGoingDays);
                                    command.Parameters.AddWithValue("@EarlyGoingHours", EarlyGoingHours);
                                    command.Parameters.AddWithValue("@WeeklyOff", WeeklyOff);
                                    command.Parameters.AddWithValue("@WeeklyOffPresent", WeeklyOffPresent);
                                    command.Parameters.AddWithValue("@Holiday", Holiday);
                                    command.Parameters.AddWithValue("@HolidayPresent", HolidayPresent);
                                    command.ExecuteNonQuery();
                                }
                                connection.Close();
                            }
                        }
                    }
                }
            }
            List<DisplayEmployeeAttendenceList> lists = EmployeeAttendenceList("select * from EmployeeAttedanceList");
            var viewModel = new AttendanceListModel
            {
                displayEmployeeAttendenceLists = lists
            };
            return PartialView("AttendanceListPartialView", viewModel);
        }
        private List<DisplayEmployeeAttendenceList> EmployeeAttendenceList(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<DisplayEmployeeAttendenceList> dataModels = new List<DisplayEmployeeAttendenceList>();
            foreach (DataRow row in dt.Rows)
            {
                DisplayEmployeeAttendenceList displayEmployeeAttendenceList = new DisplayEmployeeAttendenceList();
                {
                    displayEmployeeAttendenceList.EmployeeCode = row["EmployeeCode"].ToString();
                    displayEmployeeAttendenceList.PresentDays = row["PresentDays"].ToString();
                    displayEmployeeAttendenceList.AbsentDays = row["AbsentDays"].ToString();
                    displayEmployeeAttendenceList.NormalWorkingHours = row["NormalWorkingHours"].ToString();
                    displayEmployeeAttendenceList.OTHours = row["OTHours"].ToString();
                    displayEmployeeAttendenceList.OTDays = row["OTDays"].ToString();
                    displayEmployeeAttendenceList.CL = row["CL"].ToString();
                    displayEmployeeAttendenceList.PL = row["PL"].ToString();
                    displayEmployeeAttendenceList.SL = row["SL"].ToString();
                    displayEmployeeAttendenceList.TotalLeave = row["TotalLeave"].ToString();
                    displayEmployeeAttendenceList.LateComingDays = row["LateComingDays"].ToString();
                    displayEmployeeAttendenceList.LateComingHours = row["LateComingHours"].ToString();
                    displayEmployeeAttendenceList.EarlyGoingDays = row["EarlyGoingDays"].ToString();
                    displayEmployeeAttendenceList.EarlyGoingHours = row["EarlyGoingHours"].ToString();
                    displayEmployeeAttendenceList.WeeklyOff = row["WeeklyOff"].ToString();
                    displayEmployeeAttendenceList.WeeklyOffPresent = row["WeeklyOffPresent"].ToString();
                    displayEmployeeAttendenceList.Holiday = row["Holiday"].ToString();
                    displayEmployeeAttendenceList.HolidayPresent = row["HolidayPresent"].ToString();
                }
                dataModels.Add(displayEmployeeAttendenceList);
            }
            return dataModels;
        }
    }
}