using DBConnection;
using NALSTEST.Helpers;
using NALSTEST.Models.FunctionModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;

namespace NALSTEST.Repository.FunctionStore
{
    public class FunctionStoreService : IFunctionStore
    {
        private OracleDB db = new OracleDB();
        public IList<FunctionViewModel> GetFunctionsByUserId(decimal id, decimal? parentId, string funcCode, string funcdisplay)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_FUNCTIONS_GetPerList");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_id", OracleType.Number),
                    new OracleParameter("p_parent_id", OracleType.Number),
                    new OracleParameter("p_func_code", OracleType.VarChar),
                    new OracleParameter("p_func_display", OracleType.Char)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = id;
                oracParams[2].Value = parentId;
                oracParams[3].Value = funcCode;
                oracParams[4].Value = funcdisplay;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                //De quy 
                var temp = new Dictionary<decimal, FunctionViewModel>();
                var map = new Dictionary<decimal, decimal>();
                int min = 100;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new FunctionViewModel()
                    {
                        FuncId = Int64.Parse(dt.Rows[i]["FUNC_ID"].ToString()),
                        FuncCode = dt.Rows[i]["FUNC_CODE"].ToString(),
                        FuncName = dt.Rows[i]["FUNC_NAME"].ToString(),
                        FuncUrl = dt.Rows[i]["FUNC_URL"].ToString(),
                        FuncOrder = Int32.Parse(dt.Rows[i]["FUNC_ORDER"].ToString()),
                        FuncDisplay = dt.Rows[i]["FUNC_DISPLAY"].ToString(),
                        FuncLevel = Int32.Parse(dt.Rows[i]["FUNC_LEVEL"].ToString()),
                        FuncParentId = Int64.Parse(dt.Rows[i]["PARENT_ID"].ToString()),
                        FuncControl = dt.Rows[i]["FUNC_CONTROL"].ToString()
                    };

                    temp.Add(model.FuncId, model);
                    if (model.FuncLevel > 0 && model.FuncLevel < min)
                    {
                        min = model.FuncLevel;
                    }
                    if (model.FuncParentId != null)
                    {
                        map.Add(model.FuncId, (decimal)model.FuncParentId);
                    }

                }
                foreach (var key in map.Keys)
                {
                    if (temp.ContainsKey(key) && temp.ContainsKey(map[key]))
                    {
                        temp[map[key]].ChildFunctions.Add(temp[key]);
                    }
                }
                return temp.Values.ToList();
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetFunctionListByUser: {0}", ex.ToString()));
                return null;
            }
        }

        public IList<FunctionViewModel> GetFunctionsList()
        {
            try
            {
                var datas = new List<FunctionViewModel>();
                var sql = CommonHelper.GetProcedureForPkqSystemSupport("BO_FUNCTIONS_GETLISTFUNC");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                };
                oracParams[0].Direction = ParameterDirection.Output;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);

                //De quy 
                var temp = new Dictionary<decimal, FunctionViewModel>();
                var map = new Dictionary<decimal, decimal>();
                int min = 100;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new FunctionViewModel()
                    {
                        FuncId = Int64.Parse(dt.Rows[i]["FUNC_ID"].ToString()),
                        FuncCode = dt.Rows[i]["FUNC_CODE"].ToString(),
                        FuncName = dt.Rows[i]["FUNC_NAME"].ToString(),
                        FuncUrl = dt.Rows[i]["FUNC_URL"].ToString(),
                        FuncOrder = Int32.Parse(dt.Rows[i]["FUNC_ORDER"].ToString()),
                        FuncDisplay = dt.Rows[i]["FUNC_DISPLAY"].ToString(),
                        FuncLevel = Int32.Parse(dt.Rows[i]["FUNC_LEVEL"].ToString()),
                        FuncParentId = Int64.Parse(dt.Rows[i]["PARENT_ID"].ToString())
                    };
                    temp.Add(model.FuncId, model);
                    if (model.FuncLevel > 0 && model.FuncLevel < min)
                    {
                        min = model.FuncLevel;
                    }
                    if (model.FuncParentId != null)
                    {
                        map.Add(model.FuncId, (decimal)model.FuncParentId);
                    }
                }
                foreach (var key in map.Keys)
                {
                    if (temp.ContainsKey(key) && temp.ContainsKey(map[key]))
                    {
                        temp[map[key]].ChildFunctions.Add(temp[key]);
                    }
                }
                foreach (var item in temp.Values)
                {
                    datas.Add(item);
                }
                return datas;
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetFunctionList: {0}", ex.ToString()));
                return null;

            }

        }

        public bool CheckUserAccessFunction(string userName, decimal funcId)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_FUNCTIONS_GetPerByUserName");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_user_name", OracleType.VarChar),
                    new OracleParameter("p_func_id", OracleType.Number)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = userName;
                oracParams[2].Value = funcId;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                return dt.Rows[0][0].ToString() == "1";
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("CheckUserAccessFunction: {0}", ex.ToString()));
                return false;
            }
        }

        public IList<FunctionViewModel> GetFunctionByRoleName(string role, decimal? parentId, string funcCode, string funcdisplay)
        {
            try
            {
                var sql = CommonHelper.GetProcedureForPkqSystem("BO_FUNCTIONS_GetPerByRoleName");
                var oracParams = new[]
                {
                    new OracleParameter("p_out", OracleType.Cursor),
                    new OracleParameter("p_role", OracleType.VarChar),
                    new OracleParameter("p_parent_id", OracleType.Number),
                    new OracleParameter("p_func_code", OracleType.VarChar),
                    new OracleParameter("p_func_display", OracleType.Char)
                };
                oracParams[0].Direction = ParameterDirection.Output;
                oracParams[1].Value = role;
                oracParams[2].Value = parentId;
                oracParams[3].Value = funcCode;
                oracParams[4].Value = funcdisplay;
                var dt = db.ExecuteDataTable(CommandType.StoredProcedure, sql, oracParams);
                //De quy 
                var temp = new Dictionary<decimal, FunctionViewModel>();
                var map = new Dictionary<decimal, decimal>();
                int min = 100;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new FunctionViewModel()
                    {
                        FuncId = Int64.Parse(dt.Rows[i]["FUNC_ID"].ToString()),
                        FuncCode = dt.Rows[i]["FUNC_CODE"].ToString(),
                        FuncName = dt.Rows[i]["FUNC_NAME"].ToString(),
                        FuncUrl = dt.Rows[i]["FUNC_URL"].ToString(),
                        FuncOrder = Int32.Parse(dt.Rows[i]["FUNC_ORDER"].ToString()),
                        FuncDisplay = dt.Rows[i]["FUNC_DISPLAY"].ToString(),
                        FuncLevel = Int32.Parse(dt.Rows[i]["FUNC_LEVEL"].ToString()),
                        FuncParentId = Int64.Parse(dt.Rows[i]["PARENT_ID"].ToString()),
                        FuncControl = dt.Rows[i]["FUNC_CONTROL"].ToString()
                    };

                    temp.Add(model.FuncId, model);
                    if (model.FuncLevel > 0 && model.FuncLevel < min)
                    {
                        min = model.FuncLevel;
                    }
                    if (model.FuncParentId != null)
                    {
                        map.Add(model.FuncId, (decimal)model.FuncParentId);
                    }

                }
                foreach (var key in map.Keys)
                {
                    if (temp.ContainsKey(key) && temp.ContainsKey(map[key]))
                    {
                        temp[map[key]].ChildFunctions.Add(temp[key]);
                    }
                }
                return temp.Values.ToList();
            }
            catch (Exception ex)
            {
                NLogHelper.Logger.Error(string.Format("GetFunctionListByUser: {0}", ex.ToString()));
                return null;
            }
        }
    }
}