using NALSTEST.Service.Role;
using System.Collections.Generic;
using System.Linq;

namespace NALSTEST.Models.UserModel
{
    public class UserViewModel : UserLoginViewModel
    {
        private RoleService _roleStoreService;

        public UserViewModel()
        {
            _roleStoreService = new RoleService();
        }
        public decimal Stt { get; set; }
        public string Branch { get; set; }
        public string BankPos { get; set; }
        public bool Status { get; set; }
        public string CreateByUser { get; set; }
        public string CreateDate { get; set; }
        public string Email { get; set; }

        public List<decimal> RoleList
        {
            get
            {
                return _roleStoreService.GetAuthorizeRolePerUserId(UserId).Select(m => m.RoleId).ToList();
            }
        }

        public string Role
        {
            get
            {
                var roles = _roleStoreService.GetAuthorizeRolePerUserId(UserId).Select(m => m.RoleName).ToList();
                var roleDis = roles.Aggregate(string.Empty, (current, role) => current + role + ',');
                return roleDis.TrimEnd(',');
            }
        }
    }
}