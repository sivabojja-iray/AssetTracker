using I_RAY_ASSET_TRACKER_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AdminController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["username"] != null)
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
                return View();
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
    }
}