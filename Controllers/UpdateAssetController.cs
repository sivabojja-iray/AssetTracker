using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I_RAY_ASSET_TRACKER_MVC.Models;
using System.Web.UI;
using PagedList;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class UpdateAssetController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: UpdateAsset
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SearchSerialNumber(UpdateAssetModel updateAssetModel)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand comnd = new SqlCommand("select Sno,Team,HWSWName,SerialNumberVersionNumber,AssetType,InvoiceNo from SaveAsset where SerialNumberVersionNumber='" + updateAssetModel.SerialNumber + "'  and ((UnUsed = 0 or UnUsed is null)  and (IsReturn = 0 or IsReturn is null))", con);
            SqlDataAdapter adp = new SqlDataAdapter(comnd); 
            DataTable dt = new DataTable();
            adp.Fill(dt);
            List<UpdateAssetModel> dataModels = new List<UpdateAssetModel>();
            foreach (DataRow row in dt.Rows)
            {
                updateAssetModel.Team = row["Team"].ToString();
                updateAssetModel.HWSWName = row["HWSWName"].ToString();
                updateAssetModel.SerialNumberVersionNumber = row["SerialNumberVersionNumber"].ToString();
                updateAssetModel.AssetType = row["AssetType"].ToString();
                updateAssetModel.InvoiceNo = row["InvoiceNo"].ToString();
            }
            dataModels.Add(updateAssetModel);
            return PartialView("_UpdateAssetPartialView",updateAssetModel);
        }
        public ActionResult AssetUpdateDetails(string Team,string HWSWName,string SerialNumberVersionNumber, string AssetType,string InvoiceNo)
        {
            return PartialView("AssetUpdateDetails");
        }
        public ActionResult SaveUpdateAssetDetails(UpdateAssetModel updateAssetModel)
        {
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            string saveAsset = "UPDATE SaveAsset SET Team = '" + updateAssetModel.Team + "',HWSWName='" + updateAssetModel.HWSWName + "',AssetType='" + updateAssetModel.AssetType + "',InvoiceNo='" + updateAssetModel.InvoiceNo + "' WHERE SerialNumberVersionNumber = '" + updateAssetModel.SerialNumberVersionNumber + "'";
            SqlCommand cmd = new SqlCommand(saveAsset, con);
            cmd.CommandType = CommandType.Text;
            int i = cmd.ExecuteNonQuery();
            string AssignAsset = "UPDATE AssignAsset SET Team = '" + updateAssetModel.Team + "',HWSWName='" + updateAssetModel.HWSWName + "',AssetType='" + updateAssetModel.AssetType + "',InvoiceNo='" + updateAssetModel.InvoiceNo + "' WHERE SerialNumberVersionNumber = '" + updateAssetModel.SerialNumberVersionNumber + "'";
            SqlCommand cmd2 = new SqlCommand(AssignAsset, con);
            cmd2.CommandType = CommandType.Text;
            int j = cmd2.ExecuteNonQuery();
            string AssignAsset1 = "UPDATE AssignAsset1 SET Team = '" + updateAssetModel.Team + "',HWSWName='" + updateAssetModel.HWSWName + "',AssetType='" + updateAssetModel.AssetType + "',InvoiceNo='" + updateAssetModel.InvoiceNo + "' WHERE SerialNumberVersionNumber = '" + updateAssetModel.SerialNumberVersionNumber + "'";
            SqlCommand cmd3 = new SqlCommand(AssignAsset1, con);
            cmd3.CommandType = CommandType.Text;
            int k = cmd3.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }
    }
}