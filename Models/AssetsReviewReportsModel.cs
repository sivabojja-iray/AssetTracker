using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AssetsReviewReportsModel
    {
        public string EmployeeTeam { get; set; }
        public List<AssetReviewReportList> assetReviewReportLists { get; set; }
        public string AssetBelongsto { get; set; }
        public string EmployeeID { get; set; }
        public List<AssetTrackNowFromTeam> assetTrackNowFromTeam { get; set; }
    }
    public class AssetReviewReportList
    {
        public string AssignID { get; set; }
        public string EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string Team { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string Assetbelongsto { get; set; }
        public string AssetImage { get; set; }
        public string ReviewBy { get; set; }
        public string ReviewDate { get; set; }
        public string Remarks { get; set; }
        public bool ReportToQMS { get; set; }
    }
    public class AssetTrackNowFromTeam
    {
        public string EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string Team { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetImage { get; set; }
        public string Assetbelongsto { get; set; }
        public string ReviewBy { get; set; }
        public string ReviewDate { get; set; }
        public string Remarks { get; set; }

    }
}