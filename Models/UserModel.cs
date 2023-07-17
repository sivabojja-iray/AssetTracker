using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class UserModel
    {
        public IPagedList<Table1Model> Table1Data { get; set; }
        public IPagedList<Table2Model> Table2Data { get; set; }
    }
    public class Table1Model
    {
        public string AssignID { get; set; }
        public string EmployeeName { get; set; }
        public string Assetbelongsto { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssignDate { get; set; }
        public string ExpectedReturnDate { get; set; }
        public string AssetTransferTeam { get; set; }
        public string AssetTransferEmployeeName { get; set; }
    }
    public class Table2Model
    {
        public string FromEmployeeName { get; set; }
        public string Assetbelongsto { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssetTransferDate { get; set; }
        public string FromEmpID { get; set; }
        public string ToEmpID { get; set; }
        public string TeamToWhichAssetisTransfered { get; set; }
        public string ToEmployeeName { get; set; }
    }
}