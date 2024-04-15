using System;

namespace BGAuto.Models.DbEntities
{
    public class CarModels
    {
        public int ModelID { get; set; }
        public int MakeID { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}
