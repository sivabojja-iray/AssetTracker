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
using System.Drawing;
using System.Net.Mail;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class UserLoginController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        string myregisteredpwd;
        string mymail;
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

                SqlCommand cmd5 = new SqlCommand("select EmployeeName from EmployeeList where EmpID='" + lc.UserName + "'", sqlConnection);
                string n = (string)(cmd5.ExecuteScalar());
                Session["EmployeeName"] = n;

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
        public ActionResult forgotPassword()
        {
            return PartialView("forgotPassword");  
        }
        public JsonResult passwordSendtoMail(LoginModelClass loginModelClass)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select count(*) from EmployeeRegistration1 where  UserName='" + loginModelClass.EmployeeID + "' ", sqlConnection);
            sqlConnection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            sqlConnection.Close();
            if (i > 0)
            {
                sqlConnection.Open();
                SqlCommand cmd1 = new SqlCommand("select Mail from EmployeeList where EmpID=" + loginModelClass.EmployeeID + "", sqlConnection);
                SqlDataReader dr2 = cmd1.ExecuteReader();
                if (dr2.Read())
                {
                    mymail = dr2["Mail"].ToString();
                    Session["Mymail"] = mymail;
                }
                sqlConnection.Close();
                sqlConnection.Open();
                SqlCommand cmd2 = new SqlCommand("select Decryptpwd from EmployeeRegistration1 where UserName=" + loginModelClass.EmployeeID + "", sqlConnection);
                SqlDataReader dr3 = cmd2.ExecuteReader();
                if (dr3.Read())
                {
                    myregisteredpwd = dr3["Decryptpwd"].ToString();
                }
                sqlConnection.Close();
                if (myregisteredpwd != null)
                {
                    MailMessage mail = new MailMessage("assettracker@i-raysolutions.com", Session["Mymail"].ToString(), "AssetTracker Password", "Your Registered Password : " + myregisteredpwd + "");
                    mail.Priority = MailPriority.High;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Send(mail);
                    ViewBag.PasswordSenttoMail = "Password Sent to your Mail";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please Register...";
            }
            return Json(loginModelClass);
        }
    }
}