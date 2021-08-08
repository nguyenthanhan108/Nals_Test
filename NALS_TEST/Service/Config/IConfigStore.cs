using NALSTEST.Models.SystemParameter.ConfigModel;
using System.Collections.Generic;
using System.Data;
namespace NALSTEST.Service.Config
{
    public interface IConfigService
    {
        DataTable GetList(string code, string value, string desc, string status);
        bool Insert(ConfigCreateViewModel model, ref string loidb);
        bool Update(ConfigCreateViewModel model, ref string loidb);
        bool ChangeStatus(string code, string status, ref string loidb);
        DataTable GetConfigById(string code);
        string GetConfigByValues(string code);
        //DataTable IsCheckUserName(string sServiceName);
    }
}