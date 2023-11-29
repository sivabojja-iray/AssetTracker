using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;
using Org.BouncyCastle.Ocsp;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class NotificationManagerController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: NotificationManager
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

            SqlCommand cmdd = new SqlCommand("select FirstName from EmployeeList where EmpID=@EmpID");
            cmdd.CommandType = CommandType.Text;
            cmdd.Connection = sqlConnection;
            sqlConnection.Open();
            cmdd.Parameters.AddWithValue("@EmpID", Session["username"]);
            SqlDataReader dr = cmdd.ExecuteReader();
            if (dr.Read())
            {
                TempData["FirstName"] = dr["FirstName"].ToString();
            }
            sqlConnection.Close();

            SqlCommand com = new SqlCommand();
            sqlConnection.Open();
            com.Connection = sqlConnection;
            com.CommandText = "sp_notification";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@Event", SqlDbType.VarChar, 50));
            com.Parameters["@Event"].Value = "Select";
            SqlDataAdapter sqlda = new SqlDataAdapter(com);
            DataTable dataTable = new DataTable();
            sqlda.Fill(dataTable);
            List<Notifation> dataModels = new List<Notifation>();
            foreach (DataRow row in dataTable.Rows)
            {
                Notifation notifation = new Notifation();
                {
                    notifation.Sno = row["Sno"].ToString();
                    notifation.Name = row["Name"].ToString();
                    notifation.Notification = row["Notification111"].ToString();
                    notifation.Date = row["Last_Update"].ToString();                
                }
                dataModels.Add(notifation);
            }
            sqlConnection.Close();
            var viewModal = new NotificationManagerModel
            {
                notifations = dataModels
            };
            return View(viewModal);
        }
        public ActionResult NotificationMessageDetails(string Message, string firstname)
        {          
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmdd = new SqlCommand("select FirstName from EmployeeList where EmpID=@EmpID");
            cmdd.CommandType = CommandType.Text;
            cmdd.Connection = sqlConnection;
            sqlConnection.Open();
            cmdd.Parameters.AddWithValue("@EmpID", Session["username"]);
            SqlDataReader dr = cmdd.ExecuteReader();
            if (dr.Read())
            {
                firstname = dr["FirstName"].ToString();
            }
            sqlConnection.Close();

            sqlConnection.Open();
            SqlCommand com = new SqlCommand();
            com.CommandText = "sp_notification";
            com.CommandType = CommandType.StoredProcedure;
            com.Connection = sqlConnection;
            com.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 300));
            com.Parameters.Add(new SqlParameter("@Event", SqlDbType.VarChar, 50));
            com.Parameters.Add(new SqlParameter("@Notification111", SqlDbType.VarChar));
            com.Parameters["@Event"].Value = "Add";
            com.Parameters["@Name"].Value = firstname;
            com.Parameters["@Notification111"].Value = Message;
            SqlDataAdapter sqlda = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            sqlConnection.Close();
            ViewBag.Announcement = "Announcement sent successfully...";
            return Json(ViewBag.Announcement, JsonRequestBehavior.AllowGet);
        }
        public ActionResult NotificationUpdateDetails(string Sno, string Name,string Message,string Date,Notifation notifation)
        {
            notifation.Sno = Sno;
            notifation.Name = Name;
            notifation.Notification = Message;
            notifation.Date = Date;
            return PartialView("NotificationUpdatePartialView",notifation);
        }
        public ActionResult NotificationUpdateDetailsSave(Notifation notifation)
        {         
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand com = new SqlCommand();         
            com.Connection = con;
            com.CommandText = "sp_notification";
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.Add(new SqlParameter("@Sno", SqlDbType.Int));
            com.Parameters.Add(new SqlParameter("@Event", SqlDbType.VarChar, 50));
            com.Parameters.Add(new SqlParameter("@Notification111", SqlDbType.VarChar));
            com.Parameters["@Event"].Value = "Update";
            com.Parameters["@Sno"].Value = notifation.Sno;
            com.Parameters["@Notification111"].Value = notifation.Notification;
            SqlDataAdapter sqlda = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            con.Close();
            TempData["Message"] = "Announcement Message Update successfully...";
            return RedirectToAction("Index");
        }
        public ActionResult NotificationDeleteDetails(Notifation notifation,string Sno)
        {
            notifation.Sno = Sno;
            return PartialView("NotificationDeletePartialView", notifation);
        }
        public ActionResult NotificationMessageDelete(Notifation notifation)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand com = new SqlCommand();
            com.CommandText = "sp_notification";
            com.CommandType = CommandType.StoredProcedure;
            com.Connection = con;
            com.Parameters.Add(new SqlParameter("@Event", SqlDbType.VarChar, 50));
            com.Parameters.Add(new SqlParameter("@Sno", SqlDbType.Int));
            com.Parameters["@Event"].Value = "Delete";
            com.Parameters["@Sno"].Value = notifation.Sno;
            SqlDataAdapter sqlda = new SqlDataAdapter(com);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            con.Close();
            TempData["DeleteMessage"] = "Announcement Message Delete successfully...";
            return RedirectToAction("Index");
        }
    }
}