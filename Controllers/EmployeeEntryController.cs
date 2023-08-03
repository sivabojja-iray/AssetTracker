using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class EmployeeEntryController : Controller
    {
        private object full;
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: EmployeeEntry
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult IsUsernameOrEmailExists(string username,string email)
        {
            bool isUsernameExists = false;
            bool isEmailExists = false;
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand cmdUsername = new SqlCommand("select count(*) from EmployeeList where EmpID ='" + username + "'", sqlConnection);
            int count = (int)cmdUsername.ExecuteScalar();
            isUsernameExists = count > 0;
            SqlCommand cmdEmail = new SqlCommand("select count(*) from EmployeeList where Mail ='" + email + "'", sqlConnection);
            int count1 = (int)cmdEmail.ExecuteScalar();
            isEmailExists = count1 > 0;
            sqlConnection.Close();
            return Json(new { isUsernameExists, isEmailExists },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RegisterEmployee(EmployeeEntryModel employeeEntryModel)
        {
            if(ModelState.IsValid)
            {
                SqlConnection sqlConnection = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("ProEmployeeList", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", employeeEntryModel.EmployeeID);
                cmd.Parameters.AddWithValue("@FirstName", employeeEntryModel.FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", employeeEntryModel.MiddleName);
                cmd.Parameters.AddWithValue("@LastName", employeeEntryModel.LastName);
                full = employeeEntryModel.FirstName + " " + employeeEntryModel.MiddleName + " " + employeeEntryModel.LastName;
                cmd.Parameters.AddWithValue("@EmployeeName", full);

                cmd.Parameters.AddWithValue("@Team", employeeEntryModel.Team);
                cmd.Parameters.AddWithValue("@Role", employeeEntryModel.Role);
                cmd.Parameters.AddWithValue("@Mail", employeeEntryModel.Mail);
                cmd.Parameters.AddWithValue("@ContactNumber", employeeEntryModel.ContactNumber);
                sqlConnection.Open();
                int i = cmd.ExecuteNonQuery();
                sqlConnection.Close();
                //MailMessage mail = new MailMessage("assettracker@i-raysolutions.com", employeeEntryModel.Mail, "AssetTracker Registration", "Your Employee ID : " + employeeEntryModel.EmployeeID + " is registered successfully, Please click on the URL to view :" + "https://friendly-davinci.72-167-36-205.plesk.page/EmployeeRegistration" + ".");
                //mail.Priority = MailPriority.High;
                //SmtpClient smtp = new SmtpClient();
                //smtp.Send(mail);
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}