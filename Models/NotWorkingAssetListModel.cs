using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class NotWorkingAssetListModel
    {
        public string AssetBelongsTo { get; set; }
        public string AssetName { get; set; }
        public string SerialNumber { get; set; }
        public List<NotWorkingAssetList> notWorkingAssetList { get; set; }
    }
    public class NotWorkingAssetList
    {
        public string Team { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetType { get; set; }
        public string DateofReciept { get; set; }
        public string Category { get; set; }
        public string AssetRemarks { get; set; }
    }
}