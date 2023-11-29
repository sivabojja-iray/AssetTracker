using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using iTextSharp.text;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetRequestRejectedController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        List<AssetRequestReject> list;
        // GET: AssetRequestRejected
        public ActionResult Index()
        {
            SqlConnection con = new SqlConnection(constr);
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
            return View();
        }
        public ActionResult AssetRequestRejectedTeamList(string Team)
        {
            if (Team == "FSW")
            {
                list = assetRequestReject("ProFSWRejectedassets");
            }
            else if (Team == "SV")
            {
                list = assetRequestReject("ProSVRejectedassets");
            }
            var viewModel = new AssetRequestRejectedModel
            {
                assetRequestRejects = list
            };
            return PartialView("AssetRequestRejectedTeamList", viewModel);
        }
        private List<AssetRequestReject> assetRequestReject(string query)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            list = new List<AssetRequestReject>();
            foreach (DataRow row in dt.Rows)
            {
                AssetRequestReject assetRequestReject = new AssetRequestReject();
                {
                    assetRequestReject.EmpID = row["EmpID"].ToString();
                    assetRequestReject.EmpTeam = row["EmpTeam"].ToString();
                    assetRequestReject.AssetTeam = row["AssetTeam"].ToString();
                    assetRequestReject.AssetType = row["AssetType"].ToString();
                    assetRequestReject.HWSWName = row["HWSWName"].ToString();
                    assetRequestReject.RequestDate = row["RequestDate"].ToString();                 
                }
                list.Add(assetRequestReject);
            }
            return list;
        }
    }
}