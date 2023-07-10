using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AssignAssetApproval
    {
        public string AllocationType { get; set; }
        public bool DailyAllocation { get; set; }
        public bool PermanentAllocation { get;set; }
        //public bool Allocation { get; set; }
        public string RequestID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeTeam { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string Quantity { get; set; }
        public string Assetbelongsto { get; set; }
        public string AssignDate { get; set; }
        public string ReturnDate { get; set; }
        public string SerialNumber { get; set; }
        public string Comments { get; set; }
        public string invoicenumber { get; set; }
    }
}