using System.Collections.Generic;
using NALSTEST.Models.RoleModel;
using System.Data;
namespace NALSTEST.Repository.RoleStore
{
    public interface IRoleStore
    {
        /// <summary>
        /// Lay danh sach role theo dieu kien tim kiem
        /// </summary>
        /// <param name="roleName"> type string</param>
        /// <param name="roleStatus">type string</param>
        /// <returns>List RoleViewModel</returns>
        DataTable GetListRole(string roleName, string roleStatus);
        /// <summary>
        /// Lay ve list string role nam theo userName
        /// </summary>
        /// <param name="userName">type string</param>
        /// <returns>List string</returns>
        DataTable GetListRolePerUser(string userName);
        /// <summary>
        /// Lay ve danh sach role theo userId
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <returns>List RoleViewModel</returns>
        DataTable GetListRolePerUserId(decimal userId);
        /// <summary>
        /// Lay ve danh sach role da duoc assigne cho user theo userId
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <returns>List RoleViewModel</returns>
        DataTable GetAuthorizeRolePerUserId(decimal userId);
        /// <summary>
        /// Lay ve thong tin role theo roleid
        /// </summary>
        /// <param name="roleId">type decimal</param>
        /// <returns>CreateRoleModel</returns>
        DataTable GetRoleById(decimal roleId);

        /// <summary>
        /// Lay ve trang thai role: active hay inactive
        /// </summary>
        /// <param name="roleId">type decimal</param>
        /// <returns>string 0 or 1</returns>
        string GetRoleStatus(decimal roleId);
        /// <summary>
        /// Thay doi trang thai cua role neu active --> inactive va nguoc lai
        /// </summary>
        /// <param name="roleId">type deciaml</param>
        /// <param name="status">type string</param>
        /// <returns>true or false</returns>
        bool ChangeStatus(decimal roleId, string status,ref string loidb);
        /// <summary>
        /// Tao moi role
        /// </summary>
        /// <param name="model">Type CreateRoleModel</param>
        /// <returns>true or false</returns>
        bool CreateRole(CreateRoleModel model, ref string loidb);
        /// <summary>
        /// Cap nhat lai role
        /// </summary>
        /// <param name="model">Type CreateRoleModel</param>
        /// <returns>true or false</returns>
        bool UpdateRole(CreateRoleModel model,ref string loidb );
        /// <summary>
        /// Xoa role
        /// </summary>
        /// <param name="roleId">type decimal</param>
        /// <returns>true or false</returns>
        bool DeleteRole(decimal roleId);
        /// <summary>
        /// Kiem tra user hien tai co quyen chinh sua role hay khong
        /// </summary>
        /// <param name="roleId">Id cua role duoc chinh sua Type decimal</param>
        /// <param name="userId">Id cua user dang nhap Type decimal</param>
        /// <returns>true or false</returns>
        bool CheckModifyRole(decimal roleId, decimal userId);
    }
}