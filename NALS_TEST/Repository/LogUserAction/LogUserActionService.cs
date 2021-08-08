using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.LogUserAction;
using System;
using System.Data;
using System.Data.OracleClient;

namespace NALSTEST.Repository.LogUserAction
{
    public class LogUserActionService : ILogUserAction
    {
        private OracleDB db = new OracleDB();
        public LogUserActionCreateModel GetLogModel(decimal userId, int funcId, LogActionTypeModel.ActionTypeEnum actionType, string actionDesc)
        {
            try
            {
                return new LogUserActionCreateModel()
                {
                    UserId = userId,
                    FuncId = funcId,
                    ActionDesc = actionDesc,
                    ActionType = LogActionTypeModel.Parse(actionType)
                };
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetLogModel: {0}", ex));
                return null;
            }
        }

        public void Insert(LogUserActionCreateModel model)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkqOperation("BO_USER_HIS_Insert");
                var oracParams = new[]
                {
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_funcid", OracleType.Number),
                    new OracleParameter("p_actiontype", OracleType.Char),
                    new OracleParameter("p_actiondesc", OracleType.NVarChar),
                };
                oracParams[0].Value = model.UserId;
                oracParams[1].Value = model.FuncId;
                oracParams[2].Value = model.ActionType;
                oracParams[3].Value = model.ActionDesc;
                db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("Insert: {0}", ex));
            }
        }
    }
}