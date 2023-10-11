using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I_RAY_ASSET_TRACKER_MVC.Models;
using System.Collections;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class ReportsController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: Reports
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
        public ActionResult SeachEmployeeIDTrackNow(string EmployeeID)
        {
            List<SearchData> list = SearchEmployeeIDList("ProTrackingByEmpID", EmployeeID);
            var viewModel = new ReportsModel
            {
                searchData= list
            };
            return PartialView("SeachEmployeeIDTrackNow", viewModel);
        }

        private List<SearchData> SearchEmployeeIDList(string query,string EmployeeID)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", EmployeeID);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        public ActionResult SeachSerialNumberTrackNow(string SerialNumber)
        {
            List<SearchData> list = SearchSerialNumberList("ProTrackingByAssetSno", SerialNumber);
            var viewModel = new ReportsModel
            {
                searchData = list
            };
            return PartialView("SeachSerialNumberTrackNow", viewModel);
        }
        private List<SearchData> SearchSerialNumberList(string query,string SerialNumber)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", SerialNumber);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        public ActionResult SearchEmployeeNameTrackNow(string EmployeeName)
        {
            List<SearchData> list = EmployeeNameList("ProTrackingByEmployeeName", EmployeeName);
            var viewModel = new ReportsModel
            {
                searchData = list
            };
            return PartialView("SearchEmployeeNameTrackNow", viewModel);
        }
        private List<SearchData> EmployeeNameList(string query, string EmployeeName)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeName", EmployeeName);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        public ActionResult SearchAssetTypeTrackNow(string AssetType)
        {
            List<SearchData> list = AssetTypeList("ProTrackingByAssetType", AssetType);
            var viewModel = new ReportsModel
            {
                searchData = list
            };
            return PartialView("SearchAssetTypeTrackNow",viewModel);
        }
        private List<SearchData> AssetTypeList(string query, string AssetType)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssetType", AssetType);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        public ActionResult SearchHWSWNameTrackNow(string HWSWName)
        {
            List<SearchData> list = HWSWNameList("ProTrackingByHSName", HWSWName);
            var viewModel = new ReportsModel
            {
                searchData = list
            };
            return PartialView("SearchHWSWNameTrackNow", viewModel);
        }
        private List<SearchData> HWSWNameList(string query, string HWSWName)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HWSWName", HWSWName);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        [HttpPost]
        [Obsolete]
        public ActionResult EmployeeIDPDFReport(FormCollection formCollection,ReportsModel reportsModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "HistoricalDataPDF":
                    List<SearchData> list = HistoricalEmployeeIDList("ProEmpwiseReports", reportsModel.EmployeeID);
                    var data = list;
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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "AssetTransferRecordPDF":
                    List<AssetTransferRecords> records = AssetTransferRecordEmployeeIDList("proEmpwiseAssetTransferRecords", reportsModel.EmployeeID);
                    var recordData = records;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview1 = new GridView();
                            gridview1.DataSource = recordData;
                            gridview1.DataBind();
                            gridview1.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview1.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "PDFReport":
                    List<SearchData> employeeIDList = SearchEmployeeIDList("ProTrackingByEmpID", reportsModel.EmployeeID);
                    var employeeIDListData = employeeIDList;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview2 = new GridView();
                            gridview2.DataSource = employeeIDListData;
                            gridview2.DataBind();
                            gridview2.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview2.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "ExcelReport":
                    List<SearchData> employeeIDListExcle = SearchEmployeeIDList("ProTrackingByEmpID", reportsModel.EmployeeID);
                    var employeeIDListExcleData = employeeIDListExcle;
                    GridView gridview3 = new GridView();
                    gridview3.DataSource = employeeIDListExcleData;
                    gridview3.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    //string FileName = "Reports" + DateTime.Now + ".xlsx";
                    string FileName = "Reports.xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    gridview3.GridLines = GridLines.Both;
                    gridview3.HeaderStyle.Font.Bold = true;
                    gridview3.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        private List<SearchData> HistoricalEmployeeIDList(string query, string EmployeeID)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmpID", EmployeeID);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                    reportsModel.ActualReturnDate = row["ActualReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        private List<AssetTransferRecords> AssetTransferRecordEmployeeIDList(string query, string EmployeeID)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromEmpID", EmployeeID);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();    
            List<AssetTransferRecords> list = new List<AssetTransferRecords>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTransferRecords reportsModel = new AssetTransferRecords();
                {
                    reportsModel.FromEmpID = row["FromEmpID"].ToString();
                    reportsModel.FromEmployeeName = row["FromEmployeeName"].ToString();
                    reportsModel.Assetbelongsto = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssetTransferDate = row["AssetTransferDate"].ToString();
                    reportsModel.ToEmpID = row["ToEmpID"].ToString();
                    reportsModel.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    reportsModel.ToEmployeeName = row["ToEmployeeName"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        [HttpPost]
        [Obsolete]
        public ActionResult AssetSerialNumberPDFReport(FormCollection formCollection, ReportsModel reportsModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "HistoricalDataPDF":
                    List<SearchData> list = HistoricalAssetSerialNoList("ProAssetReports", reportsModel.AssetSerialNumber);
                    var data = list;
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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "AssetTransferRecordPDF":
                    List<AssetTransferRecords> records = AssetTransferRecordSerialNoList("ProEmpSerialNumberTR", reportsModel.AssetSerialNumber);
                    var recordData = records;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview1 = new GridView();
                            gridview1.DataSource = recordData;
                            gridview1.DataBind();
                            gridview1.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview1.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "PDFReport":
                    List<SearchData> SerialNoList = SearchSerialNumberList("ProTrackingByAssetSno", reportsModel.AssetSerialNumber);
                    var SerialNoListData = SerialNoList;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview2 = new GridView();
                            gridview2.DataSource = SerialNoListData;
                            gridview2.DataBind();
                            gridview2.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview2.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "ExcelReport":
                    List<SearchData> serialNoListExcle = SearchSerialNumberList("ProTrackingByAssetSno", reportsModel.AssetSerialNumber);
                    var serialNoListExcleData = serialNoListExcle;
                    GridView gridview3 = new GridView();
                    gridview3.DataSource = serialNoListExcleData;
                    gridview3.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    //string FileName = "Reports" + DateTime.Now + ".xlsx";
                    string FileName = "Reports.xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    gridview3.GridLines = GridLines.Both;
                    gridview3.HeaderStyle.Font.Bold = true;
                    gridview3.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        private List<SearchData> HistoricalAssetSerialNoList(string query,string assetSerialNo)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", assetSerialNo);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                    reportsModel.ActualReturnDate = row["ActualReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        private List<AssetTransferRecords> AssetTransferRecordSerialNoList(string query,string serialNo)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SerialNumberVersionNumber", serialNo);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<AssetTransferRecords> list = new List<AssetTransferRecords>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTransferRecords reportsModel = new AssetTransferRecords();
                {
                    reportsModel.FromEmpID = row["FromEmpID"].ToString();
                    reportsModel.FromEmployeeName = row["FromEmployeeName"].ToString();
                    reportsModel.Assetbelongsto = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssetTransferDate = row["AssetTransferDate"].ToString();
                    reportsModel.ToEmpID = row["ToEmpID"].ToString();
                    reportsModel.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    reportsModel.ToEmployeeName = row["ToEmployeeName"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        [HttpPost]
        [Obsolete]
        public ActionResult EmployeeNamePDFReport(FormCollection formCollection, ReportsModel reportsModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "HistoricalDataPDF":
                    List<SearchData> list = HistoricalEmployeeNameList("ProEmpNameReports", reportsModel.EmployeeName);
                    var data = list;
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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "AssetTransferRecordPDF":
                    List<AssetTransferRecords> records = AssetTransferRecordEmployeeNameList("ProEmpNameTransferRecords", reportsModel.EmployeeName);
                    var recordData = records;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview1 = new GridView();
                            gridview1.DataSource = recordData;
                            gridview1.DataBind();
                            gridview1.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview1.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "PDFReport":
                    List<SearchData> employeeNameList = EmployeeNameList("ProTrackingByEmployeeName", reportsModel.EmployeeName);
                    var employeeNameListData = employeeNameList;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview2 = new GridView();
                            gridview2.DataSource = employeeNameListData;
                            gridview2.DataBind();
                            gridview2.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview2.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "ExcelReport":
                    List<SearchData> employeeNameListExcle = EmployeeNameList("ProTrackingByEmployeeName", reportsModel.EmployeeName);
                    var employeeNameListExcleData = employeeNameListExcle;
                    GridView gridview3 = new GridView();
                    gridview3.DataSource = employeeNameListExcleData;
                    gridview3.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    //string FileName = "Reports" + DateTime.Now + ".xlsx";
                    string FileName = "Reports.xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    gridview3.GridLines = GridLines.Both;
                    gridview3.HeaderStyle.Font.Bold = true;
                    gridview3.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        private List<SearchData> HistoricalEmployeeNameList(string query, string employeeName)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeName", employeeName);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                    reportsModel.ActualReturnDate = row["ActualReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        private List<AssetTransferRecords> AssetTransferRecordEmployeeNameList(string query, string employeeName)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromEmployeeName", employeeName);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<AssetTransferRecords> list = new List<AssetTransferRecords>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTransferRecords reportsModel = new AssetTransferRecords();
                {
                    reportsModel.FromEmpID = row["FromEmpID"].ToString();
                    reportsModel.FromEmployeeName = row["FromEmployeeName"].ToString();
                    reportsModel.Assetbelongsto = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssetTransferDate = row["AssetTransferDate"].ToString();
                    reportsModel.ToEmpID = row["ToEmpID"].ToString();
                    reportsModel.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    reportsModel.ToEmployeeName = row["ToEmployeeName"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        [HttpPost]
        [Obsolete]
        public ActionResult AssetTypePDFReport(FormCollection formCollection, ReportsModel reportsModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "HistoricalDataPDF":
                    List<SearchData> list = HistoricalAssetTypeList("ProAssetTypeReports", reportsModel.AssetType);
                    var data = list;
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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "AssetTransferRecordPDF":
                    List<AssetTransferRecords> records = AssetTransferRecordAssetTypeList("ProAssetTypeTransferRecords", reportsModel.AssetType);
                    var recordData = records;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview1 = new GridView();
                            gridview1.DataSource = recordData;
                            gridview1.DataBind();
                            gridview1.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview1.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "PDFReport":
                    List<SearchData> assetTypeList = AssetTypeList("ProTrackingByAssetType", reportsModel.AssetType);
                    var assetTypeListData = assetTypeList;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview2 = new GridView();
                            gridview2.DataSource = assetTypeListData;
                            gridview2.DataBind();
                            gridview2.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview2.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "ExcelReport":
                    List<SearchData> assetTypeListExcle = AssetTypeList("ProTrackingByAssetType", reportsModel.AssetType);
                    var assetTypeListExcleData = assetTypeListExcle;
                    GridView gridview3 = new GridView();
                    gridview3.DataSource = assetTypeListExcleData;
                    gridview3.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    //string FileName = "Reports" + DateTime.Now + ".xlsx";
                    string FileName = "Reports.xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    gridview3.GridLines = GridLines.Both;
                    gridview3.HeaderStyle.Font.Bold = true;
                    gridview3.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        private List<SearchData> HistoricalAssetTypeList(string query, string assetType)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssetType", assetType);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                    reportsModel.ActualReturnDate = row["ActualReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        private List<AssetTransferRecords> AssetTransferRecordAssetTypeList(string query, string assetType)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AssetType", assetType);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<AssetTransferRecords> list = new List<AssetTransferRecords>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTransferRecords reportsModel = new AssetTransferRecords();
                {
                    reportsModel.FromEmpID = row["FromEmpID"].ToString();
                    reportsModel.FromEmployeeName = row["FromEmployeeName"].ToString();
                    reportsModel.Assetbelongsto = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssetTransferDate = row["AssetTransferDate"].ToString();
                    reportsModel.ToEmpID = row["ToEmpID"].ToString();
                    reportsModel.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    reportsModel.ToEmployeeName = row["ToEmployeeName"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        [HttpPost]
        [Obsolete]
        public ActionResult HWSWNamePDFReport(FormCollection formCollection, ReportsModel reportsModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "HistoricalDataPDF":
                    List<SearchData> list = HistoricalHWSWNameList("ProHWSWNameReports", reportsModel.HWSWName);
                    var data = list;
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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "AssetTransferRecordPDF":
                    List<AssetTransferRecords> records = AssetTransferRecordHWSWNameList("ProHWSWNameTransferRecords", reportsModel.HWSWName);
                    var recordData = records;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview1 = new GridView();
                            gridview1.DataSource = recordData;
                            gridview1.DataBind();
                            gridview1.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview1.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "PDFReport":
                    List<SearchData> hWSWNameList = HWSWNameList("ProTrackingByHSName", reportsModel.HWSWName);
                    var hWSWNameListData = hWSWNameList;
                    using (StringWriter sw = new StringWriter())
                    {
                        using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                        {
                            GridView gridview2 = new GridView();
                            gridview2.DataSource = hWSWNameListData;
                            gridview2.DataBind();
                            gridview2.HeaderStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E8B72A");
                            gridview2.RenderControl(hw);

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
                            docTitle.Add(("Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
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
                    return RedirectToAction("Index");
                case "ExcelReport":
                    List<SearchData> hWSWNameListExcle = HWSWNameList("ProTrackingByHSName", reportsModel.HWSWName);
                    var hWSWNameListExcleData = hWSWNameListExcle;
                    GridView gridview3 = new GridView();
                    gridview3.DataSource = hWSWNameListExcleData;
                    gridview3.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    //string FileName = "Reports" + DateTime.Now + ".xlsx";
                    string FileName = "Reports.xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    gridview3.GridLines = GridLines.Both;
                    gridview3.HeaderStyle.Font.Bold = true;
                    gridview3.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        private List<SearchData> HistoricalHWSWNameList(string query, string HWSWName)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HWSWName", HWSWName);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<SearchData> list = new List<SearchData>();
            foreach (DataRow row in dt.Rows)
            {
                SearchData reportsModel = new SearchData();
                {
                    reportsModel.AssignID = row["AssignID"].ToString();
                    reportsModel.EmployeeID = row["EmpID"].ToString();
                    reportsModel.EmployeeName = row["EmployeeName"].ToString();
                    reportsModel.Team = row["Team"].ToString();
                    reportsModel.AssetBelongsTo = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssignDate = row["AssignDate"].ToString();
                    reportsModel.ExpectedDate = row["ExpectedReturnDate"].ToString();
                    reportsModel.ActualReturnDate = row["ActualReturnDate"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
        private List<AssetTransferRecords> AssetTransferRecordHWSWNameList(string query, string HWSWName)
        {

            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@HWSWName", HWSWName);
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            con.Close();
            List<AssetTransferRecords> list = new List<AssetTransferRecords>();
            foreach (DataRow row in dt.Rows)
            {
                AssetTransferRecords reportsModel = new AssetTransferRecords();
                {
                    reportsModel.FromEmpID = row["FromEmpID"].ToString();
                    reportsModel.FromEmployeeName = row["FromEmployeeName"].ToString();
                    reportsModel.Assetbelongsto = row["Assetbelongsto"].ToString();
                    reportsModel.AssetType = row["AssetType"].ToString();
                    reportsModel.HWSWName = row["HWSWName"].ToString();
                    reportsModel.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    reportsModel.AssetTransferDate = row["AssetTransferDate"].ToString();
                    reportsModel.ToEmpID = row["ToEmpID"].ToString();
                    reportsModel.TeamToWhichAssetisTransfered = row["TeamToWhichAssetisTransfered"].ToString();
                    reportsModel.ToEmployeeName = row["ToEmployeeName"].ToString();
                }
                list.Add(reportsModel);
            }
            return list;
        }
    }
}