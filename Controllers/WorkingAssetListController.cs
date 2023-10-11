using I_RAY_ASSET_TRACKER_MVC.Models;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class WorkingAssetListController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        List<WorkingAssetList> list;
        // GET: WorkingAssetList
        public ActionResult Index()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("SELECT DISTINCT Team FROM EmployeeList WHERE Team NOT IN('Support','HR','Automation')", sqlConnection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            List<string> TeamList = new List<string>();
            foreach (System.Data.DataRow dataRow in dataSet.Tables[0].Rows)
            {
                TeamList.Add(dataRow["Team"].ToString());
            }
            SelectList Team = new SelectList(TeamList);
            ViewData["Team"] = Team;

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

            sqlConnection.Close();
            return View();
        }
        public JsonResult AssetType()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand("select distinct[AssetType] from SaveAsset where AssetType IS NOT NULL", sqlConnection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            List<string> AssetTypeList = new List<string>();
            foreach (System.Data.DataRow dataRow in ds.Tables[0].Rows)
            {
                AssetTypeList.Add(dataRow["AssetType"].ToString());
            }
            sqlConnection.Close();
            return Json(AssetTypeList);
        }
        public JsonResult AssetTeam()
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("SELECT DISTINCT Team FROM EmployeeList WHERE Team NOT IN('Support','HR','Automation')", sqlConnection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            List<string> TeamList = new List<string>();
            foreach (System.Data.DataRow dataRow in dataSet.Tables[0].Rows)
            {
                TeamList.Add(dataRow["Team"].ToString());
            }
            sqlConnection.Close();
            return Json(TeamList);
        }
        public ActionResult AssetBelongsLoadPartialView(string assetBelongsToselectedValue)
        {          
            if (assetBelongsToselectedValue == "i-ray Asset")
            {
                list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
            }
            else if (assetBelongsToselectedValue == "Client Asset")
            {
                list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
            }
            else
            {
                list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
            }
            var viewModel = new WorkingAssetListModel
            {
                workingAssetList = list
            };
            return PartialView("AssetBelongsLoadPartialView", viewModel);
        }
        private List<WorkingAssetList> GetWorkingAssetList(string query)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand objcmd = new SqlCommand(query, sqlConnection);
            SqlDataAdapter adp = new SqlDataAdapter(objcmd);
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<WorkingAssetList> list = new List<WorkingAssetList>();
            foreach (DataRow row in dt.Rows)
            {
                WorkingAssetList listItem = new WorkingAssetList();
                {
                    listItem.Sno = row["Sno"].ToString();
                    listItem.Team = row["Team"].ToString();
                    listItem.HWSWName = row["HWSWName"].ToString();
                    listItem.HWSWDescriptionCategory = row["HWSWDescriptionCategory"].ToString();
                    listItem.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                    listItem.AssetType = row["AssetType"].ToString();
                    listItem.DateofReciept = row["DateofReciept"].ToString();
                    listItem.Category = row["Category"].ToString();
                    listItem.InvoiceNo = row["InvoiceNo"].ToString();
                }
                list.Add(listItem);
            }
            return list;
        }
        public ActionResult AssetTypeLoadPartialView(string assetBelongsToselectedValue, string assetType)
        {
            if (assetBelongsToselectedValue == "i-ray Asset")
            {
                list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + assetType + "'");
            }
            else if (assetBelongsToselectedValue == "Client Asset")
            {
                list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + assetType + "'");
            }
            var viewModel = new WorkingAssetListModel
            {
                workingAssetList = list
            };
            return PartialView("AssetTypeLoadPartialView",viewModel);
        }
        public ActionResult LoadPartialView(string teamselectedValue,string assetTypeselectedValue,string assetBelongsToselectedValue)
        {
            if (assetBelongsToselectedValue == "")
            {
                if (assetTypeselectedValue == "")
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                }
            }
            else if (assetBelongsToselectedValue == "i-ray Asset")
            {
                if(assetTypeselectedValue == "")
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                }
                else if (assetTypeselectedValue == "-- Select --")
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                }
                else
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + assetTypeselectedValue + "'");
                }
            }
            else if (assetBelongsToselectedValue == "Client Asset")
            {
                if( assetTypeselectedValue == "")
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                }
                else if (assetTypeselectedValue == "-- Select --")
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                }
                else
                {
                    list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + teamselectedValue + "' and Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + assetTypeselectedValue + "'");
                }
            }
            var viewModel = new WorkingAssetListModel
            {
                workingAssetList = list
            };
            return PartialView("LoadPartialView", viewModel);
        }
        public ActionResult SearchSerialNoInvoiceNo(string serialNoInvoiceNo)
        {
            list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where (SerialNumberVersionNumber='" + serialNoInvoiceNo + "' or InvoiceNo='" + serialNoInvoiceNo + "') and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
            var viewModel = new WorkingAssetListModel
            {
                workingAssetList = list
            };
            return PartialView("SearchSerialNoInvoiceNo",viewModel);
        }

        [Obsolete]
        public ActionResult SerialNoInvoiceNoPDFReprot(WorkingAssetListModel workingAssetListModel)
        {
            list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where (SerialNumberVersionNumber='" + workingAssetListModel.SerilaNoInvoiceNo + "' or InvoiceNo='" + workingAssetListModel.SerilaNoInvoiceNo + "') and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
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
                    docTitle.Add(("i-ray Asset List : " + DateTime.Now.ToString("dd/MM/yyyy")));
                    docTitle.SpacingBefore = 150f;
                    docTitle.SpacingAfter = 1f;
                    docTitle.Alignment = Element.ALIGN_RIGHT;
                    pdfDoc.Add(docTitle);
                    pdfDoc.Add(image);

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;" + "filename=AssetsList.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
            return View();
        }

        [Obsolete]
        public ActionResult PDFReport(WorkingAssetListModel workingAssetListModel)
        {
            if (workingAssetListModel.AssetBelongsTo == null)
            {
                if(workingAssetListModel.AssetType == null)
                {
                    if(workingAssetListModel.Team==null)
                    {
                        list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + workingAssetListModel.Team + "' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");                    
                    }
                    else
                    {
                        list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + workingAssetListModel.Team + "' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");                    
                    }
                }                
            }
            else if (workingAssetListModel.AssetBelongsTo == "i-ray Asset")
            {
                if (workingAssetListModel.AssetType == "-- Select --")
                {
                    if (workingAssetListModel.Team == "-- Select Team --")
                    {
                        list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                    }
                    else
                    {
                        list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + workingAssetListModel.Team + "' and Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                    }
                }
                else
                {
                    if (workingAssetListModel.Team == "-- Select Team --")
                    {
                        list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + workingAssetListModel.AssetType + "'");
                    }
                    else
                    {
                        list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + workingAssetListModel.Team + "' and Client='No' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + workingAssetListModel.AssetType + "'");
                    }
                }             
            }
            else if (workingAssetListModel.AssetBelongsTo == "Client Asset")
            {
                if (workingAssetListModel.AssetType == "-- Select --")
                {
                    if (workingAssetListModel.Team == "-- Select Team --")
                    {
                        list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                    }
                    else
                    {
                        list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + workingAssetListModel.Team + "' and Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))");
                    }
                }
                else
                {
                    if (workingAssetListModel.Team == "-- Select Team --")
                    {
                        list = GetWorkingAssetList("Select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo FROM SaveAsset where Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + workingAssetListModel.AssetType + "'");
                    }
                    else
                    {
                        list = GetWorkingAssetList("select Sno,Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,Category,InvoiceNo from SaveAsset where Team='" + workingAssetListModel.Team + "' and Client='Yes' and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null)) and AssetType='" + workingAssetListModel.AssetType + "'");
                    }
                }
            }
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
                    docTitle.Add(("i-ray Asset List : " + DateTime.Now.ToString("dd/MM/yyyy")));
                    docTitle.SpacingBefore = 150f;
                    docTitle.SpacingAfter = 1f;
                    docTitle.Alignment = Element.ALIGN_RIGHT;
                    pdfDoc.Add(docTitle);
                    pdfDoc.Add(image);

                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;" + "filename=AssetsList.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }

            return View(workingAssetListModel);
        }
    }
}