using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using PagedList;
using System.Web.UI;
using Org.BouncyCastle.Utilities.Collections;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class EmployeeListController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: EmployeeList
        public ActionResult Index()
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comnd = new SqlCommand("SELECT * FROM EmployeeList ORDER BY EmpID");
            comnd.Connection = con;
            SqlDataAdapter adp = new SqlDataAdapter(comnd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<PagedListEmployee> dataModels = new List<PagedListEmployee>();
            foreach (DataRow row in dt.Rows)
            {
                PagedListEmployee userList = new PagedListEmployee();
                {
                    userList.Sno = row["Sno"].ToString();
                    userList.EmpID = row["EmpID"].ToString();
                    userList.FirstName = row["FirstName"].ToString();
                    userList.MiddleName = row["MiddleName"].ToString();
                    userList.LastName = row["LastName"].ToString();
                    userList.EmployeeName = row["EmployeeName"].ToString();
                    userList.Team = row["Team"].ToString();
                    userList.Role = row["Role"].ToString();
                    userList.Mail = row["Mail"].ToString();
                    userList.ContactNumber = row["ContactNumber"].ToString();
                }
                dataModels.Add(userList);
            }
            var viewModal = new EmployeeList
            {
                PagedListEmployee = dataModels
            };

            SqlCommand cmd9 = new SqlCommand("select top 4 * from Assetrequest order by RequestID desc");
            cmd9.Connection = con;
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

            return View(viewModal);
        }
        public ActionResult EmployeeUpdate(int Id,PagedListEmployee pagedListEmployee)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select Sno,EmpID,FirstName,MiddleName,LastName,EmployeeName,Team,Role,Mail,ContactNumber from EmployeeList where EmpID=" + Id, sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<PagedListEmployee> dataModels = new List<PagedListEmployee>();
            foreach (DataRow row in dt.Rows)
            {
                pagedListEmployee.Sno = row["Sno"].ToString();
                pagedListEmployee.EmpID = row["EmpID"].ToString();
                pagedListEmployee.FirstName = row["FirstName"].ToString();
                pagedListEmployee.MiddleName = row["MiddleName"].ToString();
                pagedListEmployee.LastName = row["LastName"].ToString();
                pagedListEmployee.EmployeeName = row["EmployeeName"].ToString();
                pagedListEmployee.Team = row["Team"].ToString();
                pagedListEmployee.Role = row["Role"].ToString();
                pagedListEmployee.Mail = row["Mail"].ToString();
                pagedListEmployee.ContactNumber = row["ContactNumber"].ToString();
            }
            dataModels.Add(pagedListEmployee);
            return PartialView("EmployeeUpdate", pagedListEmployee);
        }
        public ActionResult SaveEmployeeUpdate(string EmpID, string FirstName, string MiddleName, string LastName, string EmployeeName, string Team, string Role, string Mail, string ContactNumber)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("UPDATE EmployeeList SET EmpID = @EmpID, FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, EmployeeName = @EmployeeName, Team = @Team, Role = @Role, Mail = @Mail, ContactNumber = @ContactNumber WHERE EmpID = '" + EmpID + "'", sqlConnection);
            cmd.Parameters.AddWithValue("@EmpID", EmpID);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", MiddleName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@EmployeeName", EmployeeName);
            cmd.Parameters.AddWithValue("@Team", Team);
            cmd.Parameters.AddWithValue("@Role", Role);
            cmd.Parameters.AddWithValue("@Mail", Mail);
            cmd.Parameters.AddWithValue("@ContactNumber", ContactNumber);
            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("Index", "EmployeeList");
        }
    }
}