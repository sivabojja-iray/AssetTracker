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

                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT COUNT(AssetType) FROM AssignAsset where EmpID='" + Session["username"] + "'");
                cmd.Connection = con;
                con.Open();
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                ViewBag.TotalAssetsUserHave = i;

                SqlCommand comnd = new SqlCommand("SELECT AssignID,EmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate FROM AssignAsset WHERE EmpID='" + Session["username"] + "'order by AssignID");
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

                SqlCommand command = new SqlCommand("SELECT FromEmployeeName,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssetTransferDate,FromEmpID,ToEmpID,TeamToWhichAssetisTransfered,ToEmployeeName FROM AssetTransfer where ToEmpID='" + Session["username"] + "'");
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

                var viewModal = new UserModel
                {
                    Table1Data=dataModels.ToPagedList(pageIndex, pageSize),
                    Table2Data=list.ToPagedList(pageIndex,pageSize)
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
        public ActionResult SaveTransferAsset(Table1Model userModel) 
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
        public ActionResult SaveTransferAssetFromEmployee(int? id,Table2Model table2Model)
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
        public ActionResult RejectTransferAssetFromEmployee(int? id, Table2Model table2Model)
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