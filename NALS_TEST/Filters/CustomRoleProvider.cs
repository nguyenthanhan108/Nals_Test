using System;
using System.Linq;
using System.Web.Security;
using NALSTEST.Service.Role;

namespace NALSTEST.Filters
{
    public class CustomRoleProvider : RoleProvider
    {
        private RoleService _roleStore = new RoleService();
        public CustomRoleProvider()
        {
            _roleStore = new RoleService();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            //var roles = _roleStore.GetListRolePerUser(username);
            //if (roles.Count == 0) return false;
            //return roles.Contains(roleName.Trim().ToUpper());
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            var roles = _roleStore.GetListRolePerUser(username).ToArray();
            return roles;
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}