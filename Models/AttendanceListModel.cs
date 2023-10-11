using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I_RAY_ASSET_TRACKER_MVC.Models
{
    public class AttendanceListModel
    {
        public string fileupload { get; set; }
        public List<DisplayEmployeeAttendenceList> displayEmployeeAttendenceLists { get; set; }
    }
    public class DisplayEmployeeAttendenceList
    {
        public string EmployeeCode { get; set;}
        public string PresentDays { get; set;}
        public string AbsentDays { get; set;}
        public string NormalWorkingHours { get; set;}
        public string OTHours { get; set;}
        public string OTDays { get; set;}
        public string CL { get; set;}
        public string PL { get; set;}
        public string SL { get; set;}
        public string TotalLeave { get; set;}
        public string LateComingDays { get; set;}
        public string LateComingHours { get; set;}
        public string EarlyGoingDays { get; set;}
        public string EarlyGoingHours { get; set;}
        public string WeeklyOff { get; set;}
        public string WeeklyOffPresent { get; set;}
        public string Holiday { get; set;}
        public string HolidayPresent { get; set;}
    }
}