using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class LoginModelClass
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string PasswordEmployeeID { get; set; }
        public string EmployeeID { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [DataType(DataType.Password)]
        public string RegisterPassword { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}