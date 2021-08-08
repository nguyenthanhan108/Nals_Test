using System.EnterpriseServices;
using NALSTEST.Models.UserModel;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
namespace NALSTEST.Repository.UserStore
{
    public interface IUserStore
    {
        /// <summary>
        /// Dang nhap va kiem tra user dang nhap
        /// </summary>
        /// <param name="userName">Ten dang nhap</param>
        /// <param name="password">Mat khau dang nhap</param>
        /// <param name="ip">Ip client login</param>
        /// <returns>true or false</returns>
        bool Login(string userName, string password, string ip ,ref string codelogin);
        /// <summary>
        /// Xac thuc user theo username va password
        /// </summary>
        /// <param name="userName">type string</param>
        /// <param name="password">type string</param>
        /// <returns>UserLoginViewModel</returns>
        DataTable CheckExitUserLogin(string userName, string password);
        /// <summary>
        /// Lay ve thong tin user login theo userName
        /// </summary>
        /// <param name="userName">type string</param>
        /// <returns>UserLoginViewModel</returns>
        DataTable GetUserByName(string userName);
        /// <summary>
        /// Lay ve thong tin user login theo userId
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <returns>UserCreateViewModel</returns>
        DataTable GetUserById(decimal userId);
        /// <summary>
        /// Lay ve thong tin user dang nhap: loginIp, lastLogin Time
        /// </summary>
        /// <param name="userId">User id dang nhap</param>
        /// <returns>UserProfileViewModel</returns>
        DataTable GetProfileUser(decimal userId);

        /// <summary>
        /// Lay ve danh sach user theo cac dieu kien tim kiem
        /// </summary>
        /// <param name="userName">User Name type string</param>
        /// <param name="status">Status type string</param>
        /// <param name="branch">Branche type string</param>
        /// <param name="bankPos">type string</param>
        /// <param name="fromDate">type string</param>
        /// <param name="toDate">type string</param>
        /// <returns>List UserViewModel</returns>
        DataTable GetListUser(string userName, string status, string branch, string bankPos, string role, string fromdate, string todate);
        /// <summary>
        /// Tao moi user
        /// </summary>
        /// <param name="model">Type UserCreateViewModel</param>
        /// <param name="userCreate">Type string: User hien tai tao user nay</param>
        /// <returns>Type int: 1 = success; -1 = error</returns>
        int CreateUser(UserCreateViewModel model, string userCreate, string passWord, ref string loidb);
        /// <summary>
        /// Cap nhat thong tin User
        /// </summary>
        /// <param name="model">type UserCreateViewModel</param>
        /// <returns>true or false</returns>
        bool UpdateUser(UserCreateViewModel model, ref string loidb);
        /// <summary>
        /// Xoa user
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <returns>true or false</returns>
        bool DeleteUser(decimal userId);
        /// <summary>
        /// Thay doi trang thai user: active -> inactive va nguoc lai
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <param name="status">type string</param>
        /// <returns>true or false</returns>
        bool ChangeStatus(decimal userId, string status, ref string loidb);
        /// <summary>
        /// Thay doi password cho user
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <param name="oldPassword">type string</param>
        /// <param name="newPassword">type string</param>
        /// <returns>true or false</returns>
        string ChangePassword(decimal userId, string oldPassword, string newPassword, ref string error);
        /// <summary>
        /// Thiet lap password cua user ve gia tri mac dinh "123456
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <param name="newPassword">type string: 123456</param>
        /// <returns>true or false</returns>
        bool ResetPassword(decimal userId, string newPassword, ref string loidb);
        /// <summary>
        /// Gan role cho user
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <param name="roleIds">type string: danh sach id role</param>
        /// <param name="userCreated">type string: user thuc hien gan role</param>
        /// <returns>true or false</returns>
        bool AssignRole(decimal userId, string roleIds, string userCreated, ref string loidb);
        /// <summary>
        /// Lay ve trang thai user
        /// </summary>
        /// <param name="userId">type decimal</param>
        /// <returns>type string: 1 or 0</returns>
        string GetUserStatus(decimal userId);
        /// <summary>
        /// Kiem tra xem user hien tai co quyen chinh sua user trong danh sach user hay khong
        /// </summary>
        /// <param name="userCreated">Ten cua User hien tai Type:string </param>
        /// <param name="userId">Id cua user co the bi chinh sua Type:decimal</param>
        /// <returns>true or false</returns>
        bool CheckModifyUser(string userCreated, decimal userId);
        /// <summary>
        /// Lay thong tin username theo userId
        /// </summary>
        /// <param name="userId">Id cua user</param>
        /// <returns>User name type:string</returns>
        string GetUserNameByUserId(decimal userId);
        bool CheckPassChange(string userName);
    }
}