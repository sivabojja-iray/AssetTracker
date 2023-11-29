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
        [Authorize]
        public ActionResult Login(LoginModelClass lc,FormCollection formCollection)
        {
            string submitButtonValue = formCollection["submitButton"];
            switch (submitButtonValue)
            {
                case "Login":
                    if (string.IsNullOrEmpty(lc.UserName))
                    {
                        ModelState.AddModelError("UserName", "Please Enter The Username !");
                    }
                    if (string.IsNullOrEmpty(lc.Password))
                    {
                        ModelState.AddModelError("Password", "Please Enter The Password !");
                    }
                    if (ModelState.IsValid)
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
                            return RedirectToAction("Index", "User");
                        }
                        else if (j > 0)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (k > 0)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else if (l > 0)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            ViewData["message"] = "Login Details Failed";
                        }
                        sqlConnection.Close();
                    }
                    return View(lc);
                case "Register":
                    if (ModelState.IsValid)
                    {
                        SqlConnection sqlConnection = new SqlConnection(constr);
                        sqlConnection.Open();
                        SqlCommand cmd = new SqlCommand("select count(*) from EmployeeList where EmpID='" + lc.EmployeeID + "'", sqlConnection);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        SqlCommand cmd3 = new SqlCommand("select count(*) from EmployeeRegistration1 where UserName='" + lc.EmployeeID + "'", sqlConnection);
                        int count2 = Convert.ToInt32(cmd3.ExecuteScalar());
                        sqlConnection.Close();
                        if (count > 0 && count2 <= 0)
                        {
                            try
                            {
                                sqlConnection.Open();
                                string UserName = lc.EmployeeID;
                                string Password = Cryptography.Encrypt(lc.RegisterPassword.ToString());
                                string DecryptPwd = Cryptography.Decrypt(Password);
                                string str = "insert into EmployeeRegistration1 values('" + UserName + "','" + Password + "','" + DecryptPwd + "','" + lc.Role + "')";
                                SqlCommand cmd2 = new SqlCommand(str, sqlConnection);
                                cmd2.ExecuteNonQuery();
                                sqlConnection.Close();
                                ViewData["RegisterMessage"] = "Registered Sucessfully";
                            }
                            catch (Exception ex)
                            {
                                string excep = ex.Message;
                            }
                        }
                        else if (count == 0)
                        {
                            ViewData["RegisterMessage"] = "Please Enter Valid Employee ID";
                        }
                        else if (count2 > 0)
                        {
                            ViewData["RegisterMessage"] = "EmployeeID already exists";
                        }
                        else
                        {
                            ViewData["RegisterMessage"] = "Try Again";
                        }
                    }
                        return View(lc);
            }         
            return View(lc);          
        }
        public ActionResult forgotPassword()
        {
            return PartialView("forgotPassword");  
        }
        public JsonResult passwordSendtoMail(string PasswordEmployeeID)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select count(*) from EmployeeRegistration1 where  UserName='" + PasswordEmployeeID + "' ", sqlConnection);
            sqlConnection.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            sqlConnection.Close();
            if (i > 0)
            {
                sqlConnection.Open();
                SqlCommand cmd1 = new SqlCommand("select Mail from EmployeeList where EmpID=" + PasswordEmployeeID + "", sqlConnection);
                SqlDataReader dr2 = cmd1.ExecuteReader();
                if (dr2.Read())
                {
                    mymail = dr2["Mail"].ToString();
                    Session["Mymail"] = mymail;
                }
                sqlConnection.Close();
                sqlConnection.Open();
                SqlCommand cmd2 = new SqlCommand("select Decryptpwd from EmployeeRegistration1 where UserName=" + PasswordEmployeeID + "", sqlConnection);
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
                    ViewBag.ErrorMessage = "Password Sent to your Mail";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please Register...";
            }
            return Json(ViewBag.ErrorMessage,JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChangeEmployeeID(string newValue) 
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select Role from EmployeeList where EmpID=@EmpID", sqlConnection);
            cmd.CommandType = CommandType.Text;
            sqlConnection.Open();
            cmd.Parameters.AddWithValue("@EmpID", newValue);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                if (!string.IsNullOrEmpty(Convert.ToString(dr["Role"])))
                    ViewBag.Role = Convert.ToString(dr["Role"]);
                else
                    ViewBag.Role = "Employee";
            }
            else
            {
                ViewBag.Role = "User Not Exit";
            }
            sqlConnection.Close();
            return Json(ViewBag.Role,JsonRequestBehavior.AllowGet); 
        }
        public ActionResult Signout()
        {
            Session.Clear();
            return RedirectToAction("Login", "UserLogin");
        }
    }
}