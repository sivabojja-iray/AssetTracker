using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I_RAY_ASSET_TRACKER_MVC.Models;
using System.Drawing;
using PagedList;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssignAssetController : Controller
    {
        // GET: AssignAsset
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<AssignAssetApproval> assignAssetApprovals = null;

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("spProAllAssetRequest1");
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<AssignAssetApproval> dataModels= new List<AssignAssetApproval>();
            foreach (DataRow row in dt.Rows)
            {
                AssignAssetApproval approval = new AssignAssetApproval();
                {
                    approval.RequestID = row["RequestID"].ToString();
                    approval.EmployeeID = row["EmpID"].ToString();
                    approval.EmployeeName = row["EmployeeName"].ToString();
                    approval.EmployeeTeam = row["EmpTeam"].ToString();
                    approval.Assetbelongsto = row["AssetTeam"].ToString();
                    approval.AssetType = row["AssetType"].ToString();
                    approval.HWSWName = row["HWSWName"].ToString();
                    approval.AssignDate = row["RequestDate"].ToString();
                    approval.Quantity = row["Quantity"].ToString();
                    approval.SerialNumber = row["Serialnumber"].ToString();
                }
                dataModels.Add(approval);
            }
            assignAssetApprovals=dataModels.ToPagedList(pageIndex, pageSize);
            System.Threading.Thread.Sleep(1000);
            return View(assignAssetApprovals);
        }

        public ActionResult ApprovalAssignAsset(int? id, AssignAssetApproval assetApproval)
        {        
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("select EmpID,EmpTeam,AssetType,HWSWName,AssetTeam,Quantity,Serialnumber from Assetrequest where RequestID=" + id, sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            Session["RequestID"] = Convert.ToInt32(id);

            assetApproval.EmployeeID = dt.Rows[0][0].ToString();
            assetApproval.EmployeeTeam = dt.Rows[0][1].ToString();
            assetApproval.AssetType = dt.Rows[0][2].ToString();
            assetApproval.HWSWName = dt.Rows[0][3].ToString();
            assetApproval.Assetbelongsto = dt.Rows[0][4].ToString();
            assetApproval.Quantity = dt.Rows[0][5].ToString();
            assetApproval.SerialNumber = dt.Rows[0][6].ToString();

            assetApproval.AssignDate = DateTime.Now.ToString();

            SqlCommand cmdd1 = new SqlCommand("select EmployeeName from EmployeeList where EmpID=@EmpID");
            cmdd1.CommandType = CommandType.Text;
            cmdd1.Connection = sqlConnection;
            sqlConnection.Open();
            cmdd1.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
            SqlDataReader dr1 = cmdd1.ExecuteReader();
            if (dr1.Read())
            {
               assetApproval.EmployeeName = dr1["EmployeeName"].ToString();
            }
            sqlConnection.Close();
            System.Threading.Thread.Sleep(1000);
            return View(assetApproval);
        }
        public ActionResult RejectAssignAsset(int? id)
        {          
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("delete from AssetRequest where RequestID= '" + id + "'", sqlConnection);
            sqlConnection.Open();
            cmd.ExecuteNonQuery();
            sqlConnection.Close();

            string squery = "Update PermanentAssetrequest SET Approveornot = '0' where RequestID = '" + id + "'";
            SqlCommand cmd5 = new SqlCommand(squery, sqlConnection);   
            cmd5.CommandType = CommandType.Text;
            sqlConnection.Open();
            cmd5.ExecuteNonQuery();
            sqlConnection.Close();
            System.Threading.Thread.Sleep(1000);

            return RedirectToAction("Index");
        }

        /* This action is to send all values from form to database */

        public ActionResult SaveAssignAsset(AssignAssetApproval assetApproval)
        {
            System.Threading.Thread.Sleep(3000);

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";

            /* Get Invoice Number from SaveAssetInQuantity Table */
           
            if (assetApproval.AssetType== "Receivers" || assetApproval.AssetType == "Cables" || assetApproval.AssetType == "Strips" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Flex strips" || assetApproval.AssetType == "Adaptors" || assetApproval.AssetType == "Inserts" || assetApproval.AssetType == "Battery")
            {
                SqlCommand cmnd = new SqlCommand("select DISTINCT InvoiceNo from SaveAssetInQuantity where AssetType='" + assetApproval.AssetType + "' and HWSWName='" + assetApproval.HWSWName + "' and Team='" + assetApproval.EmployeeTeam + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader dr = cmnd.ExecuteReader();
                if(dr.Read())
                {
                    assetApproval.invoicenumber = dr["InvoiceNo"].ToString();
                }
                sqlConnection.Close() ;
            }
            else
            {
                SqlCommand cmnd = new SqlCommand("select DISTINCT InvoiceNo from SaveAsset where SerialNumberVersionNumber='" + assetApproval.SerialNumber + "' and HWSWName='" + assetApproval.HWSWName + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader dr = cmnd.ExecuteReader();
                if (dr.Read())
                {
                    assetApproval.invoicenumber = dr["InvoiceNo"].ToString();
                }
                sqlConnection.Close();
            }

            SqlCommand command = new SqlCommand("ProAssignAsset");
            sqlConnection.Open();
            command.Connection = sqlConnection;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
            command.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
            command.Parameters.AddWithValue("@Team", assetApproval.EmployeeTeam);
            command.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
            command.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
            command.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);
            command.Parameters.AddWithValue("@Comments", assetApproval.Comments);
            command.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
            command.Parameters.AddWithValue("@Assetbelongsto", assetApproval.Assetbelongsto);
            if (assetApproval.DailyAllocation == true)
            {
                command.Parameters.AddWithValue("@Allocationtype", "Daily Allocation");
            }
            else if(assetApproval.DailyAllocation == false)
            {
                command.Parameters.AddWithValue("@Allocationtype", "Permanent Allocation");
            }
            command.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
            command.Parameters.AddWithValue("@ActualReturnDate", assetApproval.ReturnDate);
            command.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);
            int i = command.ExecuteNonQuery();

            SqlCommand cmd = new SqlCommand("delete from AssetRequest  where RequestID=" + Session["RequestID"], sqlConnection);           
            cmd.ExecuteNonQuery();          

            sqlConnection.Close();
            if (i > 0)
            {
                return RedirectToAction("Index");
            }
            return View(assetApproval);
        }
    }
}