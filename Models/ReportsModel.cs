using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class ReportsModel
    {
        public string EmployeeID { get; set; }
        public string AssetSerialNumber { get; set; }
        public string EmployeeName { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public List<SearchData> searchData { get; set; }
        
    }
    public class SearchData
    {
        public string AssignID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Team { get; set; }
        public string AssetBelongsTo { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumber { get; set; }
        public string AssignDate { get; set; }
        public string ExpectedDate { get; set; }
        public string ActualReturnDate { get; set; }
    }
    public class AssetTransferRecords
    {
        public string FromEmpID { get; set; }
        public string FromEmployeeName { get; set; }
        public string Assetbelongsto { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetTransferDate { get; set; }
        public string ToEmpID { get; set; }
        public string TeamToWhichAssetisTransfered { get; set; }
        public string ToEmployeeName { get; set; }
    }
}