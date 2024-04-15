using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using BGAuto.DAL.Helpers;
using BGAuto.DAL.Services;
using BGAuto.Models.DbEntities;

namespace VehiclesSell.Controllers
{
    public class AdminAuthenticationController : Controller
    {

        private readonly IBasicDataServices _basicDataServices;
        private readonly ISessionManager _SessionManag;
        private readonly IHttpContextAccessor _contx;
        public AdminAuthenticationController(IBasicDataServices basicDataServices, ISessionManager sessionManag, IHttpContextAccessor contx)
        {
            this._basicDataServices = basicDataServices;
            this._SessionManag = sessionManag;
            this._contx = contx;
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginJson(string EmailAddress, string Password)
        {
            if (String.IsNullOrEmpty(EmailAddress) || String.IsNullOrEmpty(EmailAddress))
            {

                return Json(new { success = false, message = "Please fill both user name & password!" });
            }

            Users usr = new Users();
            Password = _SessionManag.Encrypt(Password);
            usr = this._basicDataServices.UserLogin(EmailAddress, Password);

            if (usr != null)
            {
                if (usr.UserID > 0)
                {

                    //--set user data in session and cookies
                    this._SessionManag.SetUserDataInSession(usr);


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

        [HttpGet]
        public ActionResult Logout()
        {

            string msg = RemoveSessionAndCookies();
            if (msg != "Removed Successfully!")
            {
                return RedirectToAction("Login", "AdminAuthentication");
            }

            return RedirectToAction("Login", "AdminAuthentication");
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
