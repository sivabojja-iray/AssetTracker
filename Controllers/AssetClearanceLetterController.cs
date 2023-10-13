using I_RAY_ASSET_TRACKER_MVC.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetClearanceLetterController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: AssetClearanceLetter
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
        public ActionResult AssetClearanceLetterPDF(AssetClearanceLetterModel assetClearanceLetterModel)
        {
            string EmployeeName = null;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmdd = new SqlCommand("select EmployeeName from EmployeeList where EmpID=@EmpID");
            cmdd.CommandType = CommandType.Text;
            cmdd.Connection = con;
            con.Open();
            cmdd.Parameters.AddWithValue("@EmpID", assetClearanceLetterModel.EmpID);
            SqlDataReader dr = cmdd.ExecuteReader();
            if (dr.Read())
            {
                EmployeeName = dr["EmployeeName"].ToString();
            }
            con.Close();
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
                document.Open();
                    //   string imagepath = Server.MapPath(".") + "/images/icon/logo.png";
                    string imagepath = Server.MapPath("~/Images/Icon/logo_irayit.jpg");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imagepath);
                    //  image.ScalePercent(24f);
                    image.ScaleToFit(140f, 120f);

                    //Give space before image

                    image.SpacingBefore = 10f;

                    //Give some space after the image

                    image.SpacingAfter = 1f;
                    image.Alignment = Element.ALIGN_LEFT;
                    var docTitle = new Paragraph();
                    var titleFont = FontFactory.GetFont("TIMES_ROMAN", 8, BaseColor.RED);
                    docTitle.Font = titleFont;
                    docTitle.Add("");
                    docTitle.Add(("Reports:" + DateTime.Now.ToString("dd/MM/yyyy")));
                    docTitle.SpacingBefore = 150f;
                    docTitle.SpacingAfter = 1f;
                    docTitle.Alignment = Element.ALIGN_RIGHT;
                    // docTitle.Add(docTitle);
                    document.Add(docTitle);
                    document.Add(image);
                Paragraph para7 = new Paragraph("\n");
                document.Add(para7);
                Paragraph para0 = new Paragraph("To Whom ever concern,");
                document.Add(para0);
                Paragraph para18 = new Paragraph("\n");
                document.Add(para18);
                Paragraph para = new Paragraph("This letter to inform you that, Mr/Mrs. " + EmployeeName + ",  with Employee ID :" + assetClearanceLetterModel.EmpID + " has completed their tenure at I-Ray IT SOLUTIONS. I am pleased to confirm that " + EmployeeName + "has returned all company assets in their possession.");
                document.Add(para);
                Paragraph para10 = new Paragraph("\n");
                document.Add(para10);
                Paragraph para11 = new Paragraph("\n");
                document.Add(para10);
                Paragraph para2 = new Paragraph("" + EmployeeName + " has diligently followed the prescribed procedures and guidelines for returning the assets. They have handed over the assets to the designated department responsible for asset management. We ensured that the assets were returned in their original condition, considering any standard wear and tear.");
                document.Add(para2);
                Paragraph para12 = new Paragraph("\n");
                document.Add(para10);
                Paragraph para13 = new Paragraph("\n");
                document.Add(para10);
                Paragraph para3 = new Paragraph("I would like to take this opportunity to express our gratitude to " + EmployeeName + " for their valuable contributions during their employment at I-Ray IT SOLUTIONS. Their dedication and professionalism have been commendable. ");
                document.Add(para3);
                Paragraph para14 = new Paragraph("\n");
                document.Add(para14);
                Paragraph para15 = new Paragraph("\n");
                document.Add(para15);
                Paragraph para6 = new Paragraph("Yours sincerely,");
                document.Add(para6);
                Paragraph para9 = new Paragraph("" + EmployeeName + "");
                document.Add(para9);
                document.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;" + "filename=AssetClearance_Letter.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(document);
                Response.End();
            }
            return RedirectToAction("Index");
        }
    }
}