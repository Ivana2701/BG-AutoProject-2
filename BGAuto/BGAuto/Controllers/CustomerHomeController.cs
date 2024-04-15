using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using BGAuto.DAL.Helpers;
using BGAuto.DAL.Services;
using BGAuto.Models;
using BGAuto.Models.CommonEntities;
using BGAuto.Models.DbEntities;



namespace VehiclesSell.Controllers
{
    public class CustomerHomeController : Controller
    {

        private readonly IBasicDataServices _basicDataServices;
        private readonly ICustomerServices _customerServices;
        private readonly IHttpContextAccessor _contx;
        public CustomerHomeController(IBasicDataServices basicDataServices, ICustomerServices customerServices, IHttpContextAccessor contx)
        {
            this._basicDataServices = basicDataServices;
            this._customerServices = customerServices;
            this._contx = contx;
        }


        public IActionResult Index()
        {
            BasicDataModel model = new BasicDataModel();

            ////--get make list
            //model.MakesList = _basicDataServices.GetMakesList().Take(10).ToList();

            ////--get color list
            //model.ColorsList = _basicDataServices.GetColorsList().Take(10).ToList();

            ////--get product list
            //model.productsList = _basicDataServices.GetProductsListForWebsite(0, 0).Where(s => s.ShowOnHomePage == true).ToList();



            model.ColorsList = _basicDataServices.GetColorsList();
            model.MakesList = _basicDataServices.GetMakesList();
            model.CarsLocationsList = _basicDataServices.GetCarsLocationsList();


            return View(model);
           
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

       


     


        [HttpGet]
        public IActionResult GetProductsListByFilter(int MakeID=0 , int ModelID = 0, int LocationID = 0, int ColorID = 0)
        {
            BasicDataModel model = new BasicDataModel();

            //--get product list
            model.productsList = _basicDataServices.GetProductsListForWebsite(MakeID, ModelID , LocationID, ColorID).ToList();

            return PartialView("_ProductsHomePageList", model);
        }


        [HttpGet]
        public IActionResult GetProductsByColorIDHomePage(int ColorID)
        {
            BasicDataModel model = new BasicDataModel();

            //--get product list
            model.productsList = _basicDataServices.GetProductsListForWebsite(0, 0 , 0 , 0).Where(s => s.ColorID==ColorID).ToList();

            return PartialView("_ProductsHomePageList", model);
        }

        [HttpGet]
        public IActionResult GetProductDetail(int ProductID)
        {
            BasicDataModel model = new BasicDataModel();

            //--get product list
            model.productsObj = _basicDataServices.GetProductDetailByIDForWebsite(ProductID);

            return PartialView("_ProductDetailPartial", model);
        }


        [HttpGet]
        public IActionResult AddItemToCart(int ProductID)
        {
            BasicDataModel model = new BasicDataModel();

            List<Products> UserCart = new List<Products>();


            var cartString = _contx.HttpContext.Session.GetString("UserCart");
            if (!String.IsNullOrEmpty(cartString))
            {
                UserCart = JsonConvert.DeserializeObject<List<Products>>(cartString);
            }

            //--check if item already exists in cart
            if (UserCart != null && UserCart.Any(c => c.ProductID == ProductID))
            {
                return Json(new { success = false, message = "Item already exists in your list!" });
            }


            model.productsObj = _basicDataServices.GetProductDetailByIDForWebsite(ProductID);

            if (model.productsObj == null)
            {
                return Json(new { success = false, message = "This item no more exists!" });
            }
            else
            {
                UserCart.Add(model.productsObj);
            }




            var carSerialize = JsonConvert.SerializeObject(UserCart);
            _contx.HttpContext.Session.SetString("UserCart", carSerialize);


            return Json(new { success = true, message = "Added Successfully!", totalCartItems = UserCart.Count });
        }


        [HttpGet]
        public IActionResult CartItems()
        {


            BasicDataModel model = new BasicDataModel();


            var cartString = _contx.HttpContext.Session.GetString("UserCart");
            if (!String.IsNullOrEmpty(cartString))
            {
                model.productsList = JsonConvert.DeserializeObject<List<Products>>(cartString);
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult RemoveItem(int ProductID)
        {
            BasicDataModel model = new BasicDataModel();

            List<Products> UserCart = new List<Products>();


            var cartString = _contx.HttpContext.Session.GetString("UserCart");
            if (!String.IsNullOrEmpty(cartString))
            {
                UserCart = JsonConvert.DeserializeObject<List<Products>>(cartString);
            }

            //--check if item already exists in cart
            if (UserCart != null && UserCart.Any(c => c.ProductID == ProductID))
            {
                UserCart.RemoveAll(i => i.ProductID == ProductID);

            }




            var carSerialize = JsonConvert.SerializeObject(UserCart);
            _contx.HttpContext.Session.SetString("UserCart", carSerialize);


            return Json(new { success = true, message = "Removed Successfully!" });
        }



        [HttpPost]

        public IActionResult PlaceOrder(  int CustomerID, string CustomerFullName, string CustomerEmail, string CustomerPhone, string CustomerAddress)
        {
            BasicDataModel model = new BasicDataModel();


            if (CustomerID<1)
            {
                return Json(new { success = false, message = "Customer ID is null!" });
            }

            if (String.IsNullOrWhiteSpace(CustomerFullName) || String.IsNullOrWhiteSpace(CustomerEmail) || String.IsNullOrWhiteSpace(CustomerPhone) || String.IsNullOrWhiteSpace(CustomerAddress))
            {
                return Json(new { success = false, message = "Please fill all fields of Customer Info Form!" });
            }

            var cartString = _contx.HttpContext.Session.GetString("UserCart");
            if (String.IsNullOrEmpty(cartString))
            {
                return Json(new { success = false, message = "No item found in your cart!" });
            }


            if (!String.IsNullOrEmpty(cartString))
            {
                model.productsList = JsonConvert.DeserializeObject<List<Products>>(cartString);
                var recordValueJson = JsonConvert.SerializeObject(model.productsList);

                //--place order
                model.orderObj = _basicDataServices.PlaceCustomerOrder( CustomerID, CustomerFullName, CustomerEmail, CustomerPhone, CustomerAddress, recordValueJson);

                if (model.orderObj != null && model.orderObj.ResponseMsg != null && model.orderObj.ResponseMsg == "Saved Successfully!")
                {
                    //--remove all items from cart
                    _contx.HttpContext.Session.SetString("UserCart", "");

                    return Json(new { success = true, message = "Saved Successfully!", totalCartItems = 0, orderdetail = model.orderObj });
                }
                else
                {
                    return Json(new { success = false, message = "An error occured in placement of your order. Please try again!" });
                }


            }
            else
            {
                return Json(new { success = false, message = "No item found in your cart!" });
            }



        }



        [HttpPost]

        public IActionResult ContactUsInsert(string CustomerName, string CustomerEmail, string CustomerPhone, string CustomerMessage)
        {
            BasicDataModel model = new BasicDataModel();


            if (String.IsNullOrWhiteSpace(CustomerName) || String.IsNullOrWhiteSpace(CustomerEmail) || String.IsNullOrWhiteSpace(CustomerPhone) || String.IsNullOrWhiteSpace(CustomerMessage))
            {
                return Json(new { success = false, message = "Please fill all fields of Customer Info Form!" });
            }

            var restult = _basicDataServices.ContactUsInsert(CustomerName, CustomerEmail, CustomerPhone, CustomerMessage);
            if (restult)
            {
                return Json(new { success = true, message = "Saved Successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "No item found in your cart!" });
            }


        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult CarsList(int MakeID=0, int ModelID=0, int LocationID=0 ,int ColorID=0)
        {
            BasicDataModel model = new BasicDataModel();

            ViewBag.MakeID = MakeID;
            ViewBag.ModelID = ModelID;
            ViewBag.LocationID = LocationID;
            ViewBag.ColorID = ColorID;


            model.ColorsList = _basicDataServices.GetColorsList().Take(10).ToList();
            model.MakesList = _basicDataServices.GetMakesList().Take(10).ToList();
            model.CarsLocationsList = _basicDataServices.GetCarsLocationsList().Take(10).ToList();

            model.productsList = _basicDataServices.GetProductsListForWebsite(MakeID, ModelID, LocationID, ColorID).Where(s => s.ShowOnHomePage == true).ToList();


            return View(model);

        }




        [HttpGet]
        public IActionResult CarsCustomerBookingList(int CustomerID)
        {
            BasicDataModel model = new BasicDataModel();

           
          //  model.ColorsList = _basicDataServices.GetColorsList().Take(10).ToList();
            model.MakesList = _basicDataServices.GetMakesList().Take(10).ToList();
           // model.CarsLocationsList = _basicDataServices.GetCarsLocationsList().Take(10).ToList();

            model.productsList = _customerServices.GetCustomerOrderBookingList(CustomerID).ToList();


            return View(model);

        }




    }
}
