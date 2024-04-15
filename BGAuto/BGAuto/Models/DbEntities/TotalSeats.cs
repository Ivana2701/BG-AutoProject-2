using System;

namespace BGAuto.Models.DbEntities
{
    public class TotalSeats
    {
        public int SeatNumberID { get; set; } 
        public int Seats { get; set; } 
        public bool IsActive { get; set; } 
        public DateTime? CreatedOn { get; set; } 
        public int? CreatedBy { get; set; } 
    }
}
