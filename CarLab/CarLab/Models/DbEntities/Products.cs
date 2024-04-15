using Microsoft.AspNetCore.Http;
using System;

namespace CarLab.Models.DbEntities
{
    public class Products
    {
        public int ProductID { get; set; } 
        public string ProductName { get; set; } 
        public string Description { get; set; } 
        public int SizeID { get; set; } 
        public int CategoryID { get; set; } 
        public int MakeID { get; set; } 
        public int ModelID { get; set; } 
        public int ColorID { get; set; }
        public int LocationID { get; set; }
        public int EnginePowerID { get; set; }
        public int SeatNumberID { get; set; }
        public int FuelTypeID { get; set; }
        public int TransmissionID { get; set; }

        public string MakeName { get; set; } 
        public string ModelName { get; set; } 
        public string ColorName { get; set; } 
        public decimal Price { get; set; } 
        public string ProductYear { get; set; } 
        public String  SizeName { get; set; } 
        public String CategoryName { get; set; }

        public string LocationName { get; set; }
        public string EngineType { get; set; }
        public int? Seats { get; set; }
        public string FuelType { get; set; }
        public string TransmissionType { get; set; }



        public bool IsActive { get; set; } 
        public bool ShowOnHomePage { get; set; } 
        public int DisplaySeqNo { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public int? CreatedBy { get; set; } 
        public DateTime? ModifiedOn { get; set; } 
        public int? ModifiedBy { get; set; }

        public IFormFile photoFile { get; set; }
        public byte[] ProductImage { get; set; }

        public int Quantity { get; set; }
        public int? OrderID { get; set; }


    }
}
