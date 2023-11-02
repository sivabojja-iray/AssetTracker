using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class ChangePasswordModel
    {
        public string EmployeeID { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set;}
    }
}