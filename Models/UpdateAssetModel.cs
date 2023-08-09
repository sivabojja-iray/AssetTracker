using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class UpdateAssetModel
    {
        public string SerialNumber { get; set; }
        public string Team { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetType { get; set; }
        public string InvoiceNo { get; set; }

    }
}