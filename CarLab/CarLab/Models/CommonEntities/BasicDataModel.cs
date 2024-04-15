using System.Collections.Generic;
using CarLab.Models.DbEntities;

namespace CarLab.Models.CommonEntities
{
    public class BasicDataModel
    {
        public Products productsObj { get; set; }
        public List<Products> productsList { get; set; }
        public Sizes sizesObj { get; set; }
        public DashboardData DashboardDataObj { get; set; }
        public List<DashboardData> DashboardDataOrderStatusList { get; set; }
        public List<Sizes> sizesList { get; set; }
        public List<Categories> CategoriesList { get; set; }
        public List<Orders> ordersList { get; set; }
        public Orders orderObj { get; set; }
        public List<Statuses> statusesList { get; set; }
        public List<ContactUs> ContactUsList { get; set; }
        public List<Notifications> NotificationsList { get; set; }
        public List<Colors> ColorsList { get; set; }
        public List<Makes> MakesList { get; set; }
        public List<CarModels> ModelsList { get; set; }


        public List<CarsLocations> CarsLocationsList { get; set; }
        public List<EngineTypes> EngineTypesList { get; set; }
        public List<TotalSeats> TotalSeatsList { get; set; }
        public List<FuelTypes> FuelTypesList { get; set; }
        public List<TransmissionTypes> TransmissionTypesList { get; set; }



        public Users userObj { get; set; }

    }
}
