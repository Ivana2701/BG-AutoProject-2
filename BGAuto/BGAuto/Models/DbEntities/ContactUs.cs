using System;

namespace BGAuto.Models.DbEntities
{
    public class ContactUs
    {

        public int ContactUsID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerMessage { get; set; }
        public string AdminReply { get; set; }
        public DateTime? CustomerAskedOn { get; set; }
        public DateTime? AdminReliedOn { get; set; }

      
    }
}
