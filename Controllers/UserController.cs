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
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class UserController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
       
        // GET: User
        public ActionResult Index(int? page)
        {
            if (Session["username"] != null)
            {
                int pageSize = 5;
                int pageIndex = 1;
                pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
                IPagedList<UserModel> userModels = null;

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT COUNT(AssetType) FROM AssignAsset  where EmpID=" + Session["username"] + "");
                cmd.Connection = con;
                con.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                ViewBag.TotalAssetsUserHave = i;

                SqlCommand comnd = new SqlCommand("SELECT AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + Session["username"] + "'order by AssignID");
                comnd.Connection = con;
                SqlDataAdapter adp = new SqlDataAdapter(comnd);
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
                userModels= dataModels.ToPagedList(pageIndex, pageSize);
                con.Close();
                return View(userModels);
            }
            else
            {
                return Redirect("/~UserLogin/Login");
            }
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
        public ActionResult TransferAsset(int? Id, UserModel userList)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);        
            SqlCommand cmd = new SqlCommand("select AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate from AssignAsset where AssignID=" + Id, sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<UserModel> dataModels = new List<UserModel>();
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
             
                dataModels.Add(userList);
            }

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
        public ActionResult SaveTransferAsset(UserModel userModel) 
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
            return RedirectToAction("Index"); 
        }
    }
}