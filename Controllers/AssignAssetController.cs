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
using System.Xml.Linq;
using System.Web.Mail;
using System.Net.Mail;
using MailMessage = System.Net.Mail.MailMessage;
using MailPriority = System.Net.Mail.MailPriority;
using Org.BouncyCastle.Ocsp;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssignAssetController : Controller
    {
        string EmpMail;
        // GET: AssignAsset
        public ActionResult Index(int? page)
        {
            //int pageSize = 10;
            //int pageIndex = 1;
            //pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //IPagedList<AssignAssetApproval> assignAssetApprovals = null;

            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand("spProAllAssetRequest1");
            cmd.Connection = sqlConnection;
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<AssignAsset> dataModels= new List<AssignAsset>();
            foreach (DataRow row in dt.Rows)
            {
                AssignAsset approval = new AssignAsset();
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
            var viewModal = new AssignAssetApproval
            {
                AssignAssets = dataModels
            };

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

            System.Threading.Thread.Sleep(1000);
            return View(viewModal);
        }

        public ActionResult ApprovalAssignAsset(int? id, AssignAsset assetApproval)
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

            SqlCommand cmd9 = new SqlCommand("select top 4 * from Assetrequest order by RequestID desc");
            cmd9.Connection = sqlConnection;
            sqlConnection.Open();
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
            return View(assetApproval);
        }
        public ActionResult RejectAssignAsset(int? id,int EmpID, AssignAsset assetApproval)
        {          
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";

            SqlCommand cmd = new SqlCommand("select EmpID,EmpTeam,AssetType,HWSWName,AssetTeam,Quantity,Serialnumber from Assetrequest where RequestID=" + id, sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            assetApproval.EmployeeID = dt.Rows[0][0].ToString();
            assetApproval.EmployeeTeam = dt.Rows[0][1].ToString();
            assetApproval.AssetType = dt.Rows[0][2].ToString();
            assetApproval.HWSWName = dt.Rows[0][3].ToString();
            assetApproval.Assetbelongsto = dt.Rows[0][4].ToString();
            assetApproval.Quantity = dt.Rows[0][5].ToString();
            assetApproval.SerialNumber = dt.Rows[0][6].ToString();

            SqlCommand cmd1 = new SqlCommand("delete from AssetRequest where RequestID= '" + id + "'", sqlConnection);
            sqlConnection.Open();
            cmd1.ExecuteNonQuery();
            sqlConnection.Close();

            string squery = "Update PermanentAssetrequest SET Approveornot = '0' where RequestID = '" + id + "'";
            SqlCommand cmd5 = new SqlCommand(squery, sqlConnection);   
            cmd5.CommandType = CommandType.Text;
            sqlConnection.Open();
            cmd5.ExecuteNonQuery();
            sqlConnection.Close();

            SqlCommand command = new SqlCommand("select Mail from EmployeeList where EmpID='" + EmpID + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dr2 = command.ExecuteReader();
            if (dr2.Read())
            {
                EmpMail = dr2["Mail"].ToString();
            }
            TempData["RejectMessage"] = "Asset Request is Rejected...";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
            mailMessage.To.Add(new MailAddress(EmpMail));
            mailMessage.Subject = "AssetRequest is not Approved";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = "Asset Request is Rejected";
            mailMessage.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th bgcolor='#ffc107'> EmpID </th>" + "<th bgcolor='#ffc107'> EmployeeName </th>" + "<th bgcolor='#ffc107'> Emp Team </th>" + "<th bgcolor='#ffc107'> Request Raised to ? </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'> HWSWName </th>" + "<th bgcolor='#ffc107'> RequestDate </th>" + "<th bgcolor='#ffc107'> Quantity </th>" + "<th bgcolor='#ffc107'> SerialNumber </th>" + "</tr>" +
                "<tr>" + "<td>" + assetApproval.EmployeeID + "</td>" + "<td>" + assetApproval.EmployeeName + "</td>" + assetApproval.EmployeeTeam + "</td>" + "</td>" + assetApproval.Assetbelongsto + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + assetApproval.AssetType + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + assetApproval.HWSWName + "</td>" + "<td>" + assetApproval.ReturnDate + "<td>" + assetApproval.Quantity + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + assetApproval.SerialNumber + "</td>" + "</td>" + "</tr>" + "</table>";
            mailMessage.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mailMessage);
            return RedirectToAction("Index");
        }

        /* This action is to send all values from form to database */

        public ActionResult SaveAssignAsset(AssignAsset assetApproval)
        {
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

            if (assetApproval.AssetType == "Receivers" || assetApproval.AssetType == "Cables" || assetApproval.AssetType == "Strips" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Flex strips" || assetApproval.AssetType == "Adaptors" || assetApproval.AssetType == "Inserts" || assetApproval.AssetType == "Battery")
            {
                SqlCommand selectCmd = new SqlCommand("select * from AssignAsset  where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
                sqlConnection.Open();
                int count = Convert.ToInt32(selectCmd.ExecuteScalar());
                if (count > 0)
                {
                    String Quantity = null;
                    SqlCommand Quantity_cmd = new SqlCommand("select SerialNumberVersionNumber from AssignAsset  where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
                    SqlDataReader rdr = Quantity_cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Quantity = rdr[0].ToString();
                    }
                    rdr.Close();
                    string newQuantity = assetApproval.Quantity;
                    int AddingQuantity = Convert.ToInt32(Quantity) + Convert.ToInt32(newQuantity);
                    string UpdatedQuantity = AddingQuantity.ToString();
                    String sql = "";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    sql = "Update AssignAsset set SerialNumberVersionNumber='" + UpdatedQuantity + "' where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'";
                    SqlCommand UpdateCommand = new SqlCommand(sql, sqlConnection);
                    adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
                    UpdateCommand.ExecuteNonQuery();
                    SqlDataAdapter adapter1 = new SqlDataAdapter();
                    SqlCommand UpdateAssignAsset1 = new SqlCommand("Update AssignAsset1 set SerialNumberVersionNumber='" + UpdatedQuantity + "' where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
                    adapter1.InsertCommand = new SqlCommand("Update AssignAsset1 set SerialNumberVersionNumber='" + UpdatedQuantity + "' where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
                    UpdateAssignAsset1.ExecuteNonQuery();
                    SqlDataAdapter adapter2 = new SqlDataAdapter();
                    SqlCommand UpdatetblAuditDocs = new SqlCommand("Update tblAuditDocs2 set SerialNumberVersionNumber='" + UpdatedQuantity + "' where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
                    adapter2.InsertCommand = new SqlCommand("Update tblAuditDocs2 set SerialNumberVersionNumber='" + UpdatedQuantity + "' where HWSWName ='" + assetApproval.HWSWName + "' and AssetType='" + assetApproval.AssetType + "' and Assetbelongsto='" + assetApproval.Assetbelongsto + "' and EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
                    UpdatetblAuditDocs.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("ProAssignAsset", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
                    cmd.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
                    cmd.Parameters.AddWithValue("@Team", assetApproval.EmployeeTeam);
                    cmd.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
                    cmd.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
                    if (assetApproval.AssetType == "Receivers" || assetApproval.AssetType == "Cables" || assetApproval.AssetType == "Strips" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Flex strips" || assetApproval.AssetType == "Adaptors" || assetApproval.AssetType == "Inserts" || assetApproval.AssetType == "Battery")
                    {
                        cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.Quantity);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);
                    }
                    cmd.Parameters.AddWithValue("@Comments", assetApproval.Comments);
                    cmd.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
                    cmd.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
                    cmd.Parameters.AddWithValue("@ActualReturnDate", assetApproval.ReturnDate);
                    cmd.Parameters.AddWithValue("@Assetbelongsto", assetApproval.Assetbelongsto);
                    if (assetApproval.DailyAllocation == true)
                    {
                        cmd.Parameters.AddWithValue("@Allocationtype", "Daily Allocation");
                    }
                    else if (assetApproval.DailyAllocation == false)
                    {
                        cmd.Parameters.AddWithValue("@Allocationtype", "Permanent Allocation");
                    }
                    cmd.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);                 
                    sqlConnection.Open();
                    int i = cmd.ExecuteNonQuery();
                    sqlConnection.Close();

                    SqlCommand cmd3 = new SqlCommand("ProAssignAsset1", sqlConnection);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
                    cmd3.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
                    cmd3.Parameters.AddWithValue("@Team", assetApproval.EmployeeTeam);
                    cmd3.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
                    cmd3.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
                    if (assetApproval.AssetType == "Receivers" || assetApproval.AssetType == "Cables" || assetApproval.AssetType == "Strips" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Flex strips" || assetApproval.AssetType == "Adaptors" || assetApproval.AssetType == "Inserts" || assetApproval.AssetType == "Battery")
                    {
                        cmd3.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.Quantity);
                    }
                    else
                    {
                        cmd3.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);
                    }
                    cmd3.Parameters.AddWithValue("@Comments", assetApproval.Comments);
                    cmd3.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
                    cmd3.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
                    cmd3.Parameters.AddWithValue("@ActualReturnDate", assetApproval.ReturnDate);
                    cmd3.Parameters.AddWithValue("@Assetbelongsto", assetApproval.AssignDate);
                    if (assetApproval.DailyAllocation == true)
                    {
                        cmd3.Parameters.AddWithValue("@Allocationtype", "Daily Allocation");
                    }
                    else if (assetApproval.DailyAllocation == false)
                    {
                        cmd3.Parameters.AddWithValue("@Allocationtype", "Permanent Allocation");
                    }
                    cmd3.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);                
                    sqlConnection.Open();
                    int j = cmd3.ExecuteNonQuery();
                    sqlConnection.Close();

                    SqlCommand cmdt = new SqlCommand("spAuditdata", sqlConnection);
                    cmdt.CommandType = CommandType.StoredProcedure;
                    cmdt.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
                    cmdt.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
                    cmdt.Parameters.AddWithValue("@Team", assetApproval.EmployeeName);
                    cmdt.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
                    cmdt.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
                    if (assetApproval.AssetType == "Receivers" || assetApproval.AssetType == "Cables" || assetApproval.AssetType == "Strips" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Receiver" || assetApproval.AssetType == "Flex strips" || assetApproval.AssetType == "Adaptors" || assetApproval.AssetType == "Inserts" || assetApproval.AssetType == "Battery")
                    {
                        cmdt.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.Quantity);
                    }
                    else
                    {
                        cmdt.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);
                    }
                    cmdt.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
                    cmdt.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
                    cmdt.Parameters.AddWithValue("@Assetbelongsto", assetApproval.ReturnDate);
                    cmdt.Parameters.AddWithValue("@AssetImage", assetApproval.Comments);
                    cmdt.Parameters.AddWithValue("@ReviewBy", assetApproval.Comments);
                    cmdt.Parameters.AddWithValue("@ReviewDate", assetApproval.Comments);
                    cmdt.Parameters.AddWithValue("@Remarks", assetApproval.Comments);
                    cmdt.Parameters.AddWithValue("@ReportToQMS", "");
                    cmdt.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);
                    sqlConnection.Open();
                    int h = cmdt.ExecuteNonQuery();
                    sqlConnection.Close();
                }
                sqlConnection.Close();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("ProAssignAsset", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
                cmd.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
                cmd.Parameters.AddWithValue("@Team", assetApproval.EmployeeTeam);
                cmd.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
                cmd.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
                cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);
                cmd.Parameters.AddWithValue("@Comments", assetApproval.Comments);
                cmd.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
                cmd.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
                cmd.Parameters.AddWithValue("@ActualReturnDate", assetApproval.ReturnDate);
                cmd.Parameters.AddWithValue("@Assetbelongsto", assetApproval.Assetbelongsto);
                if (assetApproval.DailyAllocation == true)
                {
                    cmd.Parameters.AddWithValue("@Allocationtype", "Daily Allocation");
                }
                else if (assetApproval.DailyAllocation == false)
                {
                    cmd.Parameters.AddWithValue("@Allocationtype", "Permanent Allocation");
                }
                cmd.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);
                sqlConnection.Open();
                int i = cmd.ExecuteNonQuery();
                sqlConnection.Close();

                SqlCommand cmd3 = new SqlCommand("ProAssignAsset1", sqlConnection);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
                cmd3.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
                cmd3.Parameters.AddWithValue("@Team", assetApproval.EmployeeTeam);
                cmd3.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
                cmd3.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
                cmd3.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);
                cmd3.Parameters.AddWithValue("@Comments", assetApproval.Comments);
                cmd3.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
                cmd3.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
                cmd3.Parameters.AddWithValue("@ActualReturnDate", assetApproval.ReturnDate);
                cmd3.Parameters.AddWithValue("@Assetbelongsto", assetApproval.Assetbelongsto);
                if (assetApproval.DailyAllocation == true)
                {
                    cmd3.Parameters.AddWithValue("@Allocationtype", "Daily Allocation");
                }
                else if (assetApproval.DailyAllocation == false)
                {
                    cmd3.Parameters.AddWithValue("@Allocationtype", "Permanent Allocation");
                }
                cmd3.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);
                sqlConnection.Open();
                int j = cmd3.ExecuteNonQuery();
                sqlConnection.Close();

                SqlCommand cmdt = new SqlCommand("spAuditdata", sqlConnection);
                cmdt.CommandType = CommandType.StoredProcedure;
                cmdt.Parameters.AddWithValue("@EmpID", assetApproval.EmployeeID);
                cmdt.Parameters.AddWithValue("@EmployeeName", assetApproval.EmployeeName);
                cmdt.Parameters.AddWithValue("@Team", assetApproval.EmployeeName);
                cmdt.Parameters.AddWithValue("@AssetType", assetApproval.AssetType);
                cmdt.Parameters.AddWithValue("@HWSWName", assetApproval.HWSWName);
                cmdt.Parameters.AddWithValue("@SerialNumberVersionNumber", assetApproval.SerialNumber);                
                cmdt.Parameters.AddWithValue("@AssignDate", assetApproval.AssignDate);
                cmdt.Parameters.AddWithValue("@ExpectedReturnDate", assetApproval.ReturnDate);
                cmdt.Parameters.AddWithValue("@Assetbelongsto", assetApproval.Assetbelongsto);
                cmdt.Parameters.AddWithValue("@AssetImage", assetApproval.Comments);
                cmdt.Parameters.AddWithValue("@ReviewBy", assetApproval.Comments);
                cmdt.Parameters.AddWithValue("@ReviewDate", assetApproval.Comments);
                cmdt.Parameters.AddWithValue("@Remarks", assetApproval.Comments);             
                if (assetApproval.DailyAllocation == true)
                {
                    cmdt.Parameters.AddWithValue("@ReportToQMS", "0");
                }
                else if (assetApproval.PermanentAllocation == true)
                {
                    cmdt.Parameters.AddWithValue("@ReportToQMS", "1");
                }
                cmdt.Parameters.AddWithValue("@InvoiceNo", assetApproval.invoicenumber);
                sqlConnection.Open();
                int h = cmdt.ExecuteNonQuery();
                sqlConnection.Close();
            }

            SqlCommand cmd4 = new SqlCommand("delete from AssetRequest where RequestID=" + Session["RequestID"], sqlConnection);
            sqlConnection.Open();
            cmd4.ExecuteNonQuery();
            sqlConnection.Close();
            SqlCommand cmd5 = new SqlCommand("select Mail from EmployeeList where EmpID='" + assetApproval.EmployeeID + "'", sqlConnection);
            sqlConnection.Open();
            SqlDataReader dr2 = cmd5.ExecuteReader();
            if (dr2.Read())
            {
                EmpMail = dr2["Mail"].ToString();             
            }
            sqlConnection.Close();

            TempData["Message"] = "Asset Assigned Successfully...";
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("assettracker@i-raysolutions.com");
            mail.To.Add(new MailAddress(EmpMail));
            mail.To.Add(new MailAddress("phani.n@i-raysolutions.com"));
            mail.Subject = "Asset Assigned Successfully";
            mail.IsBodyHtml = true;
            mail.Body = "<table style= 'border: 1 ; align='center' border-color: #6495ED width: 100%' border='5'>" + "<tr>" + "<th bgcolor='#ffc107'> EmpID </th>" + "<th bgcolor='#ffc107'> AssetType </th>" + "<th bgcolor='#ffc107'> AssetName </th>" + "<th bgcolor='#ffc107'> AssetSerialNo </th>" + "<th bgcolor='#ffc107'> AssignDate </th>" + "</tr>" + "<tr>" + "<td>" + assetApproval.EmployeeID + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + assetApproval.AssetType + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + assetApproval.HWSWName + "</td>" + "<td style='font-weight:bold;font-size:15px;'>" + assetApproval.SerialNumber + "</td>" + "<td>" + assetApproval.AssignDate + "</td>" + "</tr>" + "</table>";
            mail.Priority = MailPriority.High;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
            return RedirectToAction("Index");
        }
    }
}