using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.Operation.UserSessionHis;

namespace NALSTEST.Repository.Operation.UserSessionHisStore
{
    public class UserSessionHisService:IUserSessionHisStore
    {
        private OracleDB db = new OracleDB();
        public DataTable GetList(string userName, string frd, string td,int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqOperation("VIEW_BO_SESSION_HIS_GetList");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_userName", OracleType.VarChar),
                    new OracleParameter("p_fromdate", OracleType.Timestamp),
                    new OracleParameter("p_todate", OracleType.Timestamp),
                    new OracleParameter("pageIndex", OracleType.Number),
                    new OracleParameter("pageSize", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = CommonHelper.ToDateTimeNullable(CommonHelper.ConvertDdMmYyToMmDdYy(frd));
                oracParams[3].Value = CommonHelper.DateTimeToDateSearch(CommonHelper.ConvertDdMmYyToMmDdYy(td));
                oracParams[4].Value = pageIndex;
                oracParams[5].Value = pageSize;
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