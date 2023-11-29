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
using System.Web.Security;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetRemarksController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        string mail;
        // GET: AssetRemarks
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
        public ActionResult TrackNow(string SerailNo)
        {
            string UserID = Session["username"].ToString();
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("sp_MyAssetsRemarks", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", UserID);
            cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", SerailNo);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<AssetRemarksModel> list = new List<AssetRemarksModel>();
            foreach (DataRow row in dt.Rows)
            {
                AssetRemarksModel assetRemarksModel = new AssetRemarksModel();
                {
                    assetRemarksModel.EmployeeName = row["EmployeeName"].ToString();
                    assetRemarksModel.EmpID = row["EmpID"].ToString();
                    assetRemarksModel.Team = row["Team"].ToString();
                    assetRemarksModel.Assetbelongsto = row["Asset belongs to"].ToString();
                    assetRemarksModel.AssetType = row["Asset Type"].ToString();
                    assetRemarksModel.HWSWName = row["Asset Name"].ToString();
                    assetRemarksModel.SerialNumberVersionNumber = row["SerialNumber/VersionNumber"].ToString();
                    assetRemarksModel.AssignDate = row[" Assign Date"].ToString();
                }
                list.Add(assetRemarksModel);
            }
            return PartialView("TrackNow",list);
        }
        public ActionResult SendMailReportToAM(string EmployeeName,string Team,string Assetbelongsto,string AssetType,string HWSWName,string SerialNumberVersionNumber,string AssignDate,string AssetRemarks)
        {
            string UserID = Session["username"].ToString();        
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select Mail from EmployeeList where EmpID='" + UserID + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            if(dataReader.Read())
            {
                mail = dataReader["Mail"].ToString();
                //FormsAuthentication.SetAuthCookie(mail, true);
                //Session["Mail"] = mail;
            }

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
            mailMessage.To.Add(new MailAddress("phani.n@i-raysolutions.com"));
            mailMessage.CC.Add(new MailAddress(mail));
            mailMessage.Subject = "Asset Remarks By Employee";
            mailMessage.IsBodyHtml = true;         
            mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th bgcolor='#ffc107'> EmpID </th>"
                + "<th bgcolor='#ffc107'> EmployeeName </th>" + "<th bgcolor='#ffc107'> EmployeeTeam </th>" + "<th bgcolor='#ffc107'> Asset belongs to? </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'>HWSWName </th>"
                + "<th bgcolor='#ffc107'> Asset Serialnumber </th>" + "<th bgcolor='#ffc107'> Asset Remarks </th>" + "</tr>" + "<tr>" + "<td>" + UserID + "</td>" + "<td>" 
                + EmployeeName + "</td>" + "<td>" + Team + "</td>" + "<td>" + Assetbelongsto + "</td>" + "<td>" + AssetType + "</td>" + "<td>" 
                + HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + SerialNumberVersionNumber + "</td>" + "<td>" + AssetRemarks + "</td>" + "</tr>" + "</table>";

            mailMessage.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mailMessage);
            ViewBag.MailMessage = "Email Sent Successfully...";
            return Json(ViewBag.MailMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AutoCompleteSerialNumberVersionNumberTextBox(string term)
        {
            string UserID = Session["username"].ToString();
            List<string> suggestions = GetSuggestionsFromDatabase(term, "select SerialNumberVersionNumber from AssignAsset where EmpID='" + UserID + "' and SerialNumberVersionNumber LIKE @term", "SerialNumberVersionNumber");
            return Json(suggestions, JsonRequestBehavior.AllowGet);
        }
        private List<string> GetSuggestionsFromDatabase(string term, string query, string columnName)
        {
            // Implement your database query here and return a list of suggestions
            // Example using SqlConnection and SqlCommand (replace with your own database connection method):
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@term", term + "%");
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<string> suggestions = new List<string>();
                        while (reader.Read())
                        {
                            suggestions.Add(reader[columnName].ToString());
                        }
                        return suggestions;
                    }
                }
            }
        }
    }
}