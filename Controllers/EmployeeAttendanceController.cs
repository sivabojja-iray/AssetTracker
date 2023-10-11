using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class EmployeeAttendanceController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: EmployeeAttendance
        public ActionResult Index()
        {
            List<DisplayEmployeeAttendenceList> lists = EmployeeAttendenceList("select * from EmployeeAttedanceList where EmployeeCode='" + Session["username"] + "'");
            var viewModel = new AttendanceListModel
            {
                displayEmployeeAttendenceLists = lists
            };
            return View(viewModel);
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