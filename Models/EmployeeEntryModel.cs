using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class EmployeeEntryModel
    {
        public string EmployeeID { get; set; }    
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Team { get; set; }
        public string Role { get; set; }
        public string Mail { get; set; }
        public string ContactNumber { get; set; }
        public bool OldEmployeeID { get; set; }
        public bool NewEmployeeID { get; set; }
    }
}