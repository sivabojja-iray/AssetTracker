using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class UserLoginController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: UserLogin
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModelClass lc)
        {
            if (string.IsNullOrEmpty(lc.UserName))
            {
                ModelState.AddModelError("UserName", "Please Enter The Username !");
            }
            if(string.IsNullOrEmpty(lc.Password))
            {
                ModelState.AddModelError("Password", "Please Enter The Password !");
            }

            if(ModelState.IsValid)
            {
                string EncryptPWD = Cryptography.Encrypt(lc.Password);
                SqlConnection sqlConnection = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("select count(*) from EmployeeRegistration1 where UserName='" + lc.UserName + "' and Password='" + EncryptPWD + "'and (Role='User' OR Role='' OR Role=NULL)");
                sqlConnection.Open();
                cmd.Connection = sqlConnection;
                int i = Convert.ToInt32(cmd.ExecuteScalar());
                SqlCommand cmd1 = new SqlCommand("select count(*) from EmployeeRegistration1 where UserName='" + lc.UserName + "' and Password='" + EncryptPWD + "'and Role= 'AssetManager'", sqlConnection);
                int j = Convert.ToInt32(cmd1.ExecuteScalar());
                SqlCommand cmd2 = new SqlCommand("select count(*) from EmployeeRegistration1 where UserName='" + lc.UserName + "' and Password='" + EncryptPWD + "'and Role= 'Manager'", sqlConnection);
                int k = Convert.ToInt32(cmd2.ExecuteScalar());
                SqlCommand cmd3 = new SqlCommand("select count(*) from EmployeeRegistration1 where UserName='" + lc.UserName + "' and Password='" + EncryptPWD + "'and Role= 'Admin'", sqlConnection);
                int l = Convert.ToInt32(cmd3.ExecuteScalar());
                FormsAuthentication.SetAuthCookie(lc.UserName, true);
                Session["username"] = lc.UserName.ToString();

                SqlCommand cmd4 = new SqlCommand("select Role from EmployeeRegistration1 where UserName='" + lc.UserName + "'", sqlConnection);
                string m = (string)(cmd4.ExecuteScalar());
                Session["Role"] = m;

                if (i > 0)
                {
                    return Redirect("~/User/Index");
                }
                else if (j > 0)
                {
                    return Redirect("~/Admin/Index");
                }
                else if (k > 0)
                {
                    return Redirect("~/Admin/Index");
                }
                else if (l > 0)
                {
                    return Redirect("~/Admin/Index");
                }
                else
                {
                    ViewData["message"] = "Login Details Failed";
                }
                sqlConnection.Close();
            }

            return View(lc);
          
        }
    }
}