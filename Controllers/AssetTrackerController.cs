using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetTrackerController : Controller
    {
        // GET: AssetTracker
        public ActionResult Index(AssetRequestModal asset)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select Team,EmployeeName from EmployeeList where EmpID=" + Session["username"] + "");
            sqlConnection.Open();
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                asset.EmployeeTeam = dr["Team"].ToString();
                asset.EmployeeName = dr["EmployeeName"].ToString();
            }
            sqlConnection.Close();

            SqlCommand cmd1 = new SqlCommand("SELECT DISTINCT Team FROM EmployeeList WHERE Team NOT IN('Support','HR')", sqlConnection);
            sqlConnection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd1);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            ViewBag.AssetTracker = ds.Tables[0];
            List<string> Teamlist = new List<string>();
            foreach (System.Data.DataRow dataRow in ds.Tables[0].Rows)
            {
                Teamlist.Add(dataRow["Team"].ToString());
            }
            SelectList Teams = new SelectList(Teamlist);
            ViewData["Teams"] = Teams;
            sqlConnection.Close();

            asset.RequestDate = DateTime.Now.ToString();

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

            return View(asset);
        }

        public JsonResult AssetType(string assetType)
        {
            //AssetRequestModal assetRequestModal = new AssetRequestModal();
            List<string> AssetTypeList = new List<string>();
            AssetTypeList = populateAssetTypeDropdown("select distinct[AssetType] from SaveAsset where Team='" + assetType + "' and SerialNumberVersionNumber is not null");
            return Json(AssetTypeList);
        }

        public JsonResult AssetName(string team, string assetType)
        {
            List<string> List = new List<string>();
            List = populateHWSWNameDropdown("select distinct[HWSWName] from SaveAsset where Team ='" + team + "' and AssetType ='" + assetType + "' ORDER BY HWSWName ASC");
            return Json(List);
        }
        public JsonResult serialNumber(string team, string HWSWName)
        {
            List<string> SaveAssetList = new List<string>();
            SaveAssetList = populateSerialNumberDropdown("SELECT SerialNumberVersionNumber from SaveAsset where HWSWName = '" + HWSWName + "' and Team='" + team + "' and (UnUsed IS NULL or UnUsed=0)");
            List<string> AssignAssetList = new List<string>();
            AssignAssetList = populateSerialNumberDropdown("SELECT SerialNumberVersionNumber from AssignAsset where HWSWName = '" + HWSWName + "' and Assetbelongsto='" + team + "' ");
            List<string> result = SaveAssetList.Where(f => !AssignAssetList.Contains(f)).ToList();
            return Json(result);
        }
        
        public JsonResult countAssetsList(string team, string HWSWName)
        {
            int availableAssets = countavailableAssets("SELECT count(SerialNumberVersionNumber) from SaveAsset where HWSWName = '" + HWSWName + "' and Team='" + team + "' and (UnUsed IS NULL or UnUsed=0)");
            int assignedAssets = countavailableAssets("SELECT count(SerialNumberVersionNumber) from AssignAsset where HWSWName = '" + HWSWName + "' and Assetbelongsto='" + team + "' ");
            int currentAvailable = availableAssets - assignedAssets;
            return Json(currentAvailable);
        }
        private static List<string> populateAssetTypeDropdown(string query)
        {
            List<string> assetTypeItems = new List<string>();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                assetTypeItems.Add(sdr["AssetType"].ToString());
            }
            sqlConnection.Close();
            return assetTypeItems;
        }
        private static List<string> populateHWSWNameDropdown(string query)
        {
            List<string> ListHWSWname = new List<string>();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                ListHWSWname.Add(sdr["HWSWName"].ToString());
            }
            sqlConnection.Close();
            return ListHWSWname;
        }
        private static List<string> populateSerialNumberDropdown(string query)
        {
            List<string> List = new List<string>();
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                List.Add(sdr["SerialNumberVersionNumber"].ToString());
            }
            sqlConnection.Close();
            return List;
        }

        private static int countavailableAssets(string query)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            int countAssests = Convert.ToInt32(sdr[0]);
            sdr.NextResult();
            return countAssests;
        }
        [HttpPost]
        public ActionResult SendRequest(AssetRequestModal assetRequest)
        {
            if(ModelState.IsValid)
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
                SqlCommand cmd = new SqlCommand("proAssetrequest");
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", Session["username"].ToString());
                cmd.Parameters.AddWithValue("@EmpTeam", assetRequest.EmployeeTeam);
                cmd.Parameters.AddWithValue("@AssetTeam", assetRequest.RequestRaisedTo);
                cmd.Parameters.AddWithValue("@AssetType", assetRequest.AssetType);
                cmd.Parameters.AddWithValue("@HWSWName", assetRequest.AssetName);
                cmd.Parameters.AddWithValue("@RequestDate", assetRequest.RequestDate);
                cmd.Parameters.AddWithValue("@ReturnDate", assetRequest.ExpectedReturnDate);
                cmd.Parameters.AddWithValue("@Quantity", assetRequest.NumberofAssetsRequired);
                cmd.Parameters.AddWithValue("@Approveornot", assetRequest.NumberofAssetsRequired);
                cmd.Parameters.AddWithValue("@Purpose", assetRequest.Purpose);
                cmd.Parameters.AddWithValue("@EmployeeName", assetRequest.EmployeeName);
                cmd.Parameters.AddWithValue("@Serialnumber", assetRequest.AvailableSerialNumber);
                int i = cmd.ExecuteNonQuery();


                SqlCommand command = new SqlCommand("proPermanentAssetrequest");
                command.Connection = sqlConnection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("EmpID", Session["username"].ToString());
                command.Parameters.AddWithValue("EmpTeam", assetRequest.EmployeeTeam);
                command.Parameters.AddWithValue("AssetTeam", assetRequest.RequestRaisedTo);
                command.Parameters.AddWithValue("AssetType", assetRequest.AssetType);
                command.Parameters.AddWithValue("HWSWName", assetRequest.AssetName);
                command.Parameters.AddWithValue("RequestDate", assetRequest.RequestDate);
                command.Parameters.AddWithValue("ReturnDate", assetRequest.ExpectedReturnDate);
                command.Parameters.AddWithValue("Quantity", assetRequest.NumberofAssetsRequired);
                command.Parameters.AddWithValue("Purpose", assetRequest.Purpose);
                command.Parameters.AddWithValue("Approveornot", assetRequest.NumberofAssetsRequired);
                command.Parameters.AddWithValue("Serialnumber", assetRequest.AvailableSerialNumber);
                int j = command.ExecuteNonQuery();
                sqlConnection.Close();

                SqlCommand sqlCommand = new SqlCommand("SELECT EmployeeName,Mail FROM EmployeeList WHERE EmpID='" + Session["username"] + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                string mail = null;
                string employeeNAme = null;
                if (reader.Read())
                {
                    mail = reader["Mail"].ToString();
                    employeeNAme = reader["EmployeeName"].ToString();
                    sqlConnection.Close();
                }
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
                mailMessage.To.Add(new MailAddress(mail));
                mailMessage.To.Add("phani.n@i-raysolutions.com");
                mailMessage.Subject = "AssetRequest";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" +
                    "<tr>" + "<th bgcolor='#ffc107'> EmpID </th>" + "<th bgcolor='#ffc107'> EmployeeName </th>" +
                    "<th bgcolor='#ffc107'> Request Raised to ? </th>" + "<th bgcolor='#ffc107'> AssetType </th>" +
                    "<th bgcolor='#ffc107'> SerialNumber </th>" + "<th bgcolor='#ffc107'> HWSWName </th>" +
                    "<th bgcolor='#ffc107'>RequestDate </th>" + "<th bgcolor='#ffc107'> Quantity </th>" + "</tr>" +
                    "<tr>" + "<td>" + Session["username"] + "</td>" + "<td>" +
                    employeeNAme + "</td>" + "<td>" + assetRequest.RequestRaisedTo
                    + "</td>" + "<td>" + assetRequest.AssetType + "</td>" + "<td style='font-weight:bold;font-size:20px;'>"
                    + assetRequest.AvailableSerialNumber + "</td>" + "<td>" +
                    assetRequest.AssetName + "</td>" + "<td>" + assetRequest.RequestDate
                    + "</td>" + "<td>" + assetRequest.NumberofAssetsRequired + "</td>" + "</tr>" + "</table>";
                mailMessage.Priority = MailPriority.High;
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);
                TempData["Message"] = "Asset Request Sent Successfully..";
                return RedirectToAction("Index");
            }
            return View(assetRequest);
        }
    }
}
