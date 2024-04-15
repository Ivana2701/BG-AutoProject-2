using System;

namespace CarLab.Models.DbEntities
{
    public class Orders
    {
        public int OrderID { get; set; } 
        public string CustomerFullName { get; set; } 
        public string CustomerEmail { get; set; } 
        public string CustomerPhone { get; set; } 
        public string CustomerAddress { get; set; } 
        public DateTime? DeliveryDate { get; set; } 
        public decimal TotalAmount { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public int TotalItems { get; set; } 
        public int StatusID { get; set; } 
        public string StatusName { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string SizeName { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string ColorName { get; set; }
        public string LocationName { get; set; }
        public int OrdeDetailID { get; set; }

        public string ResponseMsg { get; set; }

    }
}
