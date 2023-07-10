using I_RAY_ASSET_TRACKER_MVC.Models;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class AssetEntryController : Controller
    {
        // GET: AssetEntry
        public ActionResult Index()
        {
            System.Threading.Thread.Sleep(3000);
            return View();
        }
        public ActionResult SaveAsset(AssetEntryModel assetEntryModel)
        {
            SqlConnection sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = @"Data Source=fswiray77\sqlexpress;Initial Catalog=AssetManagement;Integrated Security=True";
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
            return RedirectToAction("Index");
        }
    }
}