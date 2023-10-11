using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using I_RAY_ASSET_TRACKER_MVC.Models;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class NotWorkingAssetListController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        List<NotWorkingAssetList> list;
        // GET: NotWorkingAssetList
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
        public ActionResult TeamListPartialView(string assetBelongsToselected)
        {
            if (assetBelongsToselected == "FSW")
            {
                list = TeamNotWorkingAssetList("ProFSWunsuedassets");
            }
            else if (assetBelongsToselected == "SV")
            {
                list = TeamNotWorkingAssetList("ProSVunsuedassets");
            }
            var viewModel = new NotWorkingAssetListModel
            {
                notWorkingAssetList = list
            };
            return PartialView("TeamListPartialView", viewModel);
        }
        private List<NotWorkingAssetList> TeamNotWorkingAssetList(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand objcmd = new SqlCommand(query, sqlConnection);
            objcmd.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
            SqlDataAdapter adp = new SqlDataAdapter(objcmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            sqlConnection.Close();
            List<NotWorkingAssetList> list = new List<NotWorkingAssetList>();
            foreach (DataRow row in dt.Rows)
            {
                NotWorkingAssetList listItem = new NotWorkingAssetList();
                {
                    listItem.Team = row["Team"].ToString();
                    listItem.HWSWName = row["HWSWName"].ToString();
                    listItem.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    listItem.AssetType = row["AssetType"].ToString();
                    listItem.DateofReciept = row["DateofReciept"].ToString();
                    listItem.Category = row["Category"].ToString();
                    listItem.AssetRemarks = row["AssetRemarks"].ToString();
                }
                list.Add(listItem);
            }
            return list;
        }
        [HttpPost]
        [Obsolete]
        public ActionResult PDFReportNotWorkingAssetList(FormCollection formCollection,NotWorkingAssetListModel notWorkingAssetListModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "PdfReport":
                    if (notWorkingAssetListModel.AssetBelongsTo == "FSW")
                    {
                        List<NotWorkingAssetList> list = TeamNotWorkingAssetList("ProFSWunsuedassets");
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
                                docTitle.Add(("NotWorkingAssetList : " + DateTime.Now.ToString("dd/MM/yyyy")));
                                docTitle.SpacingBefore = 150f;
                                docTitle.SpacingAfter = 1f;
                                docTitle.Alignment = Element.ALIGN_RIGHT;
                                pdfDoc.Add(docTitle);
                                pdfDoc.Add(image);

                                htmlparser.Parse(sr);
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;" + "filename=NotWorkingAssetList.pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.End();
                            }
                        }
                    }
                    else if (notWorkingAssetListModel.AssetBelongsTo == "SV")
                    {
                        List<NotWorkingAssetList> list = TeamNotWorkingAssetList("ProSVunsuedassets");
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
                                docTitle.Add(("NotWorkingAssetList : " + DateTime.Now.ToString("dd/MM/yyyy")));
                                docTitle.SpacingBefore = 150f;
                                docTitle.SpacingAfter = 1f;
                                docTitle.Alignment = Element.ALIGN_RIGHT;
                                pdfDoc.Add(docTitle);
                                pdfDoc.Add(image);

                                htmlparser.Parse(sr);
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;" + "filename=NotWorkingAssetList.pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.End();
                            }
                        }
                    }
                    return RedirectToAction("Index");
                case "ExcelReport":
                    if (notWorkingAssetListModel.AssetBelongsTo == "FSW")
                    {
                        List<NotWorkingAssetList> list = TeamNotWorkingAssetList("ProFSWunsuedassets");
                        var data = list;
                        GridView gridview = new GridView();
                        gridview.DataSource = data;
                        gridview.DataBind();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Charset = "";
                        //string FileName = "NotWorkingList" + DateTime.Now + ".xlsx";
                        string FileName = "NotWorkingList.xls";
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
                    }
                    else if (notWorkingAssetListModel.AssetBelongsTo == "SV")
                    {
                        List<NotWorkingAssetList> list = TeamNotWorkingAssetList("ProSVunsuedassets");
                        var data = list;
                        GridView gridview = new GridView();
                        gridview.DataSource = data;
                        gridview.DataBind();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Charset = "";
                        //string FileName = "Reports" + DateTime.Now + ".xlsx";
                        string FileName = "NotWorkingList.xls";
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
                    }
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        public ActionResult ReportNotWorkingAssetListWithSerialNo(string assetSerialNo)
        {
            list = NotWorkingAssetListWithSerialNumber("pronotWorkingAssets", assetSerialNo);
            var viewModel = new NotWorkingAssetListModel
            {
                notWorkingAssetList = list
            };
            return PartialView("TeamListPartialView", viewModel);
        }
        private List<NotWorkingAssetList> NotWorkingAssetListWithSerialNumber(string query,string serialNumber)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand objcmd = new SqlCommand(query, sqlConnection);
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.Parameters.AddWithValue("@SerialNumberVersionNumber", serialNumber);
            sqlConnection.Open();
            SqlDataAdapter adp = new SqlDataAdapter(objcmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            sqlConnection.Close();
            List<NotWorkingAssetList> list = new List<NotWorkingAssetList>();
            foreach (DataRow row in dt.Rows)
            {
                NotWorkingAssetList listItem = new NotWorkingAssetList();
                {
                    listItem.Team = row["Team"].ToString();
                    listItem.HWSWName = row["HWSWName"].ToString();
                    listItem.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    listItem.AssetType = row["AssetType"].ToString();
                    listItem.DateofReciept = row["DateofReciept"].ToString();
                    listItem.Category = row["Category"].ToString();
                    listItem.AssetRemarks = row["AssetRemarks"].ToString();
                }
                list.Add(listItem);
            }
            return list;
        }

        [Obsolete]
        public ActionResult ReportNotWorkingListWithSerialNumber(FormCollection formCollection, NotWorkingAssetListModel notWorkingAssetListModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "PdfReport":
                    list = NotWorkingAssetListWithSerialNumber("pronotWorkingAssets", notWorkingAssetListModel.SerialNumber);
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
                            docTitle.Add(("NotWorkingAssetList : " + DateTime.Now.ToString("dd/MM/yyyy")));
                            docTitle.SpacingBefore = 150f;
                            docTitle.SpacingAfter = 1f;
                            docTitle.Alignment = Element.ALIGN_RIGHT;
                            pdfDoc.Add(docTitle);
                            pdfDoc.Add(image);

                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment;" + "filename=NotWorkingAssetList.pdf");
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
                    return RedirectToAction("Index");
                case "ExcelReport":
                    list = NotWorkingAssetListWithSerialNumber("pronotWorkingAssets", notWorkingAssetListModel.SerialNumber);
                    var dataReport = list;
                    GridView gridview1 = new GridView();
                    gridview1.DataSource = dataReport;
                    gridview1.DataBind();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Charset = "";
                    //string FileName = "NotWorkingList" + DateTime.Now + ".xlsx";
                    string FileName = "NotWorkingList.xls";
                    StringWriter strwritter = new StringWriter();
                    HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    gridview1.GridLines = GridLines.Both;
                    gridview1.HeaderStyle.Font.Bold = true;
                    gridview1.RenderControl(htmltextwrtter);
                    Response.Write(strwritter.ToString());
                    Response.End();
                    return RedirectToAction("Index");
            }
            return View(formCollection);
        }
        public ActionResult ReturnAssetsToClient()
        {
            return PartialView("ReturnAssetsToClient");
        }
        public ActionResult ReturnedAssetstoClientPartialView(string assetBelongsToselected)
        {
            if (assetBelongsToselected == "FSW")
            {
                list = TeamNotWorkingAssetList("ProFSWAssetsToClient");
            }
            else if(assetBelongsToselected == "SV")
            {
                list = TeamNotWorkingAssetList("ProSVAssetsToClient");
            }
            var viewModel = new NotWorkingAssetListModel
            {
                notWorkingAssetList = list
            };
            return PartialView("AssetsToClientPartialView", viewModel);
        }

        [Obsolete]
        public ActionResult ReportReturnedAssetstoClientList(FormCollection formCollection, NotWorkingAssetListModel notWorkingAssetListModel)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "PdfReport":
                    if (notWorkingAssetListModel.AssetName == "FSW")
                    {
                        list = TeamNotWorkingAssetList("ProFSWAssetsToClient");
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
                                docTitle.Add(("ReturnedAssetListToClient Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
                                docTitle.SpacingBefore = 150f;
                                docTitle.SpacingAfter = 1f;
                                docTitle.Alignment = Element.ALIGN_RIGHT;
                                pdfDoc.Add(docTitle);
                                pdfDoc.Add(image);

                                htmlparser.Parse(sr);
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;" + "filename=ReturnedAssetList.pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.End();
                            }
                        }
                    }
                    else if (notWorkingAssetListModel.AssetName == "SV")
                    {
                        list = TeamNotWorkingAssetList("ProSVAssetsToClient");
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
                                docTitle.Add(("ReturnedAssetListToClient Reports : " + DateTime.Now.ToString("dd/MM/yyyy")));
                                docTitle.SpacingBefore = 150f;
                                docTitle.SpacingAfter = 1f;
                                docTitle.Alignment = Element.ALIGN_RIGHT;
                                pdfDoc.Add(docTitle);
                                pdfDoc.Add(image);

                                htmlparser.Parse(sr);
                                pdfDoc.Close();
                                Response.ContentType = "application/pdf";
                                Response.AddHeader("content-disposition", "attachment;" + "filename=ReturnedAssetList.pdf");
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.Write(pdfDoc);
                                Response.End();
                            }
                        }
                    }
                    return RedirectToAction("Index");
                case "ExcelReport":
                    if (notWorkingAssetListModel.AssetName == "FSW")
                    {
                        list = TeamNotWorkingAssetList("ProFSWAssetsToClient");
                        var dataReport = list;
                        GridView gridview1 = new GridView();
                        gridview1.DataSource = dataReport;
                        gridview1.DataBind();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Charset = "";
                        //string FileName = "NotWorkingList" + DateTime.Now + ".xlsx";
                        string FileName = "ReturnedAssetList.xls";
                        StringWriter strwritter = new StringWriter();
                        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                        gridview1.GridLines = GridLines.Both;
                        gridview1.HeaderStyle.Font.Bold = true;
                        gridview1.RenderControl(htmltextwrtter);
                        Response.Write(strwritter.ToString());
                        Response.End();
                    }
                    else if (notWorkingAssetListModel.AssetName == "SV")
                    {
                        list = TeamNotWorkingAssetList("ProSVAssetsToClient");
                        var dataReport = list;
                        GridView gridview1 = new GridView();
                        gridview1.DataSource = dataReport;
                        gridview1.DataBind();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.ClearContent();
                        Response.ClearHeaders();
                        Response.Charset = "";
                        //string FileName = "NotWorkingList" + DateTime.Now + ".xlsx";
                        string FileName = "ReturnedAssetList.xls";
                        StringWriter strwritter = new StringWriter();
                        HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                        gridview1.GridLines = GridLines.Both;
                        gridview1.HeaderStyle.Font.Bold = true;
                        gridview1.RenderControl(htmltextwrtter);
                        Response.Write(strwritter.ToString());
                        Response.End();
                    }
                    return RedirectToAction("Index");
            }
            return View();
        }
    }
}