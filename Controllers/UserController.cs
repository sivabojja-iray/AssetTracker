using I_RAY_ASSET_TRACKER_MVC.Models;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class UserController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        string m;
        string n;
        string r;
        // GET: User
        public ActionResult Index()
        {
            string UserID = Session["username"].ToString();
            if (UserID != null)
            {            
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT COUNT(AssetType) FROM AssignAsset where EmpID='" + UserID + "'");
                cmd.Connection = con;
                con.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                ViewBag.TotalAssetsUserHave = i;

                SqlCommand comnd = new SqlCommand("SELECT AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + UserID + "'order by AssignID");
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
                        userList.EmployeeName = row["EmployeeName"].ToString();
                        userList.Assetbelongsto = row["Assetbelongsto"].ToString();
                        userList.AssetType = row["AssetType"].ToString();
                        userList.HWSWName = row["HWSWName"].ToString();
                        userList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                        userList.AssignDate = row["AssignDate"].ToString();
                        userList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                    }
                    dataModels.Add(userList);
                }

                SqlCommand command = new SqlCommand("SELECT FromEmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssetTransferDate,FromEmpID,ToEmpID,TeamToWhichAssetisTransfered,ToEmployeeName FROM AssetTransfer where ToEmpID='" + UserID + "'");
                command.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                List<Table2Model> list = new List<Table2Model>();
                foreach (DataRow row in dataTable.Rows)
                {
                    Table2Model table2Model = new Table2Model();
                    {
                        table2Model.FromEmployeeName = row["FromEmployeeName"].ToString();
                        table2Model.ToEmployeeName = row["ToEmployeeName"].ToString();
                        table2Model.Assetbelongsto = row["Assetbelongsto"].ToString();
                        table2Model.AssetType = row["AssetType"].ToString();
                        table2Model.HWSWName = row["HWSWName"].ToString();
                        table2Model.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                        table2Model.AssetTransferDate = row["AssetTransferDate"].ToString();
                        table2Model.FromEmpID = row["FromEmpID"].ToString();
                        table2Model.ToEmpID = row["ToEmpID"].ToString();
                        table2Model.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    }
                    list.Add(table2Model);
                }

                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "sp_notification";
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@Event", SqlDbType.VarChar, 50));
                com.Parameters["@Event"].Value = "Select";
                SqlDataAdapter sqlda = new SqlDataAdapter(com);
                DataTable dataTa = new DataTable();
                sqlda.Fill(dataTa);
                List<Notifation> dataList = new List<Notifation>();
                foreach (DataRow row in dataTa.Rows)
                {
                    Notifation notifation = new Notifation();
                    {
                        notifation.Name = row["Name"].ToString();
                        notifation.Notification = row["Notification111"].ToString();
                        notifation.Date = row["Last_Update"].ToString();
                    }
                    dataList.Add(notifation);
                }

                var viewModal = new UserModel
                {
                    Table1Data=dataModels,
                    Table2Data=list,
                    userNotificationManager=dataList
                };
                con.Close();
                return View(viewModal);
            }
            else
            {
                return Redirect("/~UserLogin/Login");
            }
        }      
        public JsonResult EmployeeName(string selectedTeam)
        {
            string UserID = Session["username"].ToString();
            List<string> EmployeeList = new List<string>();
            EmployeeList = populateEmployeeListDropdown("SELECT DISTINCT EmpID,EmployeeName FROM EmployeeList WHERE Team='" + selectedTeam + "' AND EmpID NOT IN('" + UserID + "')");
            return Json(EmployeeList);
        }
        private List<string> populateEmployeeListDropdown(string query)
        {
            List<string> EmployeeList = new List<string>();
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                //EmployeeList.Add(sdr["EmpID"].ToString());
                EmployeeList.Add(sdr["EmployeeName"].ToString());
            }
            sqlConnection.Close();
            return EmployeeList;
        }  
        public ActionResult TransferAsset(int? Id, Table1Model userList)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);        
            SqlCommand cmd = new SqlCommand("select AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate from AssignAsset where AssignID=" + Id, sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<Table1Model> dataModels = new List<Table1Model>();
            foreach (DataRow row in dt.Rows)
            {             
                    userList.AssignID = row["AssignID"].ToString();
                    userList.EmployeeName = row["EmployeeName"].ToString();
                    userList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    userList.AssetType = row["AssetType"].ToString();
                    userList.HWSWName = row["HWSWName"].ToString();
                    userList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    userList.AssignDate = row["AssignDate"].ToString();
                    userList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();              
            }
            dataModels.Add(userList);

            SqlCommand cmd1 = new SqlCommand("SELECT DISTINCT Team FROM EmployeeList WHERE Team NOT IN('Support','HR')", sqlConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd1);
            DataTable data = new DataTable();
            dataAdapter.Fill(data);
            List<string> Teamlist = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                Teamlist.Add(row["Team"].ToString());
            }
            SelectList Teams = new SelectList(Teamlist);
            ViewData["Teams"] = Teams;

            return PartialView("TransferAsset",userList);
        }
        public ActionResult SaveTransferAsset(Table1Model userModel) 
        {
            string UserID = Session["username"].ToString();
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("INSERT INTO AssetTransfer VALUES(@FromEmployeeName,@Assetbelongsto,@AssetType,@HWSWName,@SerialNumberVersionNumber,@AssetTransferDate,@FromEmpID,@ToEmpID,@TeamToWhichAssetisTransfered,@ToEmployeeName)", sqlConnection);
            cmd.CommandType = CommandType.Text;
            SqlCommand command = new SqlCommand("select EmpID from EmployeeList where EmployeeName='" + userModel.AssetTransferEmployeeName + "'", sqlConnection);
            command.CommandType = CommandType.Text;
            sqlConnection.Open();
            int i = Convert.ToInt32(command.ExecuteScalar());
            cmd.Parameters.AddWithValue("@FromEmployeeName", userModel.EmployeeName);
            cmd.Parameters.AddWithValue("@Assetbelongsto", userModel.Assetbelongsto);
            cmd.Parameters.AddWithValue("@AssetType", userModel.AssetType);
            cmd.Parameters.AddWithValue("@HWSWName", userModel.HWSWName);
            cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", userModel.SerialNumberVersionNumber);
            cmd.Parameters.AddWithValue("@AssetTransferDate", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@FromEmpID", UserID);
            cmd.Parameters.AddWithValue("@ToEmpID", i);
            cmd.Parameters.AddWithValue("@TeamToWhichAssetisTransfered", userModel.AssetTransferTeam);
            cmd.Parameters.AddWithValue("@ToEmployeeName", userModel.AssetTransferEmployeeName);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
            TempData["TransferAssetMessage"] = "Asset Transfer Successfully..";
            SqlCommand cmd5 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE EmpID='" + i + "'", sqlConnection);
            SqlCommand cmd6 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE EmpID='" + UserID + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dr = cmd5.ExecuteReader();
            if (dr.Read())
            {
                n = dr["Mail"].ToString();
                sqlConnection.Close();
                sqlConnection.Open();
                SqlDataReader dr2 = cmd6.ExecuteReader();
                if (dr2.Read())
                {
                    m = dr2["Mail"].ToString();
                    sqlConnection.Close();
                }
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
                mailMessage.To.Add(new MailAddress(n));
                mailMessage.To.Add(new MailAddress(m));
                mailMessage.Subject = "Asset Transfer Request";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th bgcolor='#ffc107'> FromEmloyeeName </th>" + "<th bgcolor='#ffc107'> Assetbelongsto </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'> HWSWName </th>" + "<th bgcolor='#ffc107'> SerialNumber/VersionNumber </th>" + "<th bgcolor='#ffc107'> FromEmpID </th>" + "<th bgcolor='#ffc107'> ToEmpID </th>" + "<th bgcolor='#ffc107'> Asset is being Transferred </th>" + "<th bgcolor='#ffc107'> EmployeeName </th>" + "</tr>" +
                    "<tr>" + "<td>" + userModel.EmployeeName + "</td>" + "<td>" + userModel.Assetbelongsto + "</td>" + "<td>" + userModel.AssetType + "</td>" + "<td>" + userModel.HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + userModel.SerialNumberVersionNumber + "</td>" + "<td>" + UserID + "</td>" + "<td>" + i + "</td>" + "<td>" + userModel.AssetTransferTeam + "</td>" + "<td>" + userModel.AssetTransferEmployeeName + "</td>" + "</tr>" + "</table>";
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
                sqlConnection.Close();
            }         
            return RedirectToAction("Index"); 
        }
        public ActionResult SaveTransferAssetFromEmployee(int fromEmpID, string serialnumber, Table2Model table2Model)
        {
            string UserID = Session["username"].ToString();
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select FromEmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssetTransferDate,FromEmpID,ToEmpID,TeamToWhichAssetisTransfered,ToEmployeeName from AssetTransfer where SerialNumberVersionNumber='" + serialnumber + "'and FromEmpID='" + fromEmpID + "'", sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            table2Model.FromEmployeeName = dt.Rows[0][0].ToString();
            table2Model.Assetbelongsto = dt.Rows[0][1].ToString();
            table2Model.AssetType = dt.Rows[0][2].ToString();
            table2Model.HWSWName = dt.Rows[0][3].ToString();
            table2Model.SerialNumberVersionNumber = dt.Rows[0][4].ToString();
            table2Model.AssetTransferDate = dt.Rows[0][5].ToString();
            table2Model.FromEmpID = dt.Rows[0][6].ToString();
            table2Model.ToEmpID = dt.Rows[0][7].ToString();
            table2Model.TeamToWhichAssetisTransfered = dt.Rows[0][8].ToString();
            table2Model.ToEmployeeName = dt.Rows[0][9].ToString();
            SqlCommand command = new SqlCommand("UPDATE AssignAsset SET EmpID='" + table2Model.ToEmpID + "',EmployeeName='" + table2Model.ToEmployeeName + "',Team='" + table2Model.TeamToWhichAssetisTransfered + "' where EmpID='" + table2Model.FromEmpID + "' and SerialNumberVersionNumber='" + serialnumber + "'", sqlConnection);
            sqlConnection.Open();
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            sqlConnection.Close();

            SqlCommand cmd8 = new SqlCommand("INSERT INTO AssetTransferRecords VALUES(@FromEmpID,@FromEmployeeName,@Assetbelongsto,@AssetType,@HWSWName,@SerialNumberVersionNumber,@AssetTransferDate,@ToEmpID,@TeamToWhichAssetisTransfered,@ToEmployeeName)", sqlConnection);
            cmd8.CommandType = CommandType.Text;
            cmd8.Parameters.AddWithValue("@FromEmpID", table2Model.FromEmpID);
            cmd8.Parameters.AddWithValue("@FromEmployeeName", table2Model.FromEmployeeName);
            cmd8.Parameters.AddWithValue("@Assetbelongsto", table2Model.Assetbelongsto);
            cmd8.Parameters.AddWithValue("@AssetType", table2Model.AssetType);
            cmd8.Parameters.AddWithValue("@HWSWName", table2Model.HWSWName);
            cmd8.Parameters.AddWithValue("@SerialNumberVersionNumber", table2Model.SerialNumberVersionNumber);
            cmd8.Parameters.AddWithValue("@AssetTransferDate", table2Model.AssetType);
            cmd8.Parameters.AddWithValue("@ToEmpID", table2Model.ToEmpID);
            cmd8.Parameters.AddWithValue("@TeamToWhichAssetisTransfered", table2Model.TeamToWhichAssetisTransfered);
            cmd8.Parameters.AddWithValue("@ToEmployeeName", table2Model.ToEmployeeName);
            sqlConnection.Open();
            cmd8.ExecuteNonQuery();
            sqlConnection.Close();

            SqlCommand sql = new SqlCommand("DELETE FROM AssetTransfer WHERE SerialNumberVersionNumber='" + serialnumber + "' and FromEmpID='" + fromEmpID + "'", sqlConnection);
            sqlConnection.Open();
            sql.CommandType = CommandType.Text;
            sql.ExecuteNonQuery();
            sqlConnection.Close();

            SqlCommand cmd5 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE EmpID='" + UserID + "'", sqlConnection);
            SqlCommand cmd6 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE EmployeeName='" + table2Model.FromEmployeeName + "'", sqlConnection);
            SqlCommand cmd7 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE Role='Manager' AND Team='" + table2Model.TeamToWhichAssetisTransfered + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dr = cmd5.ExecuteReader();
            if (dr.Read())
            {
                m = dr["Mail"].ToString();
                sqlConnection.Close();
            }
            sqlConnection.Open();
            SqlDataReader dr2 = cmd6.ExecuteReader();
            if (dr2.Read())
            {
                n = dr2["Mail"].ToString();
                sqlConnection.Close();
            }
            sqlConnection.Open();
            SqlDataReader dr3 = cmd7.ExecuteReader();
            if (dr3.Read())
            {
                r = dr3["Mail"].ToString();
                sqlConnection.Close();
            }
            else
            {
                r = "phani.n@i-raysolutions.com";
            }
            TempData["TransferAssetMessage"] = "Tranfer asset approval successfully..";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
            mailMessage.To.Add(new MailAddress(n));
            mailMessage.To.Add(new MailAddress(m));
            mailMessage.To.Add(new MailAddress(r));
            mailMessage.To.Add("phani.n@i-raysolutions.com");
            mailMessage.Subject = "Asset Request has been Aproved";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" +
                "<tr>" + "<th bgcolor='#ffc107'> FromEmloyeeName </th>" + "<th bgcolor='#ffc107'> Assetbelongsto </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'> HWSWName </th>" + "<th bgcolor='#ffc107'> SerialNumber/VersionNumber </th>" + "<th bgcolor='#ffc107'> FromEmpID </th>" + "<th bgcolor='#ffc107'> ToEmpID </th>" + "<th bgcolor='#ffc107'> Asset is being Transferred </th>" + "<th bgcolor='#ffc107'> EmployeeName </th>" + "</tr>" +
                "<tr>" + "<td>" + table2Model.FromEmployeeName + "</td>" + "<td>" + table2Model.Assetbelongsto + "</td>" + "<td>" + table2Model.AssetType + "</td>" + "<td>" + table2Model.HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + table2Model.SerialNumberVersionNumber + "</td>" + "<td>" + table2Model.FromEmpID + "</td>" + "<td>" + table2Model.ToEmpID + "</td>" + "<td>" + table2Model.TeamToWhichAssetisTransfered + "</td>" + "<td>" + table2Model.ToEmployeeName + "</td>" + "</tr>" + "</table>";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
            return RedirectToAction("Index");
        }
        public ActionResult RejectTransferAssetFromEmployee(int fromEmpID, string serialnumber, Table2Model table2Model)
        {
            string UserID = Session["username"].ToString();
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select FromEmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssetTransferDate,FromEmpID,ToEmpID,TeamToWhichAssetisTransfered,ToEmployeeName from AssetTransfer where SerialNumberVersionNumber='" + serialnumber + "'and FromEmpID='" + fromEmpID + "'", sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            table2Model.FromEmployeeName = dt.Rows[0][0].ToString();
            table2Model.Assetbelongsto = dt.Rows[0][1].ToString();
            table2Model.AssetType = dt.Rows[0][2].ToString();
            table2Model.HWSWName = dt.Rows[0][3].ToString();
            table2Model.SerialNumberVersionNumber = dt.Rows[0][4].ToString();
            table2Model.AssetTransferDate = dt.Rows[0][5].ToString();
            table2Model.FromEmpID = dt.Rows[0][6].ToString();
            table2Model.ToEmpID = dt.Rows[0][7].ToString();
            table2Model.TeamToWhichAssetisTransfered = dt.Rows[0][8].ToString();
            table2Model.ToEmployeeName = dt.Rows[0][9].ToString();

            SqlCommand sql = new SqlCommand("DELETE FROM AssetTransfer WHERE SerialNumberVersionNumber='" + serialnumber + "' and FromEmpID='" + fromEmpID + "'", sqlConnection);
            sqlConnection.Open();
            sql.CommandType = CommandType.Text;
            sql.ExecuteNonQuery();
            sqlConnection.Close();

            SqlCommand cmd5 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE EmpID='" + UserID + "'", sqlConnection);
            SqlCommand cmd6 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE EmpID='" + fromEmpID + "'", sqlConnection);
            SqlCommand cmd7 = new SqlCommand("SELECT Mail FROM EmployeeList WHERE Role='Manager' AND Team='" + table2Model.TeamToWhichAssetisTransfered + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dr = cmd5.ExecuteReader();
            if (dr.Read())
            {
                m = dr["Mail"].ToString();
                sqlConnection.Close();
            }
            sqlConnection.Open();
            SqlDataReader dr2 = cmd6.ExecuteReader();
            if (dr2.Read())
            {
                n = dr2["Mail"].ToString();
                sqlConnection.Close();
            }
            sqlConnection.Open();
            SqlDataReader dr3 = cmd7.ExecuteReader();
            if (dr3.Read())
            {
                r = dr3["Mail"].ToString();
                sqlConnection.Close();
            }
            else
            {
                r = "phani.n@i-raysolutions.com";
            }

            TempData["RejectTransferAssetMessage"] = "Asset Transfer has been Rejected..";

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
            mailMessage.To.Add(new MailAddress(n));
            mailMessage.To.Add(new MailAddress(m));
            mailMessage.To.Add(new MailAddress(r));
            mailMessage.To.Add("phani.n@i-raysolutions.com");
            mailMessage.Subject = "Asset Request has been Rejected";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" +
                "<tr>" + "<th bgcolor='#ffc107'> FromEmloyeeName </th>" + "<th bgcolor='#ffc107'> Assetbelongsto </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'> HWSWName </th>" + "<th bgcolor='#ffc107'> SerialNumber/VersionNumber </th>" + "<th bgcolor='#ffc107'> FromEmpID </th>" + "<th bgcolor='#ffc107'> ToEmpID </th>" + "<th bgcolor='#ffc107'> Asset is being Transferred </th>" + "<th bgcolor='#ffc107'> EmployeeName </th>" + "</tr>" +
                "<tr>" + "<td>" + table2Model.FromEmployeeName + "</td>" + "<td>" + table2Model.Assetbelongsto + "</td>" + "<td>" + table2Model.AssetType + "</td>" + "<td>" + table2Model.HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + table2Model.SerialNumberVersionNumber + "</td>" + "<td>" + table2Model.FromEmpID + "</td>" + "<td>" + table2Model.ToEmpID + "</td>" + "<td>" + table2Model.TeamToWhichAssetisTransfered + "</td>" + "<td>" + table2Model.ToEmployeeName + "</td>" + "</tr>" + "</table>";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);

            return RedirectToAction("Index");
        }
    }
}