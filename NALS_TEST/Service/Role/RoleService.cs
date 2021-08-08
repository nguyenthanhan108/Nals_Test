
using NALSTEST.Models.RoleModel;
using NALSTEST.Repository.RoleStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NALSTEST.Helpers;

namespace NALSTEST.Service.Role
{
    public class RoleService
    {
        RoleStoreService roleStore = new RoleStoreService();
        public IList<RoleViewModel> GetListRole(string roleName, string roleStatus)
        {
            try
            {
                var datas = new List<RoleViewModel>();
                var dt = roleStore.GetListRole(roleName, roleStatus);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new RoleViewModel()
                    {
                        Stt = i + 1,
                        RoleId = Int64.Parse(dt.Rows[i]["ROLE_ID"].ToString()),
                        RoleName = dt.Rows[i]["ROLE_NAME"].ToString(),
                        RoleDesc = dt.Rows[i]["ROLE_DESC"].ToString(),
                        UserId = Int64.Parse(dt.Rows[i]["BO_USER_ID"].ToString()),
                        Status = dt.Rows[i]["ROLE_STATUS"].ToString() == "1",
                        CreateDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["CREATED_DATE"].ToString())
                    };
                    datas.Add(model);
                }
                return datas;
            }
            catch (Exception ex)
            {
                return new List<RoleViewModel>();
            }
        }

        public IList<string> GetListRolePerUser(string userName)
        {
            try
            {
                var listRole = new List<string>();
                var dt = roleStore.GetListRolePerUser(userName);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listRole.Add(dt.Rows[i]["ROLE_NAME"].ToString().Trim().ToUpper());
                }
                return listRole;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
        public IList<RoleViewModel> GetListRolePerUserId(decimal userId)
        {
            try
            {
                var datas = new List<RoleViewModel>();
                var dt = roleStore.GetListRolePerUserId(userId);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new RoleViewModel()
                    {
                        Stt = i + 1,
                        RoleId = Int64.Parse(dt.Rows[i]["ROLE_ID"].ToString().Trim()),
                        RoleName = dt.Rows[i]["ROLE_NAME"].ToString().Trim(),
                        RoleDesc = dt.Rows[i]["ROLE_DESC"].ToString(),
                        UserId = Int64.Parse(dt.Rows[i]["BO_USER_ID"].ToString()),
                        Status = dt.Rows[i]["ROLE_STATUS"].ToString() == "1",
                    };
                    datas.Add(model);
                }
                return datas;
            }
            catch (Exception ex)
            {
                return new List<RoleViewModel>();
            }
        }
        public IList<RoleViewModel> GetAuthorizeRolePerUserId(decimal userId)
        {
            try
            {
                var datas = new List<RoleViewModel>();
                var dt = roleStore.GetAuthorizeRolePerUserId(userId);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new RoleViewModel()
                    {
                        Stt = i + 1,
                        RoleId = Int64.Parse(dt.Rows[i]["ROLE_ID"].ToString()),
                        RoleName = dt.Rows[i]["ROLE_NAME"].ToString(),
                        RoleDesc = dt.Rows[i]["ROLE_DESC"].ToString(),
                        UserId = Int64.Parse(dt.Rows[i]["BO_USER_ID"].ToString()),
                        Status = dt.Rows[i]["ROLE_STATUS"].ToString() == "1",
                    };
                    datas.Add(model);
                }
                return datas;
            }
            catch (Exception ex)
            {
                return new List<RoleViewModel>();
            }
        }
        public CreateRoleModel GetRoleById(decimal roleId)
        {
            try
            {
                var model = new CreateRoleModel();
                var dt = roleStore.GetRoleById(roleId);
                if (dt.Rows.Count == 0) return null;
                else
                {
                    model.RoleId = Int64.Parse(dt.Rows[0]["ROLE_ID"].ToString());
                    model.RoleName = dt.Rows[0]["ROLE_NAME"].ToString();
                    model.RoleDesc = dt.Rows[0]["ROLE_DESC"].ToString();
                    model.RoleStatus = dt.Rows[0]["ROLE_STATUS"].ToString();
                    model.UserId = Int64.Parse(dt.Rows[0]["BO_USER_ID"].ToString());
                    model.ArrayFunc = GetListFuncId(dt);
                }
                return model;
            }
            catch (Exception ex)
            {
                return new CreateRoleModel();
            }
        }
        public string GetRoleStatus(decimal roleId)
        {
            var dt = roleStore.GetRoleStatus(roleId);              
            return dt;
        }
        public bool ChangeStatus(decimal roleId, string status,ref string loidb)
        {
            var dt = roleStore.ChangeStatus(roleId, status, ref loidb);
            if (dt)
                return true;
            else
                return false;
        }
        public bool CreateRole(CreateRoleModel model,ref string loidb)
        {
            var dt = roleStore.CreateRole(model, ref loidb);
            if (dt)
                return true;
            else
                return false;
        }
        public bool UpdateRole(CreateRoleModel model, ref string loidb)
        {
            var dt = roleStore.UpdateRole(model, ref loidb);
            if (dt )
                return true;
            else
                return false;
        }
        //public bool DeleteRole(decimal roleId)
        //{
        //    var dt = roleStore.DeleteRole(roleId);
        //    if (dt)
        //        return true;
        //    else
        //        return false;
        //}
        public bool CheckModifyRole(decimal roleId, decimal userId)
        {
            var dt = roleStore.CheckModifyRole(roleId, userId);
            if (dt)
                return true;
            else
                return false;
        }
        private string GetListFuncId(DataTable dt)
        {
            var str = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str = str + dt.Rows[i]["FUNC_ID"].ToString() + ",";
            }
            return str.TrimEnd(',');
        }
    }     
}