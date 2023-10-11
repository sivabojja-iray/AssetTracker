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
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AdminController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: Admin
        public ActionResult Index(int? page)
        {
            if (Session["username"] != null)
            {
                //int pageSize = 5;
                //int pageIndex = 1;
                //pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;

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

                SqlCommand cmd7 = new SqlCommand("SELECT AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + Session["username"] + "'order by AssignID");
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

                SqlCommand cmd8 = new SqlCommand("SELECT FromEmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssetTransferDate,FromEmpID,ToEmpID,TeamToWhichAssetisTransfered,ToEmployeeName FROM AssetTransfer where ToEmpID='" + Session["username"] + "'");
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

                var viewModal = new AdminModel
                {
                    adminTable1Model = dataModels,
                    adminTable2Model = list
                };
                Thread.Sleep(5000);
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
            List<string> EmployeeList = new List<string>();
            EmployeeList = populateEmployeeListDropdown("SELECT DISTINCT EmpID,EmployeeName FROM EmployeeList WHERE Team='" + selectedTeam + "'");
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
            cmd.Parameters.AddWithValue("@FromEmpID", Session["username"]);
            cmd.Parameters.AddWithValue("@ToEmpID", i);
            cmd.Parameters.AddWithValue("@TeamToWhichAssetisTransfered", userModel.AssetTransferTeam);
            cmd.Parameters.AddWithValue("@ToEmployeeName", userModel.AssetTransferEmployeeName);
            cmd.ExecuteNonQuery();
            sqlConnection.Close();
            Thread.Sleep(1000);
            return RedirectToAction("Index");
        }
        public ActionResult SaveTransferAssetFromEmployee(int? id, AdminTable2Model table2Model)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select ToEmpID,ToEmployeeName,TeamToWhichAssetisTransfered,SerialNumberVersionNumber from AssetTransfer where FromEmpID='" + id + "'", sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            table2Model.ToEmpID = dt.Rows[0][0].ToString();
            table2Model.ToEmployeeName = dt.Rows[0][1].ToString();
            table2Model.TeamToWhichAssetisTransfered = dt.Rows[0][2].ToString();
            table2Model.SerialNumberVersionNumber = dt.Rows[0][3].ToString();
            SqlCommand command = new SqlCommand("UPDATE AssignAsset SET EmpID='" + table2Model.ToEmpID + "',EmployeeName='" + table2Model.ToEmployeeName + "',Team='" + table2Model.TeamToWhichAssetisTransfered + "' where EmpID='" + id + "' and SerialNumberVersionNumber='" + table2Model.SerialNumberVersionNumber + "'", sqlConnection);
            sqlConnection.Open();
            command.CommandType = CommandType.Text;
            command.ExecuteNonQuery();
            sqlConnection.Close();

            SqlCommand sql = new SqlCommand("DELETE FROM AssetTransfer WHERE SerialNumberVersionNumber='" + table2Model.SerialNumberVersionNumber + "' and FromEmpID='" + id + "'", sqlConnection);
            sqlConnection.Open();
            sql.CommandType = CommandType.Text;
            sql.ExecuteNonQuery();
            sqlConnection.Close();

            return RedirectToAction("Index");
        }
        public ActionResult RejectTransferAssetFromEmployee(int? id, AdminTable2Model table2Model)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select SerialNumberVersionNumber from AssetTransfer where FromEmpID='" + id + "'", sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            table2Model.SerialNumberVersionNumber = dt.Rows[0][0].ToString();

            SqlCommand sql = new SqlCommand("DELETE FROM AssetTransfer WHERE SerialNumberVersionNumber='" + table2Model.SerialNumberVersionNumber + "' and FromEmpID='" + id + "'", sqlConnection);
            sqlConnection.Open();
            sql.CommandType = CommandType.Text;
            sql.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("Index");
        }
    }
}