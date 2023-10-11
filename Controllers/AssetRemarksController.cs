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
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("sp_MyAssetsRemarks", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", Session["username"]);
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
            string mail;
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select Mail from EmployeeList where EmpID='" + Session["username"] + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dataReader = cmd.ExecuteReader();
            if(dataReader.Read())
            {
                mail = dataReader["Mail"].ToString();
                FormsAuthentication.SetAuthCookie(mail, true);
                Session["Mail"] = mail;
            }

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");

            mailMessage.To.Add(new MailAddress("assettracker@i-raysolutions.com"));
            mailMessage.CC.Add(new MailAddress(Session["Mail"].ToString()));
            mailMessage.Subject = "Asset Remarks By Employee";
            mailMessage.IsBodyHtml = true;         
            mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th> EmpID </th>" 
                + "<th> EmployeeName </th>" + "<th> EmployeeTeam </th>" + "<th> Asset belongs to? </th>" + "<th> AssetType </th>" + "<th>HWSWName </th>" 
                + "<th> Asset Serialnumber </th>" + "<th> Asset Remarks </th>" + "</tr>" + "<tr>" + "<td>" + Session["username"] + "</td>" + "<td>" 
                + EmployeeName + "</td>" + "<td>" + Team + "</td>" + "<td>" + Assetbelongsto + "</td>" + "<td>" + AssetType + "</td>" + "<td>" 
                + HWSWName + "</td>" + "<td>" + SerialNumberVersionNumber + "</td>" + "<td>" + AssetRemarks + "</td>" + "</tr>" + "</table>";

            mailMessage.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mailMessage);

            return View();
        }
    }
}