using NALSTEST.Helpers;
using NALSTEST.Models.Cache;
using NALSTEST.Models.SystemParameter.ConfigModel;
using NALSTEST.Repository.SystemParameter.ConfigStore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace NALSTEST.Service.Config
{
    public class ConfigService 
    {
        ConfigStoreService configStore = new ConfigStoreService();
        public IList<ConfigModel> GetList(string code, string value, string desc, string status)
        {
            try
            {
                var datas = new List<ConfigModel>();
                var dt = configStore.GetList(code, value, desc, status);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new ConfigModel()
                    {
                        Ordinal = i + 1,
                        Code = dt.Rows[i]["CONFIG_CODE"].ToString().Trim(),
                        Value = dt.Rows[i]["CONFIG_VALUE"].ToString(),
                        Desc = dt.Rows[i]["CONFIG_DESC"].ToString(),
                        StatusId = dt.Rows[i]["CONFIG_STATUS"].ToString(),
                        Status = dt.Rows[i]["CONFIG_STATUS"].ToString() == "1"
                    };
                    datas.Add(model);
                }
                return datas;
            }
            catch (Exception ex)
            {
                return new List<ConfigModel>();
            }
        }
        public ConfigCreateViewModel GetConfigById(string code)
        {
            try
            {
                var dt = configStore.GetConfigById(code);
                var model = new ConfigCreateViewModel();
                model.Code = dt.Rows[0]["CONFIG_CODE"].ToString();
                model.Value = dt.Rows[0]["CONFIG_VALUE"].ToString();
                model.Desc = dt.Rows[0]["CONFIG_DESC"].ToString();
                return model;
            }
            catch (Exception ex)
            {
                return new ConfigCreateViewModel();
            }
        }

        public string GetConfigByValue(string code)
        {
            var dt = configStore.GetConfigByValue(code);
            return dt;
        }
    }
}