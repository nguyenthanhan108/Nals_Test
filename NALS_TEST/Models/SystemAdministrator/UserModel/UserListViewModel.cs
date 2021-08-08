
using NALSTEST.Models.RoleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.SystemAdministrator.UserModel
{
    public class UserListViewModel
    {
        public decimal UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string BranchCode { get; set; }
        public string PosCode { get; set; }
        public string RoleId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public bool SupperAdmin { get; set; }
        public List<RoleViewModel> listRole { get; set; }

    }
 
}