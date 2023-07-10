using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AssetRequestModal
    {
        public string EmployeeTeam { get; set; }
        public string EmployeeName { get; set; }
        public string RequestRaisedTo { get; set; }
        public string AssetType { get; set; }
        public string AssetName { get; set; }
        public string Available { get; set; }
        public string AvailableSerialNumber { get; set; }
        public string NumberofAssetsRequired { get; set; }
        public string RequestDate { get; set; }
        public string ExpectedReturnDate { get; set; }
        public string Purpose { get; set; }      
    }
}