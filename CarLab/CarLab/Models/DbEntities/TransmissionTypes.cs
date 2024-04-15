using System;

namespace CarLab.Models.DbEntities
{
    public class TransmissionTypes
    {

        public int TransmissionID { get; set; }
        public string TransmissionType { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
