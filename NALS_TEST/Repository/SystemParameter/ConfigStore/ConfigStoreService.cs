using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.SystemParameter.ConfigModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace NALSTEST.Repository.SystemParameter.ConfigStore
{
    public class ConfigStoreService : IConfigStore
    {
        private OracleDB db = new OracleDB();
        public DataTable GetList(string code, string value, string desc, string status)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_GetList");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_code", OracleType.VarChar),
                    new OracleParameter("p_value", OracleType.NVarChar),
                    new OracleParameter("p_desc", OracleType.NVarChar),
                    new OracleParameter("p_status", OracleType.Char)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = code;
                oracParams[2].Value = value;
                oracParams[3].Value = desc;
                oracParams[4].Value = status;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);             
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetList: {0}", ex));
                return null;
            }
        }

        public bool Insert(ConfigCreateViewModel model, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_Insert");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_code", OracleType.VarChar),
                    new OracleParameter("p_value", OracleType.NVarChar),
                    new OracleParameter("p_desc", OracleType.NVarChar),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = model.Code;
                oracParams[2].Value = model.Value;
                oracParams[3].Value = model.Desc;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("Insert: {0}", ex));
                return false;
            }
        }
        public string Update(ConfigCreateViewModel model, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_Update");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_code", OracleType.VarChar),
                    new OracleParameter("p_value", OracleType.NVarChar),
                    new OracleParameter("p_desc", OracleType.NVarChar)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = model.Code;
                oracParams[2].Value = model.Value;
                oracParams[3].Value = model.Desc;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("Update: {0}", ex));
                return null;
            }
        }

        public bool ChangeStatus(string code, string status, ref string loidb)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_ChangeStatus");
                var oracParams = new[]
                {
                    new OracleParameter("p_code", OracleType.VarChar),
                    new OracleParameter("p_status", OracleType.Char)
                };
                oracParams[0].Value = code;
                oracParams[1].Value = status;
                db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                loidb = ex.Message;
                NLogHelper.Logger.Error(string.Format("ChangeStatus: {0}", ex));
                return false;
            }
        }
        public bool Delete(string code)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_Delete");
                var oracParams = new[]
                {
                    new OracleParameter("p_code", OracleType.VarChar)
                };
                oracParams[0].Value = code;
                db.ExecuteNonQuery(CommandType.StoredProcedure, sql, oracParams);
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("Delete: {0}", ex));
                return false;
            }
        }
        public DataTable GetConfigById(string code)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_GetById");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_code", OracleType.VarChar),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = code;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);               
                return dt;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetConfigById: {0}", ex));
                return null;
            }
        }
        public string GetConfigByValue(string code)
        {
            try
            {
                var sql = CommonHelper.GetProcedurePkgConfigSystem("MB_CONFIG_GetById");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_code", OracleType.VarChar),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = code;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0]["CONFIG_VALUE"].ToString();
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetConfigById: {0}", ex));
                return null;
            }
        }
        public bool ClearCache()
        {
            try
            {
                //var webService = new WebReference.BIDVMobile();
                //webService.forceUpdateCache();
                return true;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("ClearCache: {0}",ex));
                return false;
            }
        }
    }
}