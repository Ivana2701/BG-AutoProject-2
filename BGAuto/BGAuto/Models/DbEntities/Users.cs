using System;

namespace BGAuto.Models.DbEntities
{
    public class Users
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int UserType { get; set; }
       
        public DateTime? CreateOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public string Response { get; set; }


    }
}
