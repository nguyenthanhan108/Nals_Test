using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.SystemAdministrator.UserModel
{
    public class UserInforBidv
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string FullName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Department { get; set; }
        public string LoginMessage { get; set; }
        public bool Check { get; set; }
    }
}