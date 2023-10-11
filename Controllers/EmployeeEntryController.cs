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
        public ActionResult ChangeTempEmployeeID()
        {
            return PartialView("ChangeTempEmployeeID");
        }
        public ActionResult EmployeeIDChange(string OldEmployeeID,string NewEmployeeID)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE EmployeeList SET EmpID = @EmpIDNew WHERE EmpID = @EmpIDOld; ", sqlConnection);
            cmd.Parameters.AddWithValue("@EmpIDNew", NewEmployeeID);
            cmd.Parameters.AddWithValue("@EmpIDOld", OldEmployeeID);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            SqlCommand cmd2 = new SqlCommand("UPDATE EmployeeRegistration1 SET UserName = @UserNameNew WHERE UserName = @UserNameOld; ", sqlConnection);
            cmd2.Parameters.AddWithValue("@UserNameNew", NewEmployeeID);
            cmd2.Parameters.AddWithValue("@UserNameOld", OldEmployeeID);
            cmd2.ExecuteNonQuery();
            cmd2.Dispose();
            SqlCommand cmd3 = new SqlCommand("UPDATE AssignAsset SET EmpID = @EmpIDNew WHERE EmpID = @EmpIDOld; ", sqlConnection);
            cmd3.Parameters.AddWithValue("@EmpIDNew", NewEmployeeID);
            cmd3.Parameters.AddWithValue("@EmpIDOld", OldEmployeeID);
            cmd3.ExecuteNonQuery();
            cmd3.Dispose();
            SqlCommand cmd4 = new SqlCommand("UPDATE AssignAsset1 SET EmpID = @EmpIDNew WHERE EmpID = @EmpIDOld; ", sqlConnection);
            cmd4.Parameters.AddWithValue("@EmpIDNew", NewEmployeeID);
            cmd4.Parameters.AddWithValue("@EmpIDOld", OldEmployeeID);
            cmd4.ExecuteNonQuery();
            cmd4.Dispose();
            SqlCommand cmd5 = new SqlCommand("UPDATE tblAuditDocs2 SET EmpID = @EmpIDNew WHERE EmpID = @EmpIDOld; ", sqlConnection);
            cmd5.Parameters.AddWithValue("@EmpIDNew", NewEmployeeID);
            cmd5.Parameters.AddWithValue("@EmpIDOld", OldEmployeeID);
            cmd5.ExecuteNonQuery();
            cmd5.Dispose();
            sqlConnection.Close();
            return RedirectToAction("Index");
        }
    }
}