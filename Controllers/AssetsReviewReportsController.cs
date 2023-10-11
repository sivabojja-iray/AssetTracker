using I_RAY_ASSET_TRACKER_MVC.Models;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mail;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;
using MailMessage = System.Net.Mail.MailMessage;
using MailPriority = System.Net.Mail.MailPriority;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetsReviewReportsController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        List<AssetReviewReportList> list;
        public string EmpMail;
        List<AssetTrackNowFromTeam> Teamlist;
        // GET: AssetsReviewReports
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
        public ActionResult AssetsReviewReportsTeamList(string EmployeeTeam)
        {       
            if (EmployeeTeam == "FSW")
            {
                list = assetReviewReportList("spProtblauditFSW");
            }
            else if(EmployeeTeam == "SV")
            {
                list = assetReviewReportList("spProtblauditSV");
            }
            var viewModel = new AssetsReviewReportsModel
            {
                assetReviewReportLists = list
            };
            return PartialView("AssetsReviewReportsTeamList", viewModel);
        }
        private List<AssetReviewReportList> assetReviewReportList(string query)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            list = new List<AssetReviewReportList>();
            foreach (DataRow row in dt.Rows)
            {
                AssetReviewReportList assetReviewReportList = new AssetReviewReportList();
                {
                    assetReviewReportList.AssignID = row["AssignID"].ToString();
                    assetReviewReportList.EmpID = row["EmpID"].ToString();
                    assetReviewReportList.EmployeeName = row["EmployeeName"].ToString();
                    assetReviewReportList.Team = row["Team"].ToString();
                    assetReviewReportList.AssetType = row["AssetType"].ToString();
                    assetReviewReportList.HWSWName = row["HWSWName"].ToString();
                    assetReviewReportList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    assetReviewReportList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    assetReviewReportList.AssetImage = row["AssetImage"].ToString();
                    assetReviewReportList.ReviewBy = row["ReviewBy"].ToString();
                    assetReviewReportList.ReviewDate = row["ReviewDate"].ToString();
                    assetReviewReportList.Remarks = row["Remarks"].ToString();
                    //assetReviewReportList.ReportToQMS = row["ReportToQMS"].ToString();
                }
                list.Add(assetReviewReportList);
            }
            return list;
        }
        public ActionResult ReportToQMS(string AssignID,string EmpID,string EmployeeName,string Team,string AssetType,string HWSWName,string SerialNumber,string Assetbelongsto,string ReviewBy,string ReviewDate,string Remarks)
        {
            string qms = "0";
            string sContent = string.Empty;
            sContent = "<table border='1'>" +
                "<tr><td>Employee ID</td><td>Employee Name</td><td>Employee Team</td>" +
                "<td>Asset Type</td><td>H/W S/W Name</td><td>SerialNumber</td><td>Asset belongs to ?</td>" +
                "<td>Reviewed By</td><td>Reviewed Date</td><td>Remarks</td><tr>";
            string UnUsed = "0";
            sContent += "<tr><td>" + EmpID + "</td><td>" + EmployeeName + "</td>" +
                              "<td>" + Team + "</td>" +
                              "<td>" + AssetType + "</td>" +
                              "<td>" + HWSWName + "</td>" +
                              "<td>" + SerialNumber + "</td>" +
                              "<td>" + Assetbelongsto + "</td>" +
                              "<td>" + ReviewBy + "</td>" +
                              "<td>" + ReviewDate + "</td>" +
                              "<td>" + Remarks + "</td><tr>";
            UnUsed = "1";
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string squery = "Update SaveAssetPermanent SET UnUsed = '" + UnUsed + "' where SerialNumberVersionNumber='" + SerialNumber + "'";
            SqlCommand cmdr = new SqlCommand(squery, con);
            cmdr.CommandType = CommandType.Text;
            int p = cmdr.ExecuteNonQuery();
            con.Close();
            con.Open();
            string squery1 = "Update SaveAsset SET UnUsed = '" + UnUsed + "'  where SerialNumberVersionNumber='" + SerialNumber + "'";
            SqlCommand cmdr1 = new SqlCommand(squery1, con);
            cmdr1.CommandType = CommandType.Text;
            int j = cmdr1.ExecuteNonQuery();
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand();
            string sql = string.Format("update tblAuditdocs2 set ReportToQMS='" + qms + "' where AssignID='" + AssignID + "'", con);
            cmd.Connection = con;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            con.Close();
            SqlCommand cmd5 = new SqlCommand("select Mail from EmployeeList where EmpID='" + EmpID + "'", con);
            con.Open();
            SqlDataReader dr2 = cmd5.ExecuteReader();
            if (dr2.Read())
            {
                EmpMail = dr2["Mail"].ToString();
                Session["EmpMail"] = EmpMail;
            }
            con.Close();
            con.Open();
            SqlCommand cmd1 = new SqlCommand("select AssignID,EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,Assetbelongsto,ReviewBy,ReviewDate,Remarks from tblAuditDocs2 WHERE AssignID = '" + AssignID + "'", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("assettracker@i-raysolutions.com");
                    mailMessage.To.Add(new MailAddress("raghavendra.saini@i-raysolutions.com"));
                    mailMessage.CC.Add(new MailAddress(Session["EmpMail"].ToString()));
                    mailMessage.Subject = "Report to QMS";
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = "Hi,This is related to Asset Review Information.<br/><br/>" + sContent + "</table>";                 
                    mailMessage.Priority = MailPriority.High;
                    SmtpClient SmtpMail = new SmtpClient();
                    SmtpMail.Send(mailMessage);
                    //Response.Redirect("~/AuditImages.aspx?SNo=" + AssignID);
                }
            }
            return View();
        }
        public ActionResult PartialAssetReviewView()
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
            return PartialView("PartialAssetReviewView");
        }
        public ActionResult AssetTrackNowFromTeam(string Team)
        {
            Teamlist = assetTrackNowFromTeamList("sp_AuditReportsByTeam", Team);
            var viewModel = new AssetsReviewReportsModel
            {
                assetTrackNowFromTeam = Teamlist
            };
            return PartialView("AssetTrackNowFromTeam",viewModel);
        }
        private List<AssetTrackNowFromTeam> assetTrackNowFromTeamList(string query,string selectedvalue)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Assetbelongsto", selectedvalue);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            Teamlist = new List<AssetTrackNowFromTeam>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTrackNowFromTeam assetTrackNowFromTeamList = new AssetTrackNowFromTeam();
                {
                    assetTrackNowFromTeamList.EmpID = row["EmpID"].ToString();
                    assetTrackNowFromTeamList.EmployeeName = row["EmployeeName"].ToString();
                    assetTrackNowFromTeamList.Team = row["Team"].ToString();               
                    assetTrackNowFromTeamList.AssetType = row["AssetType"].ToString();
                    assetTrackNowFromTeamList.HWSWName = row["HWSWName"].ToString();
                    assetTrackNowFromTeamList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    assetTrackNowFromTeamList.AssetImage = row["AssetImage"].ToString();
                    assetTrackNowFromTeamList.Assetbelongsto = row["Assetbelongsto"].ToString();                
                    assetTrackNowFromTeamList.ReviewBy = row["ReviewBy"].ToString();
                    assetTrackNowFromTeamList.ReviewDate = row["ReviewDate"].ToString();
                    assetTrackNowFromTeamList.Remarks = row["Remarks"].ToString();
                }
                Teamlist.Add(assetTrackNowFromTeamList);
            }
            return Teamlist;
        }
        public ActionResult AssetTrackNowFromEmployeeID(string EmployeeID)
        {
            Teamlist = assetTrackNowFromEmployeeIDList("sp_AuditReportsByEmpID", EmployeeID);
            var viewModel = new AssetsReviewReportsModel
            {
                assetTrackNowFromTeam = Teamlist
            };
            return PartialView("AssetTrackNowFromTeam",viewModel);
        }
        private List<AssetTrackNowFromTeam> assetTrackNowFromEmployeeIDList(string query, string selectedvalue)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", selectedvalue);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            Teamlist = new List<AssetTrackNowFromTeam>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTrackNowFromTeam assetTrackNowFromTeamList = new AssetTrackNowFromTeam();
                {
                    assetTrackNowFromTeamList.EmpID = row["EmpID"].ToString();
                    assetTrackNowFromTeamList.EmployeeName = row["EmployeeName"].ToString();
                    assetTrackNowFromTeamList.Team = row["Team"].ToString();
                    assetTrackNowFromTeamList.AssetType = row["AssetType"].ToString();
                    assetTrackNowFromTeamList.HWSWName = row["HWSWName"].ToString();
                    assetTrackNowFromTeamList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    assetTrackNowFromTeamList.AssetImage = row["AssetImage"].ToString();
                    assetTrackNowFromTeamList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    assetTrackNowFromTeamList.ReviewBy = row["ReviewBy"].ToString();
                    assetTrackNowFromTeamList.ReviewDate = row["ReviewDate"].ToString();
                    assetTrackNowFromTeamList.Remarks = row["Remarks"].ToString();
                }
                Teamlist.Add(assetTrackNowFromTeamList);
            }
            return Teamlist;
        }

        [Obsolete]
        public ActionResult TeamReviewReportPDF(AssetsReviewReportsModel assetsReviewReportsModel)
        {
            Teamlist = assetTrackNowFromTeamList("sp_AuditReportsByTeam", assetsReviewReportsModel.AssetBelongsto);
            var data = Teamlist;
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    gridview.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                    gridview.RenderControl(hw);

                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();

                    string imagePath = Server.MapPath("~/Images/Icon/I-Ray-Logo.png");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
                    image.ScaleToFit(200f, 170f);
                    image.SpacingBefore = 10f;
                    image.SpacingAfter = 1f;
                    image.Alignment = Element.ALIGN_LEFT;

                    var docTitle = new Paragraph();
                    var titleFont = FontFactory.GetFont("TIMES_ROMAN", 20, BaseColor.RED);
                    docTitle.Font = titleFont;
                    docTitle.Add(("Allocation list Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
                    docTitle.SpacingBefore = 150f;
                    docTitle.SpacingAfter = 1f;
                    docTitle.Alignment = Element.ALIGN_RIGHT;
                    pdfDoc.Add(docTitle);
                    pdfDoc.Add(image);

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;" + "filename=Reports.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
            return View();
        }

        [Obsolete]
        public ActionResult EmployeeIDReviewReportPDF(AssetsReviewReportsModel assetsReviewReportsModel)
        {
            Teamlist = assetTrackNowFromEmployeeIDList("sp_AuditReportsByEmpID", assetsReviewReportsModel.EmployeeID);
            var data = Teamlist;
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    gridview.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                    gridview.RenderControl(hw);

                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();

                    string imagePath = Server.MapPath("~/Images/Icon/I-Ray-Logo.png");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagePath);
                    image.ScaleToFit(200f, 170f);
                    image.SpacingBefore = 10f;
                    image.SpacingAfter = 1f;
                    image.Alignment = Element.ALIGN_LEFT;

                    var docTitle = new Paragraph();
                    var titleFont = FontFactory.GetFont("TIMES_ROMAN", 20, BaseColor.RED);
                    docTitle.Font = titleFont;
                    docTitle.Add(("Allocation list Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
                    docTitle.SpacingBefore = 150f;
                    docTitle.SpacingAfter = 1f;
                    docTitle.Alignment = Element.ALIGN_RIGHT;
                    pdfDoc.Add(docTitle);
                    pdfDoc.Add(image);

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;" + "filename=Reports.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
            return View();
        }
    }
}