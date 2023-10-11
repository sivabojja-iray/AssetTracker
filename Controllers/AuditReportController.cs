using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing;
using Org.BouncyCastle.Ocsp;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AuditReportController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        List<AuditReviewList> list;
        // GET: AuditReport
        public ActionResult Index()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
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
            return View();
        }
        public ActionResult AssetReviewFromEmployeeID(string EmployeeId) 
        {
            list = auditReviewReportList("ProAuditByEmpID", EmployeeId);
            var viewModel = new AuditModel
            {
                auditReviewLists = list
            };
            return PartialView("AssetReviewFromEmployeeID",viewModel);
        }
        private List<AuditReviewList> auditReviewReportList(string query, string value)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", value);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            list = new List<AuditReviewList>();
            foreach (DataRow row in dt.Rows)
            {
                AuditReviewList auditReviewList = new AuditReviewList();
                {
                    auditReviewList.AssignID = row["AssignID"].ToString();
                    auditReviewList.EmpID = row["EmpID"].ToString();
                    auditReviewList.EmployeeName = row["EmployeeName"].ToString();
                    auditReviewList.Team = row["Team"].ToString();
                    auditReviewList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    auditReviewList.AssetType = row["AssetType"].ToString();
                    auditReviewList.HWSWName = row["HWSWName"].ToString();
                    auditReviewList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();                
                    auditReviewList.AssignDate = row["AssignDate"].ToString();
                    auditReviewList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                    auditReviewList.AssetImage = row["AssetImage"].ToString();
                }
                list.Add(auditReviewList);
            }
            return list;
        }
        public ActionResult AssetReviewFromEmployeeName(string EmployeeName)
        {
            list = auditReviewReportEmployeeNameList("ProAuditByEmpName", EmployeeName);
            var viewModel = new AuditModel
            {
                auditReviewLists = list
            };
            return PartialView("AssetReviewFromEmployeeID",viewModel);
        }
        private List<AuditReviewList> auditReviewReportEmployeeNameList(string query, string value)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeName", value);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            list = new List<AuditReviewList>();
            foreach (DataRow row in dt.Rows)
            {
                AuditReviewList auditReviewList = new AuditReviewList();
                {
                    auditReviewList.AssignID = row["AssignID"].ToString();
                    auditReviewList.EmpID = row["EmpID"].ToString();
                    auditReviewList.EmployeeName = row["EmployeeName"].ToString();
                    auditReviewList.Team = row["Team"].ToString();
                    auditReviewList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    auditReviewList.AssetType = row["AssetType"].ToString();
                    auditReviewList.HWSWName = row["HWSWName"].ToString();
                    auditReviewList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    auditReviewList.AssignDate = row["AssignDate"].ToString();
                    auditReviewList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                    auditReviewList.AssetImage = row["AssetImage"].ToString();
                }
                list.Add(auditReviewList);
            }
            return list;
        }
        public ActionResult EditFileAuditReport(string AssignID, string EmpID, AuditReviewList auditReviewList)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select AssignID,EmpID,EmployeeName,Team,Assetbelongsto,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate,AssetImage from tblAuditDocs2 where EmpID ='" + EmpID + "'and AssignID ='" + AssignID + "'", sqlConnection);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            List<AuditReviewList> dataModels = new List<AuditReviewList>();
            foreach (DataRow row in dt.Rows)
            {
                auditReviewList.AssignID = row["AssignID"].ToString();
                auditReviewList.EmpID = row["EmpID"].ToString();
                auditReviewList.EmployeeName = row["EmployeeName"].ToString();
                auditReviewList.Team = row["Team"].ToString();
                auditReviewList.Assetbelongsto = row["Assetbelongsto"].ToString();
                auditReviewList.AssetType = row["AssetType"].ToString();
                auditReviewList.HWSWName = row["HWSWName"].ToString();
                auditReviewList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                auditReviewList.AssignDate = row["AssignDate"].ToString();
                auditReviewList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                auditReviewList.AssetImage = row["AssetImage"].ToString();
            }
            dataModels.Add(auditReviewList);
            return PartialView("EditFileAuditReport", auditReviewList);
        }
        [HttpPost]
        public ActionResult UploadFileAuditReport(HttpPostedFileBase Photo, AuditReviewList auditReviewList)
        {
            if (Photo.ContentLength > 0)
            {
                SqlConnection sqlConnection = new SqlConnection(constr);
                string currentime = DateTime.Now.ToString("dd-MM-yyyy");
                string qms = "0";
                string _FileName=Path.GetFileName(Photo.FileName);
                string extension=Path.GetExtension(Photo.FileName);
                if (extension.ToLower() == ".png" || extension.ToLower() == ".jpg")
                {
                    Stream strm = Photo.InputStream;
                    using (var image = System.Drawing.Image.FromStream(strm))
                    {
                        int newWidth = 240; // New Width of Image in Pixel  
                        int newHeight = 240; // New Height of Image in Pixel  
                        var thumbImg = new Bitmap(newWidth, newHeight);
                        var thumbGraph = Graphics.FromImage(thumbImg);
                        thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                        thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                        thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        var imgRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
                        thumbGraph.DrawImage(image, imgRectangle);
                        string targetPath = Server.MapPath(@"~\Images\") + Photo.FileName;
                        thumbImg.Save(targetPath, image.RawFormat);
                        string filename = Path.GetFileName(Photo.FileName);
                        string path = "/Images/" + filename;
                        Photo.SaveAs(Server.MapPath(path));
                     
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand();
                        string sql = string.Format("update tblAuditdocs2 set AssetImage = '" + path + "',ReviewBy='" + auditReviewList.ReviewedBy + "' ,ReviewDate='" + currentime + "',ReportToQMS='" + qms + "',Remarks='" + auditReviewList.Remarks + "' where AssignID='" + auditReviewList.AssignID + "'", sqlConnection);
                        cmd.Connection = sqlConnection;
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}