using I_RAY_ASSET_TRACKER_MVC.Models;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetEntryController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: AssetEntry
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
        public ActionResult SaveAsset(AssetEntryModel assetEntryModel)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
            SqlCommand sqlCommand = new SqlCommand("insert into SaveAsset(Team,HWSWName,HWSWDescriptionCategory,SerialNumberVersionNumber,AssetType,DateofReciept,OwnerShip,HWSWVerifiedBy,HWSWValidatedBy,HWSWVerifiedValidatedDate,InvoiceNo,Remarks,Client,Category,FirmwareVersion,MacAddress,HIProgram,IMEINo,VersionUpdatedDate,PreviousOS,ModelIdentifier,OSVerificationDate,PurchaseDate,RecordUpdatedBy,RecordUpdatedDate) values(@Team,@HWSWName,@HWSWDescriptionCategory,@SerialNumberVersionNumber,@AssetType,@DateofReciept,@OwnerShip,@HWSWVerifiedBy,@HWSWValidatedBy,@HWSWVerifiedValidatedDate,@InvoiceNo,@Remarks,@Client,@Category,@FirmwareVersion,@MacAddress,@HIProgram,@IMEINo,@VersionUpdatedDate,@PreviousOS,@ModelIdentifier,@OSVerificationDate,@PurchaseDate,@RecordUpdatedBy,@RecordUpdatedDate)", sqlConnection);
            sqlConnection.Open();

            sqlCommand.Parameters.AddWithValue("@Team", assetEntryModel.Team);
            sqlCommand.Parameters.AddWithValue("@HWSWName", assetEntryModel.HWSWname);
            sqlCommand.Parameters.AddWithValue("@HWSWDescriptionCategory", assetEntryModel.HWSWDescriptionCategory);
            sqlCommand.Parameters.AddWithValue("@SerialNumberVersionNumber", assetEntryModel.SerialNumberVersionNumber);
            sqlCommand.Parameters.AddWithValue("@AssetType", assetEntryModel.AssetType);
            sqlCommand.Parameters.AddWithValue("@DateofReciept", assetEntryModel.DateofReciept);
            sqlCommand.Parameters.AddWithValue("@OwnerShip", assetEntryModel.OwnerShip);
            sqlCommand.Parameters.AddWithValue("@HWSWVerifiedBy", assetEntryModel.HWSWVerifiedBy);
            sqlCommand.Parameters.AddWithValue("@HWSWValidatedBy", assetEntryModel.HWSWValidatedby);
            sqlCommand.Parameters.AddWithValue("@HWSWVerifiedValidatedDate", assetEntryModel.HWSWVerifiedValidatedDate);
            sqlCommand.Parameters.AddWithValue("@InvoiceNo", assetEntryModel.InvoiceNumber);
            sqlCommand.Parameters.AddWithValue("@Remarks", assetEntryModel.Remarks);
            if (assetEntryModel.yes == true)
            {
                sqlCommand.Parameters.AddWithValue("@Client", "Yes");
            }
           else if(assetEntryModel.yes == false)
            {
                sqlCommand.Parameters.AddWithValue("@Client", "No");
            }          
            if (assetEntryModel.Hardware == true)
            {
                sqlCommand.Parameters.AddWithValue("@Category", "Hardware");
            }
            else if( assetEntryModel.Hardware == false)
            {
                sqlCommand.Parameters.AddWithValue("@Category", "Software");
            }
            sqlCommand.Parameters.AddWithValue("@FirmwareVersion", assetEntryModel.PalpatineVersion);
            sqlCommand.Parameters.AddWithValue("@MacAddress", assetEntryModel.MacAddressSV);
            sqlCommand.Parameters.AddWithValue("@HIProgram", assetEntryModel.HIProgram);
            sqlCommand.Parameters.AddWithValue("@IMEINo", assetEntryModel.IMEINo);
            sqlCommand.Parameters.AddWithValue("@VersionUpdatedDate", assetEntryModel.DateofVersionUpdated);
            sqlCommand.Parameters.AddWithValue("@PreviousOS", assetEntryModel.PreviousOS);
            sqlCommand.Parameters.AddWithValue("@ModelIdentifier", assetEntryModel.ModelIdentifier);
            sqlCommand.Parameters.AddWithValue("@OSVerificationDate", assetEntryModel.OSVerificationDate);
            sqlCommand.Parameters.AddWithValue("@PurchaseDate", assetEntryModel.DateofPurchase);
            sqlCommand.Parameters.AddWithValue("@RecordUpdatedBy", assetEntryModel.RecordUpdatedby);
            sqlCommand.Parameters.AddWithValue("@RecordUpdatedDate", assetEntryModel.RecordUpdatedDate);
            sqlCommand.CommandType = CommandType.Text;
            int i = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            //if (i > 0)
            //{
            //    return RedirectToAction("Index");
            //}
            TempData["Message"] = "The asset has been saved successfully..";
            return RedirectToAction("Index");
        }
        public ActionResult BulkEntryAssets() 
        { 
            return PartialView("BulkEntryAssets"); 
        }
        public ActionResult ConsumableBulkEntryAssets()
        {
            return PartialView("ConsumableBulkEntryAssets");
        }
        public ActionResult UploadAssetList(HttpPostedFileBase AssetListfileupload)
        {
            if (AssetListfileupload != null && AssetListfileupload.ContentLength > 0)
            {
                string filePath = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(AssetListfileupload.FileName));
                AssetListfileupload.SaveAs(filePath);
                // Call method to import data to SQL Server
                ImportDataToSql(filePath);
                TempData["Message"] = "AssetList File uploaded successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "Please upload a file";
            }
            return View();
        }
        private void ImportDataToSql(string filePath)
        {
            string tableName = "SaveAsset";
            string excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
            {
                excelConnection.Open();
                DataTable dt = new DataTable();
                using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [Assetlist$]", excelConnection))
                {
                    oda.Fill(dt);
                }
                using (SqlConnection sqlConnection = new SqlConnection(constr))
                {
                    sqlConnection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        bulkCopy.DestinationTableName = tableName;

                        bulkCopy.ColumnMappings.Add("Sno", "Sno");
                        bulkCopy.ColumnMappings.Add("Team", "Team");
                        bulkCopy.ColumnMappings.Add("HWSWName", "HWSWName");
                        bulkCopy.ColumnMappings.Add("HWSWDescriptionCategory", "HWSWDescriptionCategory");
                        bulkCopy.ColumnMappings.Add("SerialNumberVersionNumber", "SerialNumberVersionNumber").ToString();
                        bulkCopy.ColumnMappings.Add("AssetType", "AssetType");
                        bulkCopy.ColumnMappings.Add("DateofReciept", "DateofReciept");
                        bulkCopy.ColumnMappings.Add("OwnerShip", "OwnerShip");
                        bulkCopy.ColumnMappings.Add("HWSWVerifiedBy", "HWSWVerifiedBy");
                        bulkCopy.ColumnMappings.Add("HWSWValidatedBy", "HWSWValidatedBy");
                        bulkCopy.ColumnMappings.Add("HWSWVerifiedValidatedDate", "HWSWVerifiedValidatedDate");
                        bulkCopy.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                        bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                        bulkCopy.ColumnMappings.Add("Client", "Client");
                        bulkCopy.ColumnMappings.Add("Category", "Category");
                        bulkCopy.ColumnMappings.Add("FirmwareVersion", "FirmwareVersion");
                        bulkCopy.ColumnMappings.Add("MacAddress", "MacAddress");
                        bulkCopy.ColumnMappings.Add("HIProgram", "HIProgram");
                        bulkCopy.ColumnMappings.Add("IMEINo", "IMEINo");
                        bulkCopy.ColumnMappings.Add("VersionUpdatedDate", "VersionUpdatedDate");
                        bulkCopy.ColumnMappings.Add("PreviousOS", "PreviousOS");
                        bulkCopy.ColumnMappings.Add("ModelIdentifier", "ModelIdentifier");
                        bulkCopy.ColumnMappings.Add("OSVerificationDate", "OSVerificationDate");
                        bulkCopy.ColumnMappings.Add("PurchaseDate", "PurchaseDate");
                        bulkCopy.ColumnMappings.Add("RecordUpdatedBy", "RecordUpdatedBy");
                        bulkCopy.ColumnMappings.Add("RecordUpdatedDate", "RecordUpdatedDate");
                        bulkCopy.ColumnMappings.Add("UDIDNumber", "UDIDNumber");
                        bulkCopy.ColumnMappings.Add("UnUsed", "UnUsed");
                        bulkCopy.ColumnMappings.Add("IsReturn", "IsReturn");

                        dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

                        var itemsToDelete = new List<DataRow>();

                        foreach (DataRow row in dt.Rows)
                        {
                            var serialNumber = row.ItemArray[4].ToString();
                            var HWSWName = row.ItemArray[2].ToString();
                            if ((serialNumber != null) || (serialNumber.ToString() != ""))
                            {
                                SqlCommand cmd2 = new SqlCommand("select * from SaveAsset  where SerialNumberVersionNumber ='" + serialNumber + "' ", sqlConnection);
                                int count1 = Convert.ToInt32(cmd2.ExecuteScalar());
                                if (count1 > 0)
                                {
                                    if ((HWSWName != null) || (HWSWName != ""))
                                    {
                                        SqlCommand HWSWName_cmd = new SqlCommand("select HWSWName from SaveAsset  where SerialNumberVersionNumber ='" + serialNumber + "' ", sqlConnection);
                                        SqlDataReader rdr = HWSWName_cmd.ExecuteReader();
                                        while (rdr.Read())
                                        {
                                            //   var cname= rdr["HWSWName"].ToString();
                                            string HWSWName_DB = rdr[0].ToString();
                                            if (HWSWName_DB == HWSWName)
                                            {
                                                itemsToDelete.Add(row);
                                            }
                                        }
                                        rdr.Close();
                                    }
                                }
                            }
                        }
                        List<DataRow> uniqueList = itemsToDelete.Distinct().ToList();
                        foreach (var item in uniqueList)
                        {
                            dt.Rows.Remove(item);
                        }
                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
        }
        //public ActionResult UploadEmployeelist(HttpPostedFileBase EmployeeListfileupload)
        //{
        //    string filePath = Path.Combine(Server.MapPath("~/Files"), Path.GetFileName(EmployeeListfileupload.FileName));
        //    EmployeeListfileupload.SaveAs(filePath);
        //    string conString = string.Empty;
        //    string extension = Path.GetExtension(EmployeeListfileupload.FileName);
        //    switch (extension)
        //    {
        //        case ".xls": //Excel 97-03
        //            conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
        //            break;
        //        case ".xlsx": //Excel 07 or higher
        //            conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
        //            break;
        //    }
        //    conString = string.Format(conString, filePath);
        //    using (OleDbConnection excel_con = new OleDbConnection(conString))
        //    {
        //        excel_con.Open();
        //        string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[2]["TABLE_NAME"].ToString();
        //        DataTable dtExcelData = new DataTable();
        //        //[OPTIONAL]: It is recommended as otherwise the data will be considered as String by default.
        //        dtExcelData.Columns.AddRange(new DataColumn[10] {
        //          new DataColumn("Sno", typeof(int)),
        //        new DataColumn("EmpID", typeof(int)),
        //         new DataColumn("FirstName", typeof(string)),
        //         new DataColumn("MiddleName", typeof(string)),
        //          new DataColumn("LastName", typeof(string)),
        //          new DataColumn("EmployeeName", typeof(string)),
        //          new DataColumn("Team", typeof(string)),
        //          new DataColumn("Role", typeof(string)),
        //          new DataColumn("Mail", typeof(string)),
        //          new DataColumn("ContactNumber", typeof(string))
        //                  });
        //        using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
        //        {
        //            oda.Fill(dtExcelData);
        //        }
        //        excel_con.Close();
        //        string consString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        //        using (SqlConnection con = new SqlConnection(consString))
        //        {
        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //            {
        //                //Set the database table name
        //                sqlBulkCopy.DestinationTableName = "dbo.EmployeeList";
        //                //[OPTIONAL]: Map the Excel columns with that of the database table
        //                sqlBulkCopy.ColumnMappings.Add("Sno", "Sno");
        //                sqlBulkCopy.ColumnMappings.Add("EmpID", "EmpID");
        //                sqlBulkCopy.ColumnMappings.Add("FirstName", "FirstName");
        //                sqlBulkCopy.ColumnMappings.Add("MiddleName", "MiddleName");
        //                sqlBulkCopy.ColumnMappings.Add("LastName", "LastName");
        //                sqlBulkCopy.ColumnMappings.Add("EmployeeName", "EmployeeName");
        //                sqlBulkCopy.ColumnMappings.Add("Team", "Team");
        //                sqlBulkCopy.ColumnMappings.Add("Role", "Role");
        //                sqlBulkCopy.ColumnMappings.Add("Mail", "Mail");
        //                sqlBulkCopy.ColumnMappings.Add("ContactNumber", "ContactNumber");
        //                con.Open();
        //                sqlBulkCopy.WriteToServer(dtExcelData);
        //                con.Close();
        //            }
        //        }
        //    }
        //    TempData["Message"] = "EmployeeList File uploaded successfully";
        //    return RedirectToAction("Index");
        //}
        public ActionResult SaveAssetInQuantity(AssetEntryModel assetEntryModel)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd;
            sqlConnection.Open();
            SqlCommand selectCmd = new SqlCommand("select * from SaveAssetInQuantity  where HWSWName ='" + assetEntryModel.ConsumableBulkEntryHWSWname + "' and AssetType='" + assetEntryModel.ConsumableBulkEntryAssetType + "' and Team='" + assetEntryModel.ConsumableBulkEntryTeam + "' and InvoiceNo='" + assetEntryModel.ConsumableBulkEntryInvoiceNumber + "'", sqlConnection);
            int count = Convert.ToInt32(selectCmd.ExecuteScalar());
            string Quantity = "";
            if (count > 0)
            {
                SqlCommand Quantity_cmd = new SqlCommand("select Quantity from SaveAssetInQuantity where HWSWName ='" + assetEntryModel.ConsumableBulkEntryHWSWname + "' and AssetType='" + assetEntryModel.ConsumableBulkEntryAssetType + "' and Team='" + assetEntryModel.ConsumableBulkEntryTeam + "' and InvoiceNo='" + assetEntryModel.ConsumableBulkEntryInvoiceNumber + "'", sqlConnection);
                SqlDataReader rdr = Quantity_cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Quantity = rdr[0].ToString();
                }
                rdr.Close();
                string newQuantity = assetEntryModel.ConsumableBulkEntryQuantity;
                int AddingQuantity = Convert.ToInt32(Quantity) + Convert.ToInt32(newQuantity);
                string UpdatedQuantity = AddingQuantity.ToString();
                String sql = "";
                SqlDataAdapter adapter = new SqlDataAdapter();
                sql = "Update SaveAssetInQuantity set Quantity='" + UpdatedQuantity + "' where HWSWName ='" + assetEntryModel.ConsumableBulkEntryHWSWname + "' and AssetType='" + assetEntryModel.ConsumableBulkEntryAssetType + "' and Team='" + assetEntryModel.ConsumableBulkEntryTeam + "' and InvoiceNo='" + assetEntryModel.ConsumableBulkEntryInvoiceNumber + "'";
                SqlCommand UpdateCommand = new SqlCommand(sql, sqlConnection);
                adapter.InsertCommand = new SqlCommand(sql, sqlConnection);
                UpdateCommand.ExecuteNonQuery();
            }
            else
            {
                string s = "insert into SaveAssetInQuantity(Team,HWSWName,HWSWVerifiedBy,HWSWDescriptionCategory,InvoiceNo,AssetType,DateofReciept,Quantity,HWSWValidatedBy,HWSWVerifiedValidatedDate,Client,Remarks,RecordUpdatedBy,RecordUpdatedDate) values(@Team,@HWSWName,@HWSWVerifiedBy,@HWSWDescriptionCategory,@InvoiceNo,@AssetType,@DateofReciept,@Quantity,@HWSWValidatedBy,@HWSWVerifiedValidatedDate,@Client,@Remarks,@RecordUpdatedBy,@RecordUpdatedDate)";
                cmd = new SqlCommand(s, sqlConnection);
                cmd.Parameters.AddWithValue("@Team", assetEntryModel.ConsumableBulkEntryTeam);
                cmd.Parameters.AddWithValue("@HWSWName", assetEntryModel.ConsumableBulkEntryHWSWname);
                cmd.Parameters.AddWithValue("@HWSWVerifiedBy", assetEntryModel.ConsumableBulkEntryHWSWVerifiedBy);
                cmd.Parameters.AddWithValue("@HWSWDescriptionCategory", assetEntryModel.ConsumableBulkEntryHWSWDescriptionCategory);
                cmd.Parameters.AddWithValue("@InvoiceNo", assetEntryModel.ConsumableBulkEntryInvoiceNumber);
                cmd.Parameters.AddWithValue("@AssetType", assetEntryModel.ConsumableBulkEntryAssetType);
                cmd.Parameters.AddWithValue("@DateofReciept", assetEntryModel.ConsumableBulkEntryDateofReciept);
                cmd.Parameters.AddWithValue("@Quantity", assetEntryModel.ConsumableBulkEntryQuantity);
                cmd.Parameters.AddWithValue("@HWSWValidatedBy", assetEntryModel.ConsumableBulkEntryHWSWVerifiedBy);
                cmd.Parameters.AddWithValue("@HWSWVerifiedValidatedDate", assetEntryModel.ConsumableBulkEntryHWSWVerifiedValidatedDate);
                if (assetEntryModel.ConsumableBulkEntryyes == true)
                {
                    cmd.Parameters.AddWithValue("@Client", "Yes");
                }
                else if (assetEntryModel.ConsumableBulkEntryyes == false)
                {
                    cmd.Parameters.AddWithValue("@Client", "No");
                }
                cmd.Parameters.AddWithValue("@Remarks", assetEntryModel.ConsumableBulkEntryRemarks);
                cmd.Parameters.AddWithValue("@RecordUpdatedBy", assetEntryModel.ConsumableBulkEntryRecordUpdatedby);
                cmd.Parameters.AddWithValue("@RecordUpdatedDate", assetEntryModel.ConsumableBulkEntryRecordUpdatedDate);
                cmd.CommandType = CommandType.Text;
                int i = cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }
            TempData["Message"] = "Asset Saved Successfully";
            return RedirectToAction("Index");
        }
    }
}