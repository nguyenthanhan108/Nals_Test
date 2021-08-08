using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NALSTEST.Models.RoleModel
{
    public class CreateRoleModel
    {
        public decimal RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
        public string RoleStatus { get; set; }
        public decimal UserId { get; set; }
        public string ArrayFunc { get; set; }
    }
}