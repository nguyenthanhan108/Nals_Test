using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.Operation.UserSession;

namespace NALSTEST.Repository.Operation.UserSessionStore
{
    public class UserSessionStoreService:IUserSessionStore
    {
        private OracleDB db = new OracleDB();
        public DataTable GetList(string userName, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqOperation("USER_SESSION_GETLIST");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor), 
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_time", OracleType.Timestamp),
                    new OracleParameter("pageIndex", OracleType.Number),
                    new OracleParameter("pageSize", OracleType.Number) 
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = DateTime.Now;
                oracParams[3].Value = pageIndex;
                oracParams[4].Value = pageSize;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);              
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetList: {0}", ex.ToString()));
                return null;
            }
        }

        public void Update(string status, string userName)
        {
            try
            {
                var sessionSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
                var time = Int32.Parse(sessionSection.Timeout.Minutes.ToString());
                var sql = CommonHelper.GetProcedurePkqOperation("USER_SESSION_UPDATE");
                var oracParams = new[]
                {
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_timeoutdate", OracleType.Timestamp),
                    new OracleParameter("p_accessdate", OracleType.Timestamp),
                    new OracleParameter("p_status", OracleType.Char)
                };
                oracParams[0].Value = userName;
                oracParams[1].Value = DateTime.Now.AddMinutes(time);
                oracParams[2].Value = DateTime.Now;
                oracParams[3].Value = status;
                db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("Update :{0}",ex));
            }
        }

        public void UpdateEndSession(string userName, string status)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqOperation("USER_SESSION_CHANGESTATUS");
                var oracParams = new[]
                {
                    new OracleParameter("p_username", OracleType.VarChar),
                    new OracleParameter("p_status", OracleType.Char)
                };
                oracParams[0].Value = userName;
                oracParams[1].Value = status;
                db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("Update :{0}", ex));
            }
        }
    }
}