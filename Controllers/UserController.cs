using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class UserController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
       
        // GET: User
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT COUNT(AssetType) FROM AssignAsset  where EmpID=" + Session["username"] + "");
                cmd.Connection = con;
                con.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                ViewBag.TotalAssetsUserHave = i;
                List<UserModel> list = userModelList("SELECT AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + Session["username"] + "'");

                SqlCommand cmd1 = new SqlCommand("SELECT DISTINCT Team FROM EmployeeList WHERE Team NOT IN('Support','HR')", con);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd1);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                List<string> Teamlist = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                   Teamlist.Add(row["Team"].ToString());
                }
                SelectList Teams = new SelectList(Teamlist);
                ViewData["Teams"] = Teams;

                con.Close();
                return View(list);
            }
            else
            {
                return Redirect("/~UserLogin/Login");
            }
        }
        private List<UserModel> userModelList(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<UserModel> dataModels = new List<UserModel>();
            foreach (DataRow row in dt.Rows)
            {
                UserModel userList = new UserModel();
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
                return dataModels;
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
    }
}