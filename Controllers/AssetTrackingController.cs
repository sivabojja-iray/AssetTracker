using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetTrackingController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: AssetTracking
        public ActionResult Index()
        {
            List<AssetTrackingModel> list = assetTrackingList("select AssignID,EmpID,EmployeeName,Team,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate from AssignAsset");
            return View(list);
        }
        private List<AssetTrackingModel> assetTrackingList(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);          
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<AssetTrackingModel> dataModels = new List<AssetTrackingModel>();

            foreach (DataRow row in dt.Rows)
            {
                AssetTrackingModel assetTrackingList = new AssetTrackingModel();
                {
                    assetTrackingList.AssignID = row["AssignID"].ToString();
                    assetTrackingList.EmpID = row["EmpID"].ToString();
                    assetTrackingList.EmployeeName = row["EmployeeName"].ToString();
                    assetTrackingList.Team = row["Team"].ToString();
                    assetTrackingList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    assetTrackingList.AssetType = row["AssetType"].ToString();
                    assetTrackingList.HWSWName = row["HWSWName"].ToString();
                    assetTrackingList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    assetTrackingList.AssignDate = row["AssignDate"].ToString();
                }
                dataModels.Add(assetTrackingList);
            }
            return dataModels;
        }

    }
}