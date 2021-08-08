using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.Operation.UserSession
{
    public class UserSessionViewModel
    {
        public int Id { get; set; }
        public int Ordinal { get; set; }
        public string UserName { get; set; }
        public string IP { get; set; }
        public string StartDate { get; set; }
        public string TimeoutDate { get; set; }
        public string AccessDate { get; set; }
        public string Status { get; set; }
    }
}