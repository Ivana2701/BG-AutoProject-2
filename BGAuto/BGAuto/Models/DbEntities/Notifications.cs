using System;

namespace BGAuto.Models.DbEntities
{
    public class Notifications
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public int NotificationType { get; set; }
        public bool IsReadByAdmin { get; set; }
        public int ReadByAdmin { get; set; }
        public DateTime? CreatedOn { get; set; }

    }
}
