using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Web.Mvc;


namespace NALSTEST.Repository.UserStore
{
    public class UserStoreService : IUserStore
    {
        private OracleDB db = new OracleDB();

        public bool Login(string userName, string password, string ip, ref string codelogin)
        {
            try
            {
                var sessionSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_LOGIN");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_password", OracleType.VarChar),
                    new OracleParameter("p_ip", OracleType.VarChar),
                    new OracleParameter("p_timeout", OracleType.Int32)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = MD5Helper.GetMd5(password);
                oracParams[3].Value = ip;
                oracParams[4].Value = Int32.Parse(sessionSection.Timeout.Minutes.ToString());
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                codelogin = dt.Rows[0][0].ToString();
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("Login: {0}", ex.ToString()));
                return false;
            }
        }
        public bool DeleteUser(decimal id)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_DELETE");
                var oracParams = new[]
                {
                    new OracleParameter("p_user_id", OracleType.Number),
                                    
                };
                oracParams[0].Value = id;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("NewDelete: {0}", ex.ToString()));
                return false;
            }
        }
        public bool CheckSession(string userName, string ip)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_CHECK_SESSION");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_ip", OracleType.VarChar),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = ip;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckSession: {0}", ex.ToString()));
                return false;
            }
        }
        public bool CheckSessionOut(string userName, string time)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_CHECK_SESSION_OUT");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_time", OracleType.Timestamp),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = CommonHelper.ToDateTimeNullable(time);
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckSessionOut: {0}", ex.ToString()));
                return false;
            }
        }
        public bool UpdateSession(string userName, string time)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_UPDATE_SESSION");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_time", OracleType.Timestamp),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = CommonHelper.ToDateTimeNullable(time);
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("UpdateSession: {0}", ex.ToString()));
                return false;
            }
        }
        public DataTable CheckExitUserLogin(string userName, string password)
        {
            try
            {
                var model = new UserLoginViewModel();
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_AUTHORIZEUSER");
                //var sql = "PKG_WEB_BAKEND_SYTEM_SUPPORT.BO_USER_AUTHORIZEUSER";
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_name", OracleType.VarChar),
                    new OracleParameter("p_password", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = MD5Helper.GetMd5(password);
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);              
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckExitUserLogin: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetUserByName(string userName)
        {
            try
            {
                var model = new UserLoginViewModel();
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETUSERBYUSERNAME");
                //var sql = "PKG_WEB_BAKEND_SYTEM_SUPPORT.BO_USER_GETUSERBYUSERNAME";
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
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetUserByName: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetUserById(decimal userId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETUSERBYID");
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
                NLogHelper.Logger.Error(string.Format("GetUserById: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetProfileUser(decimal userId)
        {
            try
            {
                var model = new UserProfileViewModel();
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETPROFILEUSER");
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
                NLogHelper.Logger.Error(string.Format("GetProfileUser: {0}", ex.ToString()));
                return null;
            }
        }

        public DataTable GetListUser(string userName, string status, string branch, string bankPos, string role, string fromdate, string todate)
        {
            try
            {
                var datas = new List<UserViewModel>();
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETLISTUSER");
                //var sql = "PKG_WEB_BAKEND_SYTEM_SUPPORT.BO_USER_GETLISTUSER";
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_name", OracleType.VarChar),
                    new OracleParameter("p_status", OracleType.VarChar),
                    new OracleParameter("p_branch", OracleType.VarChar),
                    new OracleParameter("p_bank_pos", OracleType.VarChar),
                    new OracleParameter("p_role", OracleType.Char),
                    new OracleParameter("p_fromdate", OracleType.Timestamp), 
                    new OracleParameter("p_todate", OracleType.Timestamp),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = status;
                oracParams[3].Value = branch;
                oracParams[4].Value = bankPos;
                oracParams[5].Value = role;
                oracParams[6].Value = CommonHelper.ToDateTimeNullable(CommonHelper.ConvertDdMmYyToMmDdYy(fromdate));
                oracParams[7].Value = CommonHelper.DateTimeToDateSearch(CommonHelper.ConvertDdMmYyToMmDdYy(todate));
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);               
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetListUser: {0}", ex.ToString()));
                return null;
            }

        }

        public int CreateUser(UserCreateViewModel model, string userCreate, string passWord, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_Insert");
                //var sql = "PKG_WEB_BACKEND_SYSTEM.BO_USER_Insert";
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_fullname", OracleType.VarChar),
                    new OracleParameter("p_password", OracleType.VarChar),
                    new OracleParameter("p_branch_code", OracleType.VarChar),
                    new OracleParameter("p_pos_code", OracleType.VarChar),
                    new OracleParameter("p_status", OracleType.VarChar),
                    new OracleParameter("p_roleid", OracleType.Number), 
                    new OracleParameter("p_created_user", OracleType.VarChar),
                    new OracleParameter("p_user_roletype", OracleType.VarChar),
                    new OracleParameter("p_phone", OracleType.VarChar),
                    new OracleParameter("p_mail", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = model.UserName.ToLower();
                oracParams[2].Value = model.FullName.ToUpper();
                oracParams[3].Value = MD5Helper.GetMd5(passWord); // model.Password
                oracParams[4].Value = model.BranchCode;
                oracParams[5].Value = model.PosCode;
                oracParams[6].Value = "1";
                oracParams[7].Value = model.RoleId;
                oracParams[8].Value = userCreate;
                oracParams[9].Value = model.SupperAdmin ? "S" : "N";
                oracParams[10].Value = model.Phone;
                oracParams[11].Value = model.Email;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return Int32.Parse(dt.Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("CreateUser: {0}", ex.ToString()));
                return -1;
            }
        }

        public bool UpdateUser(UserCreateViewModel model, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_UPDATE");
                //var sql = "PKG_WEB_BAKEND_SYTEM_SUPPORT.BO_USER_UPDATE";
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_fullname", OracleType.VarChar),
                    new OracleParameter("p_branch_code", OracleType.VarChar),
                    new OracleParameter("p_pos_code", OracleType.VarChar),
                    new OracleParameter("p_phone", OracleType.VarChar),
                    new OracleParameter("p_mail", OracleType.VarChar),
                    new OracleParameter("p_role_id", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = model.UserId;
                oracParams[2].Value = model.UserName;
                oracParams[3].Value = model.FullName.ToUpper();
                oracParams[4].Value = model.BranchCode;
                oracParams[5].Value = model.PosCode;
                oracParams[6].Value = model.Phone;
                oracParams[7].Value = model.Email;
                oracParams[8].Value = model.RoleId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("UpdateUser: {0}", ex.ToString()));
                return false;
            }
        }


        public bool ChangeStatus(decimal userId, string status, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_ChangeStatus");
                var oracParams = new[]
                {
                    new OracleParameter("p_user_id",OracleType.Number),
                    new OracleParameter("p_status",OracleType.Char)
                };
                oracParams[0].Value = userId;
                oracParams[1].Value = status;
                db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("ChangeStatus: {0}", ex.ToString()));
                return false;
            }
        }
        public string ChangePassword(decimal userId, string oldPassword, string newPassword, ref string erorr)
        {
            try
            {
                
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_ChangePassword");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_old_password", OracleType.VarChar),
                    new OracleParameter("p_new_password", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userId;
                oracParams[2].Value = MD5Helper.GetMd5(oldPassword);
                oracParams[3].Value = MD5Helper.GetMd5(newPassword);
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                var result = dt.Rows[0][0].ToString();
                if (result == "1")
                {
                    CreateHistoryPass(userId, newPassword); // để hạn che so lan doi pass cua 1 user
                    return result;
                }
                //else
                return result;
            }
            catch (Exception ex)
            {
                erorr = ex.Message;
                NLogHelper.Logger.Error(string.Format("ChangePassword: {0}", ex.ToString()));
                return "0";
            }

        }
        public bool ResetPassword(decimal userId, string newPassword, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_ResetPassword");
                var oraclParams = new[]
                {
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_new_password", OracleType.VarChar) 
                };
                oraclParams[0].Value = userId;
                oraclParams[1].Value = MD5Helper.GetMd5(newPassword);
                db.ExecuteDataTable(CommandType.StoredProcedure, sql, oraclParams);
                return true;
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("ResetPassword: {0}", ex.ToString()));
                return false;
            }
        }
        public bool AssignRole(decimal userId, string roleIds, string userCreated, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_USER_AuthorizeRole");
                var oracParams = new[]
                {
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_array_role_id",OracleType.VarChar),
                    new OracleParameter("p_created_user", OracleType.VarChar)
                };
                oracParams[0].Value = userId;
                oracParams[1].Value = roleIds;
                oracParams[2].Value = userCreated;
                db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("AssignRole: {0}", ex.ToString()));
                return false;
            }
        }
        public string GetUserStatus(decimal userId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETSTATUS");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetUserStatus: {0}", ex.ToString()));
                return string.Empty;
            }
        }
        public bool CheckModifyUser(string userCreated, decimal userId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_AUTHORIZEEDITUSER");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_user_create_name", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userId;
                oracParams[2].Value = userCreated;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckModifyUser: {0}", ex.ToString()));
                return false;
            }
        }
        public string GetUserNameByUserId(decimal userId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_GETUSERBYID");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0]["USERNAME"].ToString();
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetUserNameByUserId: {0}", ex.ToString()));
                return string.Empty;
            }
        }
        public bool CheckPassChange(string userName)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_CheckPassChange");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_name", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckPassChange: {0}", ex));
                return false;
            }

        }

        public bool CheckPassReset(string userName)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_USER_CheckPassReset");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_name", OracleType.VarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckPassChange: {0}", ex));
                return false;
            }

        }
        public bool CheckBlockUser(string userName)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqVersion2("CHECK_BLOOKUSER");
                var oracParams = new[]
                {
                    new OracleParameter("p_user_name", OracleType.VarChar)
                };
                oracParams[0].Value = userName;
                var dt = db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckBlockUser: {0}", ex));
                return false;
            }
        }

        public bool CheckHistoryPassWord(decimal userID, string password, int lengtop)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("PROC_CHECK_LAST_PASS");
                 var oracParams = new[]             {
                                                    new OracleParameter("p_out", OracleType.Cursor),
                                                    new OracleParameter("p_userid", OracleType.Int32),
                                                    new OracleParameter("p_password", OracleType.VarChar),
                                                    new OracleParameter("p_top", OracleType.Int32)
                                                    };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userID;
                oracParams[2].Value = MD5Helper.GetMd5(password).Trim(); 
                oracParams[3].Value = lengtop;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";            
             }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckHistoryPassWord: {0}", ex));
                return false;
            }
        }
        public bool CreateHistoryPass(decimal userID, string password)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("PROC_InsertHistoryPassUser");
               var oracParams = new[]             {
                                                     new OracleParameter("p_checkid", OracleType.Int32),
                                                     new OracleParameter("p_userid", OracleType.Int32),
                                                     new OracleParameter("p_password", OracleType.VarChar)
                                                   };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userID;
                oracParams[2].Value = MD5Helper.GetMd5(password).Trim(); 
                var dt = db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CreateHistoryPass: {0}", ex));
                return false;
            }
        }       
    }
}