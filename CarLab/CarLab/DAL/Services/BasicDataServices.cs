using CarLab.DAL.DBContext;
using CarLab.Models.DbEntities;
using PetaPoco;
using System.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CarLab.DAL.Services
{
    public class BasicDataServices : IBasicDataServices
    {

        private readonly IConfiguration _configuration;
        private readonly IDatabase _db;
        private readonly IDataContextHelper _contextHelper;


        //--Constructor of the class
        public BasicDataServices(IConfiguration configuration, IDatabase db, IDataContextHelper contextHelper)
        {
            _configuration = configuration;
            _db = db;
            _contextHelper = contextHelper;

        }


        public bool DeleteAnyRecord(string primarykeyValue, string primaryKeyColumn, string tableName)
        {
            bool result = false;

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    string deleteQuery = String.Format("DELETE TOP(1) FROM {0} WHERE {1}='{2}'", tableName, primaryKeyColumn, primarykeyValue);
                    repo.Execute(deleteQuery);
                    result = true;

                    return result;

                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }
        }

        public Users GetUserDataByUserID(int UserID)
        {
            Users result = new Users();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"TOP 1 *")

                        .From("Users usr")
                        .Where("USR.UserID=@1 AND USR.IsActive=1", UserID);


                    result = repo.Query<Users>(ppSql).FirstOrDefault();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public Users UserLogin(string EmailAddress, string Password)
        {
            Users result = new Users();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    repo.EnableAutoSelect = false;
                    result = repo.Fetch<Users>(@";EXEC [dbo].[SP_GetAdminUserLogin] @EmailAddress , @Password",
                        new { EmailAddress = EmailAddress, Password = Password }).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<Sizes> GetProductSizes()
        {
            List<Sizes> result = new List<Sizes>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"*")

                        .From("Sizes")
                        .OrderBy("SizeID ASC");



                    result = repo.Fetch<Sizes>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<Products> GetProductsList(int? ProductID, string ProductName, int? MakeID)
        {

            List<Products> result = new List<Products>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Products>(@";EXEC [dbo].[SP_GetProductsList] @ProductID , @ProductName , @MakeID",
                        new { ProductID = ProductID, ProductName = ProductName, MakeID = MakeID }).ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }


        public Products GetProductDetailByIDForWebsite(int ProductID)
        {

            Products result = new Products();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Products>(@";EXEC [dbo].[SP_GetProductDetailByIDForWebsite] @ProductID",
                        new { ProductID = ProductID}).FirstOrDefault();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }


        public string InsertUpdateProduct(Products model, int DataOperationType)
        {

            string result = " ";
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    // ProductName ,Description,	SizeID,	IsActive,	ShowOnHomePage,	DisplaySeqNo,	ProductImage

                    repo.EnableAutoSelect = false;

                    if (model != null)
                    {



                        repo.Execute(@";EXEC [dbo].[SP_InsertUpdateProduct] @ProductID, @ProductName, @Description, @SizeID, @CategoryID , @MakeID, @ModelID, @ColorID,@LocationID ,@EnginePowerID , @SeatNumberID ,@FuelTypeID, @TransmissionID ,  @ProductYear  , @IsActive, @ShowOnHomePage, @Price  , @DisplaySeqNo, @ProductImage, @DataOperationType",
                                          new
                                          {
                                              ProductID = model.ProductID,
                                              ProductName = model != null && !String.IsNullOrWhiteSpace(model.ProductName) ? model.ProductName.Trim() : "Product Name_" + DateTime.Now.ToString(),
                                              Description = model != null && !String.IsNullOrWhiteSpace(model.Description) ? model.Description.Trim() : "Product Desc" + DateTime.Now.ToString(),
                                              SizeID = model.SizeID,
                                              CategoryID = model.CategoryID,
                                              MakeID = model.MakeID,
                                              ModelID = model.ModelID,
                                              ColorID = model.ColorID,
                                              LocationID = model.LocationID,
                                              EnginePowerID = model.EnginePowerID,
                                              SeatNumberID = model.SeatNumberID,
                                              FuelTypeID = model.FuelTypeID,
                                              TransmissionID = model.TransmissionID,
                                              ProductYear = model.ProductYear,
                                              IsActive = model.IsActive,
                                              ShowOnHomePage = model.ShowOnHomePage,
                                              Price = model.Price,
                                              DisplaySeqNo = model.DisplaySeqNo,
                                              ProductImage = model.ProductImage != null ? model.ProductImage : new byte[1],
                                              DataOperationType = DataOperationType


                                          });


                        result = "Saved Successfully!";
                    }
                    else
                    {
                        result = "Product is null!";
                    }





                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;

                }

            }

        }

        public List<Orders> GetOrdersList(int? OrderID, string CustomerEmail, int? StatusID)
        {

            List<Orders> result = new List<Orders>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Orders>(@";EXEC [dbo].[SP_GetOrdersList] @OrderID , @CustomerEmail , @StatusID",
                        new { OrderID = OrderID, CustomerEmail = CustomerEmail, StatusID = StatusID }).ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }

        public List<Statuses> GetStatusList()
        {
            List<Statuses> result = new List<Statuses>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"*")

                        .From("Statuses")
                        .OrderBy("StatusID ASC");



                    result = repo.Fetch<Statuses>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public bool ChangeOrderStatus(int OrderID, int StatusID)
        {

            bool result = false;
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    repo.Execute(@";EXEC [dbo].[SP_UpdateOrderStatus] @OrderID, @StatusID",
                         new
                         {
                             OrderID = OrderID,
                             StatusID = StatusID

                         });

                    result = true;

                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }

        public List<Orders> GetOrderDetailsList(int OrderID)
        {

            List<Orders> result = new List<Orders>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Orders>(@";EXEC [dbo].[SP_GetOrderDetailsList] @OrderID",
                        new { OrderID = OrderID }).ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }

        public Products GetProductDetailByID(int ProductID)
        {
            Products result = new Products();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Products>(@";EXEC [dbo].[SP_GetProductDetailByID] @ProductID",
                        new { ProductID = ProductID }).FirstOrDefault();


                    return result;



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }


        public List<ContactUs> GetContactUsList(string CustomerName, string CustomerMessage)
        {

            List<ContactUs> result = new List<ContactUs>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<ContactUs>(@";EXEC [dbo].[SP_GetContactUsList] @CustomerName , @CustomerMessage",
                        new { CustomerName = CustomerName, CustomerMessage = CustomerMessage }).ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }


        public bool UpdateAdminReply(string AdminReply, int ContactUsID)
        {

            bool result = false;
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    repo.Execute(@";EXEC [dbo].[SP_UpdateAdminReply] @AdminReply, @ContactUsID",
                         new
                         {
                             AdminReply = AdminReply,
                             ContactUsID = ContactUsID

                         });

                    result = true;

                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }


        public List<Categories> GetProductCategories()
        {
            List<Categories> result = new List<Categories>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"top 4 *")

                        .From("Categories")
                        .OrderBy("CategoryID ASC");



                    result = repo.Fetch<Categories>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<Products> GetProductsListForWebsite(int MakeID, int ModelID , int LocationID, int ColorID )
        {

            List<Products> result = new List<Products>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Products>(@";EXEC [dbo].[SP_GetProductsListForWebsite] @MakeID , @ModelID , @LocationID, @ColorID ",
                        new { MakeID = MakeID, ModelID = ModelID, LocationID = LocationID, ColorID = ColorID }).ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }

        public Orders PlaceCustomerOrder(  int CustomerID  ,  string CustomerFullName, string CustomerEmail, string CustomerPhone, string CustomerAddress, string recordValueJson)
        {
            Orders result = new Orders();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<Orders>(@";EXEC [dbo].[SP_PlaceCustomerOrder]    @CustomerID , @CustomerFullName , @CustomerEmail,@CustomerPhone , @CustomerAddress , @recordValueJson",
                        new
                        {
                            CustomerID = CustomerID,
                            CustomerFullName = CustomerFullName,
                            CustomerEmail = CustomerEmail,
                            CustomerPhone = CustomerPhone,
                            CustomerAddress = CustomerAddress,
                            recordValueJson = recordValueJson,

                        }).FirstOrDefault();


                    return result;



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public bool ContactUsInsert(string CustomerName, string CustomerEmail, string CustomerPhone, string CustomerMessage)
        {

            bool result = false;
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    repo.Execute(@";EXEC [dbo].[SP_InsertCustomerContactUs] @CustomerName, @CustomerEmail ,@CustomerPhone  , @CustomerMessage",
                         new
                         {
                             CustomerName = CustomerName,
                             CustomerEmail = CustomerEmail,
                             CustomerPhone = CustomerPhone,
                             CustomerMessage = CustomerMessage

                         });

                    result = true;

                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }

        public DashboardData GetDashboardData()
        {

            DashboardData result = new DashboardData();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<DashboardData>(@";EXEC [dbo].[SP_GetDashboardData] ").FirstOrDefault();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }


        public List<DashboardData> GetDashboardOrdersCountByStatus()
        {

            List<DashboardData> result = new List<DashboardData>();
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    result = repo.Fetch<DashboardData>(@";EXEC [dbo].[SP_GetDashboardOrderStatusesCount] ").ToList();


                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }

            }

        }


        public List<Notifications> GetHeaderNotificationsList()
        {
            List<Notifications> result = new List<Notifications>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"top 30 *")

                        .From("Notifications")
                        .Where("IsReadByAdmin is null or IsReadByAdmin!=1")
                        .OrderBy("NotificationID desc");


                    result = repo.Fetch<Notifications>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<Colors> GetColorsList()
        {
            List<Colors> result = new List<Colors>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"*")

                        .From("Colors")
                        .OrderBy("ColorID DESC");



                    result = repo.Fetch<Colors>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public string SaveUpdateColors(Colors model, int DataOperationType)
        {

            string result = "";
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    repo.Execute(@";EXEC [dbo].[SaveUpdateColors] @ColorName , @ColorID, @DataOperationType",
                         new
                         {
                             ColorName = model.ColorName,
                             ColorID = model.ColorID,
                             DataOperationType = DataOperationType
                         });

                    result = "Saved Successfully";

                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;

                    return errorMsg;
                }

            }

        }

        public List<Makes> GetMakesList()
        {
            List<Makes> result = new List<Makes>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"*")

                        .From("Makes")
                        .OrderBy("MakeID DESC");



                    result = repo.Fetch<Makes>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public string SaveUpdateMakes(Makes model, int DataOperationType)
        {

            string result = "";
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    repo.Execute(@";EXEC [dbo].[SaveUpdateMakes] @MakeName , @MakeID, @DataOperationType",
                         new
                         {
                             MakeName = model.MakeName,
                             MakeID = model.MakeID,
                             DataOperationType = DataOperationType
                         });

                    result = "Saved Successfully";

                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;

                    return errorMsg;
                }

            }

        }

        public List<CarModels> GetModelsList()
        {
            List<CarModels> result = new List<CarModels>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"MD.*, MK.MakeName")

                        .From("Models MD")
                        .InnerJoin("Makes MK").On("MD.MakeID=MK.MakeID");


                    result = repo.Fetch<CarModels>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public string SaveUpdateCarModels(CarModels model, int DataOperationType)
        {

            string result = "";
            using (var repo = _contextHelper.GetPPContextHelper())
            {

                try
                {

                    repo.EnableAutoSelect = false;

                    repo.Execute(@";EXEC [dbo].[SaveUpdateModel] @ModelID , @MakeID, @ModelName , @DataOperationType",
                         new
                         {
                             ModelID = model.ModelID,
                             MakeID = model.MakeID,
                             ModelName = model.ModelName,
                             DataOperationType = DataOperationType
                         });

                    result = "Saved Successfully";

                    return result;
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;

                    return errorMsg;
                }

            }

        }

        public List<CarsLocations> GetCarsLocationsList()
        {
            List<CarsLocations> result = new List<CarsLocations>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"CRL.*")

                        .From("CarsLocations CRL");



                    result = repo.Fetch<CarsLocations>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<EngineTypes> GetEngineTypesList()
        {
            List<EngineTypes> result = new List<EngineTypes>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"ENG.*")

                        .From("EngineTypes ENG");



                    result = repo.Fetch<EngineTypes>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<TotalSeats> GetTotalSeatsList()
        {
            List<TotalSeats> result = new List<TotalSeats>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"TOTL.*")

                        .From("TotalSeats TOTL");



                    result = repo.Fetch<TotalSeats>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<FuelTypes> GetFuelTypesList()
        {
            List<FuelTypes> result = new List<FuelTypes>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"FUT.*")

                        .From("FuelTypes FUT");



                    result = repo.Fetch<FuelTypes>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }

        public List<TransmissionTypes> GetTransmissionTypesList()
        {
            List<TransmissionTypes> result = new List<TransmissionTypes>();

            using (var repo = _contextHelper.GetPPContextHelper())
            {
                try
                {

                    var ppSql = PetaPoco.Sql.Builder.Select(@"TRNS.*")

                        .From("TransmissionTypes TRNS");



                    result = repo.Fetch<TransmissionTypes>(ppSql).ToList();



                }
                catch (Exception ex)
                {
                    string errorMsg = ex.Message;
                    return result;
                }


            }

            return result;
        }
    }

    public interface IBasicDataServices
    {
        public Users GetUserDataByUserID(int UserID);
        public bool DeleteAnyRecord(string primarykeyValue, string primaryKeyColumn, string tableName);
        public Users UserLogin(string EmailAddress, string Password);
        public List<Sizes> GetProductSizes();
        public List<Products> GetProductsList(int? ProductID, string ProductName, int? MakeID);
        public string InsertUpdateProduct(Products model, int DataOperationType);
        public List<Orders> GetOrdersList(int? OrderID, string CustomerEmail, int? StatusID);
        public List<Statuses> GetStatusList();
        public bool ChangeOrderStatus(int OrderID, int StatusID);
        public List<Orders> GetOrderDetailsList(int OrderID);
        public Products GetProductDetailByID(int ProductID);
        public List<ContactUs> GetContactUsList(string CustomerName, string CustomerMessage);
        public bool UpdateAdminReply(string AdminReply, int ContactUsID);
        public List<Categories> GetProductCategories();

        public List<Products> GetProductsListForWebsite(int MakeID, int ModelID, int LocationID, int ColorID);
        public Orders PlaceCustomerOrder(int CustomerID,  string CustomerFullName, string CustomerEmail, string CustomerPhone, string CustomerAddress, string recordValueJson);

        public bool ContactUsInsert(string CustomerName, string CustomerEmail, string CustomerPhone, string CustomerMessage);
        public DashboardData GetDashboardData();
        public List<DashboardData> GetDashboardOrdersCountByStatus();
        public List<Notifications> GetHeaderNotificationsList();
        public List<Colors> GetColorsList();
        public string SaveUpdateColors(Colors model, int DataOperationType);
        public List<Makes> GetMakesList();
        public string SaveUpdateMakes(Makes model, int DataOperationType);
        public List<CarModels> GetModelsList();

        public string SaveUpdateCarModels(CarModels model, int DataOperationType);
        public List<CarsLocations> GetCarsLocationsList();
        public List<EngineTypes> GetEngineTypesList();
        public List<TotalSeats> GetTotalSeatsList();
        public List<FuelTypes> GetFuelTypesList();
        public List<TransmissionTypes> GetTransmissionTypesList();

        public Products GetProductDetailByIDForWebsite(int ProductID);

    }
}
