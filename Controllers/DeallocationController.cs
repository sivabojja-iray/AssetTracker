using I_RAY_ASSET_TRACKER_MVC.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Configuration;
using System.Drawing;
using System.Net.Mail;
using System.Web.UI.WebControls;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class DeallocationController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        string r;
        string m;
        // GET: Deallocation
        public ActionResult Index(int? page)
        {
            if (Session["username"] != null)
            {
                //int pageSize = 5;
                //int pageIndex = 1;
                //pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

                SqlConnection con = new SqlConnection(constr);         
                SqlCommand comnd = new SqlCommand("SELECT AssignID,EmpID,EmployeeName,Team,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,InvoiceNo,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + Session["username"] + "'order by AssignID");
                comnd.Connection = con;
                SqlDataAdapter adp = new SqlDataAdapter(comnd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                List<Table1Model> dataModels = new List<Table1Model>();
                foreach (DataRow row in dt.Rows)
                {
                    Table1Model userList = new Table1Model();
                    {
                        userList.AssignID = row["AssignID"].ToString();
                        userList.EmpID = row["EmpID"].ToString();
                        userList.EmployeeName = row["EmployeeName"].ToString();
                        userList.Team = row["Team"].ToString();
                        userList.Assetbelongsto = row["Assetbelongsto"].ToString();
                        userList.AssetType = row["AssetType"].ToString();
                        userList.HWSWName = row["HWSWName"].ToString();
                        userList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                        userList.InvoiceNo = row["InvoiceNo"].ToString();
                        userList.AssignDate = row["AssignDate"].ToString();
                        userList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                    }
                    dataModels.Add(userList);
                }
                var viewModal = new UserModel
                {
                    Table1Data = dataModels                 
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

                con.Close();
                return View(viewModal);
            }
            else
            {
                return Redirect("/~UserLogin/Login");
            }
        }
        public ActionResult DeallocationRequest(string EmployeeID,string EmployeeName,string Team,string Assetbelongsto, string AssetType, string HWSWName, string SerialNumberVersionNumber, string InvoiceNo, string AssignDate, string ReturnDate)
        {
            SqlConnection con = new SqlConnection(constr);
            string s1 = "insert into DeallocateRequest (EmpID,EmployeeName,Team,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,InvoiceNo,AssignDate,ExpectedReturnDate) values('" + EmployeeID + "','" + EmployeeName + "','" + Team + "','" + Assetbelongsto + "','" + AssetType + "','" + HWSWName + "','" + SerialNumberVersionNumber + "','" + InvoiceNo + "','" + AssignDate + "','" + ReturnDate + "')";
            SqlCommand cmd4 = new SqlCommand(s1, con);
            con.Open();
            cmd4.ExecuteNonQuery();
            con.Close();
            SqlCommand sqlCommand1 = new SqlCommand("SELECT Mail  FROM EmployeeList WHERE EmpID='" + Session["username"] + "'", con);
            SqlCommand sqlCommand = new SqlCommand("SELECT Mail  FROM EmployeeList WHERE Team='" + Team + "' AND Role='Manager'", con);
            con.Open();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            if (dr.Read())
            {
                r = dr["Mail"].ToString();
                con.Close();
                con.Open();
                SqlDataReader dr2 = sqlCommand1.ExecuteReader();
                if (dr2.Read())
                {
                    m = dr2["Mail"].ToString();
                    con.Close();
                }
            }
            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress(m);
            //mailMessage.To.Add(new MailAddress(r));
            //mailMessage.Subject = "Asset Deallocation Request";
            //mailMessage.IsBodyHtml = true;
            //mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th> EmpID </th>" + "<th> EmployeeName </th>" + "<th> Team </th>" + "<th> Assetbelongsto </th>" + "<th> AssetType </th>" + "<th> HWSWName </th>" + "<th> SerialNumberVersionNumber </th>" +
            //    "<th> InvoiceNo </th>" + "<th> AssignDate </th>" + "<th> ExpectedReturnDate </th>" + "</tr>" + "<tr>" + "<td>" + Session["username"] + "</td>" + "<td>" + EmployeeName + "</td>" + "<td>" + Team + "</td>" + "<td>" + Assetbelongsto + "</td>" + "<td>" + AssetType + "</td>" + 
            //    "<td>" + HWSWName + "</td>" + "<td>" + SerialNumberVersionNumber + "</td>" + "<td>" + InvoiceNo + "</td>" + "<td>" + AssignDate + "</td>" + "<td>" + ReturnDate + "</tr>" + "</table>";
            //SmtpClient smtpClient = new SmtpClient();
            //smtpClient.Send(mailMessage);
            con.Close();
            return RedirectToAction("Index");
        }
    }
}