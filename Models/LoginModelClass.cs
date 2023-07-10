using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class LoginModelClass
    {
        [Required(ErrorMessage = "Please Enter The Username !")]
        //[Display(Name ="Enter UserId :")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter The Password !")]
        //[Display(Name = "Enter Password :")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get;set; }
    }
}