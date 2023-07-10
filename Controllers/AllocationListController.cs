using I_RAY_ASSET_TRACKER_MVC.Models;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AllocationListController : Controller
    {
        // GET: AllocationList
        public ActionResult Index()
        {
            List<AllocationListModel> list = allocationLists("select EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate,Assetbelongsto,InvoiceNo from AssignAsset order by EmpID");
            System.Threading.Thread.Sleep(500);
            return View(list);
        }
        //public JsonResult getAllocationList()
        //{
        //    List<AllocationListModel> list = allocationLists("select EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate,Assetbelongsto,InvoiceNo from AssignAsset order by EmpID");
        //    var data = list;
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        private static List<AllocationListModel> allocationLists(string query)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Connection = sqlConnection;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<AllocationListModel> dataModels = new List<AllocationListModel>();

            foreach (DataRow row in dt.Rows)
            {
                AllocationListModel allocationList = new AllocationListModel();
                {
                    allocationList.EmpID = row["EmpID"].ToString();
                    allocationList.EmployeeName = row["EmployeeName"].ToString();
                    allocationList.Team = row["Team"].ToString();
                    allocationList.AssetType = row["AssetType"].ToString();
                    allocationList.HWSWName = row["HWSWName"].ToString();
                    allocationList.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    allocationList.AssignDate = row["AssignDate"].ToString();
                    allocationList.ExpectedReturnDate = row["ExpectedReturnDate"].ToString();
                    allocationList.Assetbelongsto = row["Assetbelongsto"].ToString();
                    allocationList.InvoiceNo = row["InvoiceNo"].ToString();
                }
                dataModels.Add(allocationList);
            }
            return dataModels;
        }
       
        public ActionResult ExportToExcel()
        {
            List<AllocationListModel> list = allocationLists("select EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate,Assetbelongsto,InvoiceNo from AssignAsset order by EmpID");
            var data = list;
             GridView gridview = new GridView();
             gridview.DataSource = data;
             gridview.DataBind();
             Response.Clear();
             Response.Buffer = true;
             Response.ClearContent();
             Response.ClearHeaders();
             Response.Charset = "";
                // string FileName = "AllocationList" + DateTime.Now + ".xlsx";
             string FileName = "AllocationList.xls";
             StringWriter strwritter = new StringWriter();
             HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
             Response.Cache.SetCacheability(HttpCacheability.NoCache);
             Response.ContentType = "application/vnd.ms-excel";
             Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
             gridview.GridLines = GridLines.Both;
             gridview.HeaderStyle.Font.Bold = true;
             gridview.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
             Response.End();
            return View();
        }

        [Obsolete]
        public ActionResult ExportToPdf()
        {
            List<AllocationListModel> list = allocationLists("select EmpID,EmployeeName,Team,AssetType,HWSWName,SerialNumberVersionNumber,AssignDate,ExpectedReturnDate,Assetbelongsto,InvoiceNo from AssignAsset order by EmpID");
            var data = list;
            using(StringWriter sw = new StringWriter())
            {
                using(HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    GridView gridview = new GridView();
                    gridview.DataSource = data;
                    gridview.DataBind();
                    gridview.RenderControl(hw);

                    StringReader sr=new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
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
                    Response.AddHeader("content-disposition", "attachment;" + "filename=AllocationList.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }               
            }
            return View();
        }
    }
}