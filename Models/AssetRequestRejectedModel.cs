using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AssetRequestRejectedModel
    {
        public string Team { get; set; }
        public List<AssetRequestReject> assetRequestRejects { get; set; }
    }
    public class AssetRequestReject
    {
        public string EmpID { get; set; }
        public string EmpTeam { get; set; }
        public string AssetTeam { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string RequestDate { get; set; }
    }
}