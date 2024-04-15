using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using CarLab.DAL.Helpers;
using CarLab.DAL.Services;
using CarLab.Models.CommonEntities;
using CarLab.Models.DbEntities;

namespace VehiclesSell.Controllers
{
    [ServiceFilter(typeof(LoginAuthorization))]
    public class AdminHomeController : Controller
    {

        private readonly IBasicDataServices _basicDataServices;
        private readonly ISessionManager _sessionManager;
        public AdminHomeController(IBasicDataServices basicDataServices, ISessionManager sessionManager)
        {
            this._basicDataServices = basicDataServices;
            this._sessionManager = sessionManager;
        }


        public IActionResult Index()
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "Dash Board";
            #endregion

            BasicDataModel model = new BasicDataModel();
            model.DashboardDataObj = _basicDataServices.GetDashboardData();
            model.DashboardDataOrderStatusList = _basicDataServices.GetDashboardOrdersCountByStatus();

            return View(model);
        }

        [HttpGet]
        public IActionResult ContactUsList(string CustomerName, string CustomerMessage)
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "ContactUs List";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get contact list
            model.ContactUsList = _basicDataServices.GetContactUsList(CustomerName, CustomerMessage);


            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")//if request is ajax
            {
                return PartialView("_ContactUsListtPartial", model);
            }

            return View(model);
        }

        [HttpPost]

        public IActionResult UpdateAdminReply(string AdminReply, int ContactUsID)
        {

            bool result = _basicDataServices.UpdateAdminReply(AdminReply, ContactUsID);
            if (result)
            {
                return Json(new { success = true, message = "Saved Successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "An error occured on server side." });
            }
        }


        [HttpGet]
        public IActionResult GetHeaderNotificationsList()
        {
            BasicDataModel model = new BasicDataModel();


            model.NotificationsList = _basicDataServices.GetHeaderNotificationsList();

            return PartialView("_AdminHeaderNotifications", model);
        }

        public IActionResult UserProfile()
        {

            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "User Profile";
            #endregion

            BasicDataModel model = new BasicDataModel();



            //--get user data from session
            model.userObj = _sessionManager.GetLoginUserFromSession();

            return View(model);
        }

    }
}
