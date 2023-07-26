using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class MyRequestListModel
    {
        public string RequestID { get; set; }
        public string EmpID { get; set; }
        public string EmpTeam { get; set; }
        public string AssetTeam { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string RequestDate { get; set; }
        public string Quantity { get; set; }
    }
}