using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class NotificationManagerModel
    {
       public List<Notifation> notifations { get; set; }
    }
    public class Notifation
    {
        public string Sno { get; set; }
        public string Name { get; set; }
        public string Notification { get; set; }
        public string Date { get; set; }
    }
}