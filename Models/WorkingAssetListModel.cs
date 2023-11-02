using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class WorkingAssetListModel
    {
        public string AssetBelongsTo { get; set; }
        public string AssetType { get; set; }
        public string Team { get; set; }
        public string SerilaNoInvoiceNo { get; set; }
        public string HWSWName { get; set; }
        public List<WorkingAssetList> workingAssetList { get; set; }     

    }
    public class WorkingAssetList 
    {
        public string Sno { get; set; }
        public string Team { get; set; }
        public string HWSWName { get; set; }
        public string HWSWDescriptionCategory { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetType { get; set; }
        public string DateofReciept { get; set; }
        public string Category { get; set; }
        public string InvoiceNo { get; set; }
        public bool UnUsed { get; set; }
        public bool IsReturn { get; set; }
        public string AssetRemarks { get; set; }
    }
}