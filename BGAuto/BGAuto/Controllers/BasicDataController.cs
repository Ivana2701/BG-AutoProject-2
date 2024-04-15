using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using BGAuto.DAL.Helpers;
using BGAuto.DAL.Services;
using BGAuto.Models.CommonEntities;
using BGAuto.Models.DbEntities;


namespace VehiclesSell.Controllers
{
    [ServiceFilter(typeof(LoginAuthorization))]
    public class BasicDataController : Controller
    {

        private readonly IBasicDataServices _basicDataServices;
        private readonly ISessionManager _sessionManager;
        public BasicDataController(IBasicDataServices basicDataServices, ISessionManager sessionManager)
        {
            this._basicDataServices = basicDataServices;
            this._sessionManager = sessionManager;
        }



        [HttpGet]
        public IActionResult Colors()
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Basic Data";
            ViewBag.BreadCrumSubTitle = "Colors";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get colors list
            model.ColorsList = _basicDataServices.GetColorsList();

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")//if request is ajax
            {
                return PartialView("_ColorsPartial", model);
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult SaveUpdateColors(Colors model, int DataOperationType = (short)DataOperationType.Insert)
        {


            if (String.IsNullOrWhiteSpace(model.ColorName) )
            {
                return Json(new { success = false, message = "Please fill all fields." });
            }

            string result = _basicDataServices.SaveUpdateColors(model, DataOperationType);
            if (!String.IsNullOrWhiteSpace(result) && result == "Saved Successfully")
            {
                return Json(new { success = true, message = "Saved Successfully!" });
            }
            else
            {
                return Json(new { success = false, message = result });
            }
        }

        [HttpGet]
        public IActionResult Makes()
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Basic Data";
            ViewBag.BreadCrumSubTitle = "Makes (Brands)";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get colors list
            model.MakesList = _basicDataServices.GetMakesList();

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")//if request is ajax
            {
                return PartialView("_MakesPartial", model);
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult DeleteAnyRecord(string primarykeyValue, string primaryKeyColumn, string tableName)
        {


            bool result = _basicDataServices.DeleteAnyRecord(primarykeyValue, primaryKeyColumn, tableName);
            if (result)
            {
                return Json(new { success = true, message = "Deleted Successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "An error occured on server side." });
            }


        }


        [HttpPost]
        public IActionResult SaveUpdateMakes(Makes model, int DataOperationType = (short)DataOperationType.Insert)
        {


            if (String.IsNullOrWhiteSpace(model.MakeName))
            {
                return Json(new { success = false, message = "Please fill all fields." });
            }

            string result = _basicDataServices.SaveUpdateMakes(model, DataOperationType);
            if (!String.IsNullOrWhiteSpace(result) && result == "Saved Successfully")
            {
                return Json(new { success = true, message = "Saved Successfully!" });
            }
            else
            {
                return Json(new { success = false, message = result });
            }
        }


        [HttpGet]
        public IActionResult Models()
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Basic Data";
            ViewBag.BreadCrumSubTitle = "Models";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get models list
            model.ModelsList = _basicDataServices.GetModelsList();
            model.MakesList = _basicDataServices.GetMakesList();

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")//if request is ajax
            {
                return PartialView("_ModelsPartial", model);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveUpdateCarModel(CarModels model, int DataOperationType = (short)DataOperationType.Insert)
        {


            if (String.IsNullOrWhiteSpace(model.ModelName))
            {
                return Json(new { success = false, message = "Please fill all fields." });
            }

            string result = _basicDataServices.SaveUpdateCarModels(model, DataOperationType);
            if (!String.IsNullOrWhiteSpace(result) && result == "Saved Successfully")
            {
                return Json(new { success = true, message = "Saved Successfully!" });
            }
            else
            {
                return Json(new { success = false, message = result });
            }
        }


    }
}
