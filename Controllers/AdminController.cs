using I_RAY_ASSET_TRACKER_MVC.Models;
using iTextSharp.text;
using Newtonsoft.Json;
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AdminController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        string n;
        string m;
        string r;
        // GET: Admin
        public ActionResult Index()
        {
            string UserID = Session["username"].ToString();
            if (UserID != null)
            {
                int totalAssignRequests = countRequestAssets("select count(*) from Assetrequest");
                Session["totalAssignRequests"] = totalAssignRequests;

                SqlConnection sqlConnection = new SqlConnection(constr);
                //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
                SqlCommand cmd = new SqlCommand("select count(HWSWName) from SaveAsset");
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                ViewBag.TotalAssets = i;
                sqlConnection.Close();

                SqlCommand command = new SqlCommand("select count(HWSWName) from AssignAsset");
                command.Connection = sqlConnection;
                sqlConnection.Open();
                int j = Convert.ToInt32(command.ExecuteScalar());
                ViewBag.AssignAssets = j;
                sqlConnection.Close();

                SqlCommand comm = new SqlCommand("select count(HWSWName) from SaveAsset where Team='FSW'");
                comm.Connection = sqlConnection;
                sqlConnection.Open();
                int k = Convert.ToInt32(comm.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand comnd = new SqlCommand("select count(HWSWName) from SaveAsset where Team='SV'");
                comnd.Connection = sqlConnection;
                sqlConnection.Open();
                int l = Convert.ToInt32(comnd.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cond = new SqlCommand("select count(HWSWName) from SaveAsset where Team='S&R'");
                cond.Connection = sqlConnection;
                sqlConnection.Open();
                int m = Convert.ToInt32(cond.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cnd = new SqlCommand("select count(HWSWName) from SaveAsset where Team='FWV'");
                cnd.Connection = sqlConnection;
                sqlConnection.Open();
                int n = Convert.ToInt32(cnd.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cmnd = new SqlCommand("select count(HWSWName) from SaveAsset where Team='Automation'");
                cmnd.Connection = sqlConnection;
                sqlConnection.Open();
                int o = Convert.ToInt32(cmnd.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cd = new SqlCommand("select count(HWSWName) from SaveAsset where Team='IS'");
                cd.Connection = sqlConnection;
                sqlConnection.Open();
                int p = Convert.ToInt32(cd.ExecuteScalar());
                sqlConnection.Close();

                List<DataPoint> dataPoints = new List<DataPoint>();
                dataPoints.Add(new DataPoint("FSW", k));
                dataPoints.Add(new DataPoint("S&R", m));
                dataPoints.Add(new DataPoint("Automation", o));
                dataPoints.Add(new DataPoint("SV", l));  
                dataPoints.Add(new DataPoint("FWV", n));              
                dataPoints.Add(new DataPoint("IS", p));
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

                SqlCommand cmd1 = new SqlCommand("select count(HWSWName) from AssignAsset where Team='FSW'");
                cmd1.Connection = sqlConnection;
                sqlConnection.Open();
                int a = Convert.ToInt32(cmd1.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cmd2 = new SqlCommand("select count(HWSWName) from AssignAsset where Team='SV'");
                cmd2.Connection = sqlConnection;
                sqlConnection.Open();
                int b = Convert.ToInt32(cmd2.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cmd3 = new SqlCommand("select count(HWSWName) from AssignAsset where Team='S&R'");
                cmd3.Connection = sqlConnection;
                sqlConnection.Open();
                int c = Convert.ToInt32(cmd3.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cmd4 = new SqlCommand("select count(HWSWName) from AssignAsset where Team='FWV'");
                cmd4.Connection = sqlConnection;
                sqlConnection.Open();
                int d = Convert.ToInt32(cmd4.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cmd5 = new SqlCommand("select count(HWSWName) from AssignAsset where Team='Automation'");
                cmd5.Connection = sqlConnection;
                sqlConnection.Open();
                int e = Convert.ToInt32(cmd5.ExecuteScalar());
                sqlConnection.Close();

                SqlCommand cmd6 = new SqlCommand("select count(HWSWName) from AssignAsset where Team='IS'");
                cmd6.Connection = sqlConnection;
                sqlConnection.Open();
                int f = Convert.ToInt32(cmd6.ExecuteScalar());
                sqlConnection.Close();

                List<DataPoint> AssetdataPoints = new List<DataPoint>();
                AssetdataPoints.Add(new DataPoint("FSW", a));
                AssetdataPoints.Add(new DataPoint("SV", b));
                AssetdataPoints.Add(new DataPoint("S&R", c));
                AssetdataPoints.Add(new DataPoint("FWV", d));
                AssetdataPoints.Add(new DataPoint("Automation", e));
                AssetdataPoints.Add(new DataPoint("IS", f));
                ViewBag.AssetDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

                SaveAssetassetTypeFSW();
                SaveAssetassetTypeSV();
                SaveAssetassetTypeSR();
                SaveAssetassetTypeFWV();
                AssignAssetassetTypeFSW();
                AssignAssetassetTypeSV();
                AssignAssetassetTypeSR();
                AssignAssetassetTypeFWV();
                AssignAssetassetTypeIS();
                AssignAssetassetTypeAutomation();

                SqlCommand cmd7 = new SqlCommand("SELECT AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + UserID + "'order by AssignID");
                cmd7.Connection = sqlConnection;
                SqlDataAdapter adp = new SqlDataAdapter(cmd7);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                List<AdminTable1Model> dataModels = new List<AdminTable1Model>();
                foreach (DataRow row in dt.Rows)
                {
                    AdminTable1Model admin1List = new AdminTable1Model();
                    {
                        admin1List.AssignID = row["AssignID"].ToString();
                        admin1List.EmployeeName = row["EmployeeName"].ToString();
                        admin1List.Assetbelongsto = row["Assetbelongsto"].ToString();
                        admin1List.AssetType = row["AssetType"].ToString();
                        admin1List.HWSWName = row["HWSWName"].ToString();
                        admin1List.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                        admin1List.AssignDate = row["AssignDate"].ToString();
                        admin1List.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                    }
                    dataModels.Add(admin1List);
                }

                SqlCommand cmd8 = new SqlCommand("SELECT FromEmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssetTransferDate,FromEmpID,ToEmpID,TeamToWhichAssetisTransfered,ToEmployeeName FROM AssetTransfer where ToEmpID='" + UserID + "'");
                cmd8.Connection = sqlConnection;
                SqlDataAdapter ad = new SqlDataAdapter(cmd8);
                DataTable dataTable = new DataTable();
                ad.Fill(dataTable);
                List<AdminTable2Model> list = new List<AdminTable2Model>();
                foreach (DataRow row in dataTable.Rows)
                {
                    AdminTable2Model admin2List = new AdminTable2Model();
                    {
                        admin2List.FromEmployeeName = row["FromEmployeeName"].ToString();
                        admin2List.ToEmployeeName = row["ToEmployeeName"].ToString();
                        admin2List.Assetbelongsto = row["Assetbelongsto"].ToString();
                        admin2List.AssetType = row["AssetType"].ToString();
                        admin2List.HWSWName = row["HWSWName"].ToString();
                        admin2List.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                        admin2List.AssetTransferDate = row["AssetTransferDate"].ToString();
                        admin2List.FromEmpID = row["FromEmpID"].ToString();
                        admin2List.ToEmpID = row["ToEmpID"].ToString();
                        admin2List.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    }
                    list.Add(admin2List);
                }

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

                SqlCommand com = new SqlCommand();
                sqlConnection.Open();
                com.Connection = sqlConnection;
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

                var viewModal = new AdminModel
                {
                    adminTable1Model = dataModels,
                    adminTable2Model = list,
                    adminNotificationManager = dataList
                };
                return View(viewModal);               
            }
            else
            {
                return Redirect("/~UserLogin/Login");
            }
        }
        private int countRequestAssets(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            int countAssests = Convert.ToInt32(sdr[0]);
            sdr.NextResult();
            return countAssests;
        }
        public void SaveAssetassetTypeFSW()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from SaveAsset where AssetType='HI' and Team='FSW'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from SaveAsset where AssetType='Adaptors' and Team='FSW'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from SaveAsset where AssetType='iDevices' and Team='FSW'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from SaveAsset where AssetType='Accessories' and Team='FSW'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from SaveAsset where AssetType='Other' and Team='FSW'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from SaveAsset where AssetType='Interface' and Team='FSW'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from SaveAsset where AssetType='CHARGER' and Team='FSW'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from SaveAsset where AssetType='Android Devices' and Team='FSW'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.SaveAssetTypeFSWDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void SaveAssetassetTypeSV()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from SaveAsset where AssetType='HI' and Team='SV'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from SaveAsset where AssetType='Adaptors' and Team='SV'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from SaveAsset where AssetType='iDevices' and Team='SV'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from SaveAsset where AssetType='Accessories' and Team='SV'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from SaveAsset where AssetType='Other' and Team='SV'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from SaveAsset where AssetType='Interface' and Team='SV'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from SaveAsset where AssetType='CHARGER' and Team='SV'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from SaveAsset where AssetType='Android Devices' and Team='SV'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.SaveAssetTypeSVDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }

        public void SaveAssetassetTypeSR()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from SaveAsset where AssetType='HI' and Team='S&R'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from SaveAsset where AssetType='Adaptors' and Team='S&R'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from SaveAsset where AssetType='iDevices' and Team='S&R'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from SaveAsset where AssetType='Accessories' and Team='S&R'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from SaveAsset where AssetType='Other' and Team='S&R'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from SaveAsset where AssetType='Interface' and Team='S&R'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from SaveAsset where AssetType='CHARGER' and Team='S&R'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from SaveAsset where AssetType='Android Devices' and Team='S&R'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.SaveAssetTypeSRDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void SaveAssetassetTypeFWV()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from SaveAsset where AssetType='HI' and Team='FWV'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from SaveAsset where AssetType='Adaptors' and Team='FWV'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from SaveAsset where AssetType='iDevices' and Team='FWV'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from SaveAsset where AssetType='Accessories' and Team='FWV'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from SaveAsset where AssetType='Other' and Team='FWV'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from SaveAsset where AssetType='Interface' and Team='FWV'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from SaveAsset where AssetType='CHARGER' and Team='FWV'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from SaveAsset where AssetType='Android Devices' and Team='FWV'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.SaveAssetTypeFWVDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void AssignAssetassetTypeFSW()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from AssignAsset where AssetType='HI' and Team='FSW'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from AssignAsset where AssetType='Adaptors' and Team='FSW'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from AssignAsset where AssetType='iDevices' and Team='FSW'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from AssignAsset where AssetType='Accessories' and Team='FSW'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from AssignAsset where AssetType='Other' and Team='FSW'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from AssignAsset where AssetType='Interface' and Team='FSW'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from AssignAsset where AssetType='CHARGER' and Team='FSW'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from AssignAsset where AssetType='Android Devices' and Team='FSW'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.AssignAssetTypeFSWDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void AssignAssetassetTypeSV()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from AssignAsset where AssetType='HI' and Team='SV'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from AssignAsset where AssetType='Adaptors' and Team='SV'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from AssignAsset where AssetType='iDevices' and Team='SV'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from AssignAsset where AssetType='Accessories' and Team='SV'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from AssignAsset where AssetType='Other' and Team='SV'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from AssignAsset where AssetType='Interface' and Team='SV'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from AssignAsset where AssetType='CHARGER' and Team='SV'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from AssignAsset where AssetType='Android Devices' and Team='SV'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.AssignAssetTypeSVDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void AssignAssetassetTypeSR()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from AssignAsset where AssetType='HI' and Team='S&R'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from AssignAsset where AssetType='Adaptors' and Team='S&R'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from AssignAsset where AssetType='iDevices' and Team='S&R'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from AssignAsset where AssetType='Accessories' and Team='S&R'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from AssignAsset where AssetType='Other' and Team='S&R'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from AssignAsset where AssetType='Interface' and Team='S&R'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from AssignAsset where AssetType='CHARGER' and Team='S&R'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from AssignAsset where AssetType='Android Devices' and Team='S&R'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.AssignAssetTypeSRDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void AssignAssetassetTypeFWV()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from AssignAsset where AssetType='HI' and Team='FWV'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from AssignAsset where AssetType='Adaptors' and Team='FWV'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from AssignAsset where AssetType='iDevices' and Team='FWV'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from AssignAsset where AssetType='Accessories' and Team='FWV'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from AssignAsset where AssetType='Other' and Team='FWV'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from AssignAsset where AssetType='Interface' and Team='FWV'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from AssignAsset where AssetType='CHARGER' and Team='FWV'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from AssignAsset where AssetType='Android Devices' and Team='FWV'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.AssignAssetTypeFWVDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void AssignAssetassetTypeIS()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from AssignAsset where AssetType='HI' and Team='IS'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from AssignAsset where AssetType='Adaptors' and Team='IS'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from AssignAsset where AssetType='iDevices' and Team='IS'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from AssignAsset where AssetType='Accessories' and Team='IS'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from AssignAsset where AssetType='Other' and Team='IS'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from AssignAsset where AssetType='Interface' and Team='IS'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from AssignAsset where AssetType='CHARGER' and Team='IS'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from AssignAsset where AssetType='Android Devices' and Team='IS'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.AssignAssetTypeISDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public void AssignAssetassetTypeAutomation()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            //sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select count(*) from AssignAsset where AssetType='HI' and Team='Automation'");
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            int a = Convert.ToInt32(cmd.ExecuteScalar());

            SqlCommand cmd1 = new SqlCommand("select count(*) from AssignAsset where AssetType='Adaptors' and Team='Automation'");
            cmd1.Connection = sqlConnection;
            int b = Convert.ToInt32(cmd1.ExecuteScalar());

            SqlCommand cmd2 = new SqlCommand("select count(*) from AssignAsset where AssetType='iDevices' and Team='Automation'");
            cmd2.Connection = sqlConnection;
            int c = Convert.ToInt32(cmd2.ExecuteScalar());

            SqlCommand cmd3 = new SqlCommand("select count(*) from AssignAsset where AssetType='Accessories' and Team='Automation'");
            cmd3.Connection = sqlConnection;
            int d = Convert.ToInt32(cmd3.ExecuteScalar());

            SqlCommand cmd4 = new SqlCommand("select count(*) from AssignAsset where AssetType='Other' and Team='Automation'");
            cmd4.Connection = sqlConnection;
            int e = Convert.ToInt32(cmd4.ExecuteScalar());

            SqlCommand cmd5 = new SqlCommand("select count(*) from AssignAsset where AssetType='Interface' and Team='Automation'");
            cmd5.Connection = sqlConnection;
            int f = Convert.ToInt32(cmd5.ExecuteScalar());

            SqlCommand cmd6 = new SqlCommand("select count(*) from AssignAsset where AssetType='CHARGER' and Team='Automation'");
            cmd6.Connection = sqlConnection;
            int g = Convert.ToInt32(cmd6.ExecuteScalar());

            SqlCommand cmd7 = new SqlCommand("select count(*) from AssignAsset where AssetType='Android Devices' and Team='Automation'");
            cmd7.Connection = sqlConnection;
            int h = Convert.ToInt32(cmd7.ExecuteScalar());

            List<DataPoint> AssetdataPoints = new List<DataPoint>();
            AssetdataPoints.Add(new DataPoint("HI", a));
            AssetdataPoints.Add(new DataPoint("Adaptors", b));
            AssetdataPoints.Add(new DataPoint("iDevices", c));
            AssetdataPoints.Add(new DataPoint("Accessories", d));
            AssetdataPoints.Add(new DataPoint("Other", e));
            AssetdataPoints.Add(new DataPoint("Interface", f));
            AssetdataPoints.Add(new DataPoint("Charger", g));
            AssetdataPoints.Add(new DataPoint("AndroidDevices", h));
            ViewBag.AssignAssetTypeAutomationDataPoints = JsonConvert.SerializeObject(AssetdataPoints);

            sqlConnection.Close();
        }
        public ActionResult TransferAsset(int? Id, AdminTable1Model userList)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate from AssignAsset where AssignID=" + Id, sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<AdminTable1Model> dataModels = new List<AdminTable1Model>();
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

            return PartialView("AdminTransferAsset", userList);
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
        public ActionResult SaveTransferAsset(AdminTable1Model userModel)
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
            TempData["TransferAssetMessage"] = "Asset Transfer Successfully..";
            return RedirectToAction("Index");
        }
        public ActionResult SaveTransferAssetFromEmployee(int fromEmpID,string serialnumber , AdminTable2Model table2Model)
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
                "<tr>" + "<td>" +table2Model.FromEmployeeName + "</td>" + "<td>" + table2Model.Assetbelongsto + "</td>" + "<td>" + table2Model.AssetType + "</td>" + "<td>" + table2Model.HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + table2Model.SerialNumberVersionNumber + "</td>" + "<td>" + table2Model.FromEmpID + "</td>" + "<td>" + table2Model.ToEmpID + "</td>" + "<td>" + table2Model.TeamToWhichAssetisTransfered + "</td>" + "<td>" + table2Model.ToEmployeeName + "</td>" + "</tr>" + "</table>";
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Send(mailMessage);
            return RedirectToAction("Index");
        }
        public ActionResult RejectTransferAssetFromEmployee(int fromEmpID, string serialnumber, AdminTable2Model table2Model)
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