using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.RoleModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;

namespace NALSTEST.Repository.RoleStore
{
    public class RoleStoreService : IRoleStore
    {
        private OracleDB db;

        public RoleStoreService()
        {
            db = new OracleDB();
        }
        public DataTable GetListRole(string roleName, string roleStatus)
        {
            try
            {
                var datas = new List<RoleViewModel>();
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_ROLE_GETLISTROLE");
                //var sql = "PKG_WEB_BAKEND_SYTEM_SUPPORT.BO_ROLE_GETLISTROLE";
                var oracaParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_role_name", OracleType.VarChar),
                    new OracleParameter("p_role_status", OracleType.Char)
                };
                oracaParams[0].Direction = ParameterDirection.Output;
                oracaParams[1].Value = roleName;
                oracaParams[2].Value = roleStatus;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracaParams);                
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetListRole: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetListRolePerUser(string userName)
        {
            var listRole = new List<string>();
            var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETAUTHORROLEBYUSER");
            //var sql = "PKG_WEB_BAKEND_SYTEM_SUPPORT.BO_USER_GETAUTHORROLEBYUSER";
            var oracParams = new[]
            {
                new OracleParameter("p_out", OracleType.Cursor),
                new OracleParameter("p_user_name", OracleType.VarChar),
            };
            oracParams[0].Direction = ParameterDirection.Output;
            oracParams[1].Value = userName;
            var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);            
            return dt;
        }

        public DataTable GetListRolePerUserId(decimal userId)
        {
            try
            {
                var datas = new List<RoleViewModel>();
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_ROLE_GetPerByUserId");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);                
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetListRolePerUserId: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetAuthorizeRolePerUserId(decimal userId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_ROLE_GetAuthorizedRole");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);               
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetAuthorizeRolePerUserId: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetRoleById(decimal roleId)
        {
            try
            {
                var model = new CreateRoleModel();
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_ROLE_FUNC_GetList");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_role_id", OracleType.Number),
                    new OracleParameter("p_func_id", OracleType.Number) 
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = roleId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);                
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetRoleById: {0}", ex.ToString()));
                return null;
            }

        }

        public string GetRoleStatus(decimal roleId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_ROLE_GETROLESTATUS");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_role_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = roleId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetRoleStatus: {0}", ex.ToString()));
                return null;
            }
        }

        public bool ChangeStatus(decimal roleId, string status,ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_ROLE_ChangeStatus");
                var oracParams = new[]
                {
                    new OracleParameter("p_role_id", OracleType.Number),
                    new OracleParameter("p_role_status", OracleType.Char)
                };
                oracParams[0].Value = roleId;
                oracParams[1].Value = status;
                db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error("ChangStatus: {0}", ex.ToString());
                return false;
            }
        }

        public bool CreateRole(CreateRoleModel model,ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_ROLE_Insert");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_role_name",OracleType.VarChar),
                    new OracleParameter("p_role_desc", OracleType.VarChar),
                    new OracleParameter("p_role_status", OracleType.Char),
                    new OracleParameter("p_bo_user_id", OracleType.Number),
                    new OracleParameter("p_array_func_id", OracleType.VarChar) 
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = model.RoleName;
                oracParams[2].Value = model.RoleDesc;
                oracParams[3].Value = model.RoleStatus;
                oracParams[4].Value = model.UserId;
                oracParams[5].Value = GetListFuncsFromString(model.ArrayFunc);
                var result = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return result.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("Create Role: {0}", ex.ToString()));
                return false;
            }

        }

        public bool UpdateRole(CreateRoleModel model, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_ROLE_UpdateWithFunc");
                var oracParams = new[]
                {
                    new OracleParameter("p_role_id", OracleType.Number),
                    new OracleParameter("p_role_name",OracleType.VarChar),
                    new OracleParameter("p_role_desc", OracleType.VarChar),
                    new OracleParameter("p_role_status", OracleType.Char),
                    new OracleParameter("p_bo_user_id", OracleType.Number),
                    new OracleParameter("p_array_func_id", OracleType.VarChar) 
                };
                oracParams[0].Value = model.RoleId;
                oracParams[1].Value = model.RoleName;
                oracParams[2].Value = model.RoleDesc;
                oracParams[3].Value = model.RoleStatus;
                oracParams[4].Value = model.UserId;
                oracParams[5].Value = GetListFuncsFromString(model.ArrayFunc);
                db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return true;

            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("Update Role: {0}", ex.ToString()));
                return false;
            }

        }

        public bool DeleteRole(decimal roleId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_ROLE_DELETE");
                var oracParams = new[]
                {
                    new OracleParameter("p_role_id", OracleType.Number)
                };
                oracParams[0].Value = roleId;
                db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("Delete: {0}", ex.ToString()));
                return false;
            }
        }

        public bool CheckModifyRole(decimal roleId, decimal userId)
        {
            try
            {
                if (userId == 1) return true; //Neu la user system thi cho phep sua
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_ROLE_AUTHORIZEMODIFYROLE");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_role_id", OracleType.Number),
                    new OracleParameter("p_user_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = roleId;
                oracParams[2].Value = userId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckModifyRole: {0}", ex.ToString()));
                return false;
            }
        }

        #region private Methode
        private string GetListFuncId(DataTable dt)
        {
            var str = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str = str + dt.Rows[i]["FUNC_ID"].ToString() + ",";
            }
            return str.TrimEnd(',');
        }

        private string GetListFuncsFromString(string str)
        {
            try
            {
                var strId = string.Empty;
                var ids = SplitString(str);
                var tempIds = new List<int>();
                foreach (var id in ids)
                {
                    GetParentIdAddToList(id, tempIds);
                }
                strId = tempIds.Aggregate(strId, (current, tempId) => current + tempId.ToString() + ",");
                return strId.TrimEnd(',');
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetListFuncsFromString: {0}", ex.ToString()));
                return string.Empty;
            }
        }
        private List<int> SplitString(string str)
        {
            var split = str.Split(',').ToList();
            return split.Select(Int32.Parse).ToList();
        }
        private List<int> GetParentIdAddToList(int funcId, List<int> ids)
        {

            var sql = CommonHelper.GetProcedureForPkqSystem("BO_FUNCTIONS_GetParentFuncId");
            var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_func_id", OracleType.Number)
                };
            oracParams[0].Direction = ParameterDirection.Output;
            oracParams[1].Value = funcId;
            var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
            //đệ quy
            var parentId = Convert.ToInt32(dt.Rows[0][0].ToString());
            if (!ids.Contains(funcId))
            {
                ids.Add(funcId);
            }
            if (!ids.Contains(parentId))
            {
                ids.Add(parentId);
            }
            var level = Convert.ToInt32(dt.Rows[0][1].ToString());
            if (level == 2)
            {
                return ids;
            }
            return GetParentIdAddToList(parentId, ids);
        }
    }
        #endregion

}