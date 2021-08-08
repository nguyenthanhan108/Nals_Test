using NALSTEST.Models.SystemParameter.ConfigModel;
using System.Collections.Generic;
using System.Data;
namespace NALSTEST.Repository.SystemParameter.ConfigStore
{
    public interface IConfigStore
    {
        DataTable GetList(string code, string value, string desc, string status);
        bool Insert(ConfigCreateViewModel model, ref string loidb);
        string Update(ConfigCreateViewModel model, ref string loidb);
        bool ChangeStatus(string code, string status, ref string loidb);
        DataTable GetConfigById(string code);
        bool Delete(string code);
        bool ClearCache();
    }
}