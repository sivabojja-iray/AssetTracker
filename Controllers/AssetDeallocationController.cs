﻿using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using System.Web.Mail;
using System.Web.UI.WebControls;
using System.Net.Mail;
using MailMessage = System.Net.Mail.MailMessage;
using MailPriority = System.Net.Mail.MailPriority;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetDeallocationController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        string EmpMail;
        // GET: AssetDeallocation
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

        public ActionResult SearchEmployeeID(string EmployeeId)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comnd = new SqlCommand("select EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,Assetbelongsto,InvoiceNo from AssignAsset where EmpID='" + EmployeeId + "'", con);
            SqlDataAdapter adp = new SqlDataAdapter(comnd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<AssetDeallocationModel> dataModels = new List<AssetDeallocationModel>();
            foreach (DataRow row in dt.Rows)
            {
                AssetDeallocationModel assetDeallocationList = new AssetDeallocationModel();
                {
                    assetDeallocationList.EmployeeID = row["EmpID"].ToString();
                    assetDeallocationList.EmployeeName = row["EmployeeName"].ToString();
                    assetDeallocationList.Team = row["Team"].ToString();
                    assetDeallocationList.AssetType = row["AssetType"].ToString();
                    assetDeallocationList.HWSWName = row["HWSWName"].ToString();
                    assetDeallocationList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    assetDeallocationList.AssignDate = row["AssignDate"].ToString();
                    assetDeallocationList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    assetDeallocationList.InvoiceNo = row["InvoiceNo"].ToString();
                }
                dataModels.Add(assetDeallocationList);
            }
            return PartialView("_SearchEmployeeIDpartialView", dataModels);
        }
        public ActionResult AssetDeallocateDetails(string EmployeeID,string EmployeeName,string Team,string AssetType,string HWSWName,string SerialNumberVersionNumber,string AssignDate,string Assetbelongsto,string InvoiceNo)
        {           
            SqlConnection con = new SqlConnection(constr);
            string s1 = "insert into AssignAsset1 (EmpID,EmployeeName,Team,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,InvoiceNo) values('" + EmployeeID + "','" 
                + EmployeeName + "','" + Team + "','" + Assetbelongsto + "','" + AssetType + "','" + HWSWName + "','" 
                + SerialNumberVersionNumber + "','" + AssignDate + "','" + InvoiceNo + "')";
            SqlCommand cmd4 = new SqlCommand(s1, con);
            con.Open();
            cmd4.ExecuteNonQuery();
            con.Close();
            SqlCommand updatecmd = new SqlCommand("UPDATE AssignAsset1 SET ActualReturnDate = '" + DateTime.Now.ToString() + "' WHERE SerialNumberVersionNumber ='" + SerialNumberVersionNumber + "'", con);
            con.Open();
            updatecmd.ExecuteNonQuery();
            con.Close();
            SqlCommand cmd5 = new SqlCommand("delete from AssignAsset where SerialNumberVersionNumber = '" + SerialNumberVersionNumber + "'", con);
            con.Open();
            cmd5.ExecuteNonQuery();
            con.Close();
            SqlCommand cmdt = new SqlCommand("delete from tblAuditDocs2 where SerialNumberVersionNumber = '" + SerialNumberVersionNumber + "'", con);
            con.Open();
            cmdt.ExecuteNonQuery();
            con.Close();
            SqlCommand cmd6 = new SqlCommand("select Mail from EmployeeList where EmpID='" + EmployeeID + "'", con);
            con.Open();
            SqlDataReader dr2 = cmd6.ExecuteReader();
            if (dr2.Read())
            {
                EmpMail = dr2["Mail"].ToString();
                //Session["EmpMail"] = EmpMail;
            }
            con.Close();
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
            mailMessage.To.Add(new MailAddress(EmpMail));
            mailMessage.Subject = "Asset is DeAllocated";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th bgcolor='#ffc107'> EmpID </th>" + "<th bgcolor='#ffc107'> EmployeeName </th>" + "<th bgcolor='#ffc107'> Team </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'> AssetName </th>" + "<th bgcolor='#ffc107'> AssetSerialNo </th>" + "<th bgcolor='#ffc107'> DeallocatedDate </th>" + "</tr>" + "<tr>" + "<td>"
                + EmployeeID + "</td>" + "<td>" + EmployeeName + "<td>" + Team + "<td>" + AssetType + "</td>" + "<td>" + HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + SerialNumberVersionNumber + "</td>" + "<td>" + DateTime.Now.ToString() + "</td>" + "</tr>" + "</table>";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
            mailMessage.Priority = MailPriority.High;
            ViewBag.MailMessage = "Your asset has been successfully relocated..";
            return Json(ViewBag.MailMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SearchEmployeeName(string EmployeeName)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comnd = new SqlCommand("select EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,Assetbelongsto,InvoiceNo from AssignAsset where EmployeeName='" + EmployeeName + "'", con);          
            SqlDataAdapter adp = new SqlDataAdapter(comnd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<AssetDeallocationModel> dataModels = new List<AssetDeallocationModel>();
            foreach (DataRow row in dt.Rows)
            {
                AssetDeallocationModel assetDeallocationList = new AssetDeallocationModel();
                {
                    assetDeallocationList.EmployeeID = row["EmpID"].ToString();
                    assetDeallocationList.EmployeeName = row["EmployeeName"].ToString();
                    assetDeallocationList.Team = row["Team"].ToString();
                    assetDeallocationList.AssetType = row["AssetType"].ToString();
                    assetDeallocationList.HWSWName = row["HWSWName"].ToString();
                    assetDeallocationList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    assetDeallocationList.AssignDate = row["AssignDate"].ToString();
                    assetDeallocationList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    assetDeallocationList.InvoiceNo = row["InvoiceNo"].ToString();
                }
                dataModels.Add(assetDeallocationList);
            }
            return PartialView("_SearchEmployeeIDpartialView", dataModels);
        }
    }
}