﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AllocationListModel
    {
        public string EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string Team { get; set; }
        public string AssetType { get; set; }
        public string HWSWName { get; set; }
        public string SerialNumberVersionNumber { get; set; }
        public string AssignDate { get; set; }
        public string ExpectedReturnDate { get; set; }
        public string Assetbelongsto { get; set; }
        public string InvoiceNo { get; set; }

    }
}