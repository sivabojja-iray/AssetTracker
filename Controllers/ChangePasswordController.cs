﻿using I_RAY_ASSET_TRACKER_MVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace I_RAY_ASSET_TRACKER_MVC.Controllers
{
    public class ChangePasswordController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;
        // GET: ChangePassword
        public ActionResult Index(ChangePasswordModel changePasswordModel)
        {
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("select EmpID from EmployeeList where EmpID=@EmpID");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            con.Open();
            cmd.Parameters.AddWithValue("@EmpID", Session["username"]);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                changePasswordModel.EmployeeID = dr["EmpID"].ToString();
            }
            con.Close();
                return View(changePasswordModel);
        }
        public ActionResult PasswordChangedDetails(string NewPassword,string ConfirmPassword)
        {
            SqlConnection con = new SqlConnection(constr);
            string EncryptNEWPassword = Cryptography.Encrypt(NewPassword.ToString());
            string DecryptPwd = Cryptography.Decrypt(EncryptNEWPassword);
            string s = "update EmployeeRegistration1 set Password='" + EncryptNEWPassword + "',Decryptpwd='" + DecryptPwd + "' where UserName='" + Session["username"] + "'";
            con.Open();
            SqlCommand cmd = new SqlCommand(s, con);
            cmd.ExecuteNonQuery();
            con.Close();
            ViewBag.ConfirmMessage = "Password Change Successfull...";
            return Json(ViewBag.ConfirmMessage, JsonRequestBehavior.AllowGet);
        }
    }
}