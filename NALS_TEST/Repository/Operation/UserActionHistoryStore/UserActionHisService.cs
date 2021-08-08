using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.LogUserAction;
using NALSTEST.Models.Operation.UserActionHistory;

namespace NALSTEST.Repository.Operation.UserActionHistoryStore
{
    public class UserActionHisService:IUserActionHisStore
    {
        private OracleDB db = new OracleDB();
        public DataTable GetList(string userName, string actionType, string frdate, string todate, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqOperation("BO_USER_HIS_GetList");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user", OracleType.VarChar),
                    new OracleParameter("p_actiontype", OracleType.Char),
                    new OracleParameter("p_fromdate", OracleType.Timestamp),
                    new OracleParameter("p_todate", OracleType.Timestamp),
                    new OracleParameter("pageIndex", OracleType.Number),
                    new OracleParameter("pageSize", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = actionType;
                oracParams[3].Value = CommonHelper.ToDateTimeNullable(CommonHelper.ConvertDdMmYyToMmDdYy(frdate));
                oracParams[4].Value = CommonHelper.DateTimeToDateSearch(CommonHelper.ConvertDdMmYyToMmDdYy(todate));
                oracParams[5].Value = pageIndex;
                oracParams[6].Value = pageSize;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetList: {0}",ex));
                return null;
            }
        }

        public DataTable GetById(decimal id)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqOperation("BO_USER_HIS_GetById");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = id;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetList: {0}", ex));
                return null;
            }
        }
    }
}