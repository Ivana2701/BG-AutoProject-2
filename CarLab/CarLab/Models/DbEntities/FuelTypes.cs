using System;

namespace CarLab.Models.DbEntities
{
    public class FuelTypes
    {
        public int FuelTypeID { get; set; }
        public string FuelType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
      
    }
}
