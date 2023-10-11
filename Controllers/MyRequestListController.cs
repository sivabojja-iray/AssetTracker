using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class MyRequestListController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: MyRequestList
        public ActionResult Index()
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("spProEmpAssetRequest", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", Session["username"]);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<MyRequestListModel> list = new List<MyRequestListModel>();
            foreach (DataRow row in dt.Rows)
            {
                MyRequestListModel myRequestListModel = new MyRequestListModel();
                {
                    myRequestListModel.RequestID = row["RequestID"].ToString();
                    myRequestListModel.EmpID = row["EmpID"].ToString();
                    myRequestListModel.EmpTeam = row["EmpTeam"].ToString();
                    myRequestListModel.AssetTeam = row["AssetTeam"].ToString();
                    myRequestListModel.AssetType = row["AssetType"].ToString();
                    myRequestListModel.HWSWName = row["HWSWName"].ToString();
                    myRequestListModel.RequestDate = row["RequestDate"].ToString();
                    myRequestListModel.Quantity = row["Quantity"].ToString();
                }
                list.Add(myRequestListModel);
            }

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

            return PartialView("Index", list);
        }
    }
}