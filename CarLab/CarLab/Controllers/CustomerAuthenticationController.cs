using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CarLab.DAL.Helpers;
using CarLab.DAL.Services;
using CarLab.Models;
using CarLab.Models.CommonEntities;
using CarLab.Models.DbEntities;

namespace CarLab.Controllers
{
    public class CustomerAuthenticationController : Controller
    {

        private readonly IBasicDataServices _basicDataServices;
        private readonly ICustomerServices _customerServices;
        private readonly IHttpContextAccessor _contx;
        private readonly ISessionManager _SessionManag;


        public CustomerAuthenticationController(IBasicDataServices basicDataServices, ISessionManager sessionManag, ICustomerServices customerServices, IHttpContextAccessor contx)
        {
            this._basicDataServices = basicDataServices;
            this._SessionManag = sessionManag;
            this._customerServices = customerServices;
            this._contx = contx;
        }


        public IActionResult LoginRegisterAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginJsonUser(string EmailAddress, string Password)
        {
            if (String.IsNullOrEmpty(EmailAddress) || String.IsNullOrEmpty(Password))
            {

                return Json(new { success = false, message = "Please fill both email & password!" });
            }

            Users usr = new Users();
            Password = _SessionManag.Encrypt(Password);
            usr = this._customerServices.CustomerLogin(EmailAddress, Password);

            if (usr != null)
            {
                if (usr.UserID > 0)
                {

                    //--set customer user data in session and cookies
                    this._SessionManag.SetCustomerUserDataInSession(usr);


                    return Json(new { success = true, message = "Login Successfully!" });
                }
                else
                {

                    return Json(new { success = false, message = "Incorrect email or password!" });
                }

            }
            else
            {
                return Json(new { success = false, message = "Incorrect email or password!" });
            }

        }





        [HttpPost]
        public IActionResult RegisterJsonUser( string FullName, string EmailAddress,  string Phone, string Password)
        {
            if (String.IsNullOrEmpty(FullName) || String.IsNullOrEmpty(EmailAddress) || String.IsNullOrEmpty(Password))
            {
                return Json(new { success = false, message = "Please fill full name , email, phone and password fields!" });
            }

            Users usr = new Users();
            Password = _SessionManag.Encrypt(Password);


            var registerUserResponse = this._customerServices.RegisterCustomerUser( FullName,  EmailAddress,  Phone,  Password);
            if (registerUserResponse==null && registerUserResponse.Response!= "Saved Successfully!")
            {
                return Json(new { success = false, message = "An error occured on server side. Please try again!" });
            }


            usr = this._customerServices.CustomerLogin(EmailAddress, Password);

            if (usr != null)
            {
                if (usr.UserID > 0)
                {

                    //--set customer user data in session and cookies
                    this._SessionManag.SetCustomerUserDataInSession(usr);


                    return Json(new { success = true, message = "Register Successfully!" });
                }
                else
                {

                    return Json(new { success = false, message = "An error occured!" });
                }

            }
            else
            {
                return Json(new { success = false, message = "An error occured!" });
            }

        }





        [HttpGet]
        public ActionResult LogoutCustomer()
        {

            string msg = RemoveSessionAndCookies();
            if (msg != "Removed Successfully!")
            {
                return RedirectToAction("LoginRegisterAccount", "CustomerAuthentication");
            }

            return RedirectToAction("LoginRegisterAccount", "CustomerAuthentication");
        }

        public string RemoveSessionAndCookies()
        {
            string result = "Removed Successfully!";
            HttpContext.Session.Clear();
            if (Request.Cookies != null)
            {
                try
                {
                    foreach (var cookie in Request.Cookies.Keys)
                    {
                        Response.Cookies.Delete(cookie);
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    result = "An error occured.";
                    return result;
                }

            }

            return result;

        }
    }
}
