using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AdminModel
    {
        public List<AdminTable1Model> adminTable1Model { get; set; }
        public List<AdminTable2Model> adminTable2Model { get; set; }
        public List<adminNotification> adminNotifications { get; set; }
        public List<Notifation> adminNotificationManager { get; set; }
    }
    public class AdminTable1Model
    {
        public string AssignID { get; set; }
        public string EmpID { get; set; }
        public string Team { get; set; }
        public string InvoiceNo { get; set; }
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
    public class AdminTable2Model
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
    public class adminNotification
    {
        public string EmpName { get; set; }
        public string EmpTeam { get; set; }
        public string RequestDate { get; set; }
    }
}