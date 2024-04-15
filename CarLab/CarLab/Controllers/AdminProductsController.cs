using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using CarLab.DAL.Helpers;
using CarLab.DAL.Services;
using CarLab.Models.CommonEntities;
using CarLab.Models.DbEntities;

namespace VehiclesSell.Controllers
{
    [ServiceFilter(typeof(LoginAuthorization))]
    public class AdminProductsController : Controller
    {

        private readonly IBasicDataServices _basicDataServices;
        public AdminProductsController(IBasicDataServices basicDataServices)
        {
            this._basicDataServices = basicDataServices;
        }

        [HttpGet]
        public IActionResult ProductsList(int? ProductID, string ProductName, int? MakeID)
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "Cars List";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get products list
            model.productsList = _basicDataServices.GetProductsList(ProductID, ProductName, MakeID);

            //--get make list
            model.MakesList = _basicDataServices.GetMakesList();



            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")//if request is ajax
            {
                return PartialView("_ProductsListPartial", model);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ProductInsert()
        {

            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "Insert Car";
            #endregion

            BasicDataModel model = new BasicDataModel();

           
            model.ColorsList = _basicDataServices.GetColorsList();
            model.MakesList = _basicDataServices.GetMakesList();
            model.ModelsList = _basicDataServices.GetModelsList();

            model.CarsLocationsList = _basicDataServices.GetCarsLocationsList();
            model.EngineTypesList = _basicDataServices.GetEngineTypesList();
            model.TotalSeatsList = _basicDataServices.GetTotalSeatsList();
            model.FuelTypesList = _basicDataServices.GetFuelTypesList();
            model.TransmissionTypesList = _basicDataServices.GetTransmissionTypesList();

            ////--get categpories list
            //model.CategoriesList = _basicDataServices.GetProductCategories();



            return View(model);
        }

        [HttpPost]
        public IActionResult ProductInsert(Products FormData, int DataOperationType = (short)DataOperationType.Insert)
        {

            if (FormData == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return RedirectToAction("ProductInsert", "AdminProducts");
            }

            if (String.IsNullOrWhiteSpace(FormData.ProductName) || String.IsNullOrWhiteSpace(FormData.Description))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return RedirectToAction("ProductInsert", "AdminProducts");
            }

            if (FormData == null || FormData.photoFile == null)
            {
                TempData["ErrorMsg"] = "Please choose product image!";
                return RedirectToAction("ProductInsert", "AdminProducts");
            }


            BasicDataModel model = new BasicDataModel();

            #region image file conversion seciont
            if (FormData != null && FormData.photoFile != null)
            {
                string title = FormData.photoFile.FileName;
                MemoryStream ms = new MemoryStream();
                FormData.photoFile.CopyTo(ms);
                FormData.ProductImage = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            #endregion

            string result = _basicDataServices.InsertUpdateProduct(FormData, DataOperationType);

            if (result == "Saved Successfully!")
            {
                return RedirectToAction("ProductsList", "AdminProducts");
            }
            else
            {
                TempData["ErrorMsg"] = "An error occured. Please try again";
                return RedirectToAction("ProductInsert", "AdminProducts");
            }


        }


        [HttpGet]
        public IActionResult ProductUpdate(int ProductID)
        {

            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "Update Car";
            #endregion

            BasicDataModel model = new BasicDataModel();

            model.productsObj = _basicDataServices.GetProductDetailByID(ProductID);


            model.ColorsList = _basicDataServices.GetColorsList();
            model.MakesList = _basicDataServices.GetMakesList();
            model.ModelsList = _basicDataServices.GetModelsList();

            model.CarsLocationsList = _basicDataServices.GetCarsLocationsList();
            model.EngineTypesList = _basicDataServices.GetEngineTypesList();
            model.TotalSeatsList = _basicDataServices.GetTotalSeatsList();
            model.FuelTypesList = _basicDataServices.GetFuelTypesList();
            model.TransmissionTypesList = _basicDataServices.GetTransmissionTypesList();


            return View(model);



        }


        [HttpPost]
        public IActionResult ProductUpdate(Products FormData, int DataOperationType = (short)DataOperationType.Update)
        {
            BasicDataModel model = new BasicDataModel();

            if (FormData == null)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return RedirectToAction("ProductUpdate", "AdminProducts");
            }

            if (String.IsNullOrWhiteSpace(FormData.ProductName) || String.IsNullOrWhiteSpace(FormData.Description))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return RedirectToAction("ProductUpdate", "AdminProducts");
            }

            if (FormData.ProductID < 1)
            {
                TempData["ErrorMsg"] = "Invalid Product ID!";
                return RedirectToAction("ProductUpdate", "AdminProducts");
            }



            #region image file conversion seciont
            if (FormData != null && FormData.photoFile != null)
            {
                string title = FormData.photoFile.FileName;
                MemoryStream ms = new MemoryStream();
                FormData.photoFile.CopyTo(ms);
                FormData.ProductImage = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            #endregion


            string result = _basicDataServices.InsertUpdateProduct(FormData, DataOperationType);

            if (result == "Saved Successfully!")
            {
                return RedirectToAction("ProductsList", "AdminProducts");
            }
            else
            {
                TempData["ErrorMsg"] = "An error occured. Please try again!";
                return RedirectToAction("ProductUpdate", "AdminProducts", new { ProductID = FormData.ProductID });
            }

        }



        [HttpGet]
        public IActionResult OrderDetail(int OrderID)
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "Orders Detail";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get orders list
            model.ordersList = _basicDataServices.GetOrderDetailsList(OrderID);



            return View(model);
        }



        [HttpGet]
        public IActionResult OrdersList(int? OrderID, string CustomerEmail, int? StatusID)
        {
            #region bread crum setting
            ViewBag.BreadCrumTitle = "Admin";
            ViewBag.BreadCrumSubTitle = "Orders List";
            #endregion

            BasicDataModel model = new BasicDataModel();

            //--get orders list
            model.ordersList = _basicDataServices.GetOrdersList(OrderID, CustomerEmail, StatusID);

            //--get status list
            model.statusesList = _basicDataServices.GetStatusList();



            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")//if request is ajax
            {
                return PartialView("_OrdersListPartial", model);
            }

            return View(model);
        }


        [HttpPost]
        public IActionResult ChangeOrderStatus(int OrderID, int StatusID)
        {

            bool result = _basicDataServices.ChangeOrderStatus(OrderID, StatusID);
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
        public IActionResult GetModelByMakeID(int MakeID)
        {
            BasicDataModel model = new BasicDataModel();

         
            model.ModelsList = _basicDataServices.GetModelsList().Where(m=>m.MakeID==MakeID).ToList();
            return Json(new { modellist = model.ModelsList });
        }


    }
}
