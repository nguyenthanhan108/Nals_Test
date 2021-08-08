using NALSTEST.Models.Operation.UserSession;
using NALSTEST.Repository.Operation.UserSessionStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NALSTEST.Helpers;
namespace NALSTEST.Service.UserSession
{
    public class UserSessionService
    {
        UserSessionStoreService usersessionStore = new UserSessionStoreService();
        public IList<UserSessionViewModel> GetList(string userName, string branch, out int total, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var dt = usersessionStore.GetList(userName, pageIndex, pageSize);
                if (dt.Rows.Count > 0)
                {
                    total = dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["TOTALROWS"].ToString()) : 0;
                    var datas = new List<UserSessionViewModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var model = new UserSessionViewModel()
                        {
                            Id = Convert.ToInt32(dt.Rows[i]["USES_ID"].ToString()),
                            Ordinal = (pageIndex - 1) * pageSize + i + 1,
                            UserName = dt.Rows[i]["USERNAME"].ToString(),
                            IP = dt.Rows[i]["USES_IP"].ToString(),
                            StartDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["FROMDATE"].ToString()),
                            TimeoutDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["TIMEOUTDATE"].ToString()),
                            AccessDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["ACCESSDATE"].ToString()),
                            Status = "true"
                        };
                        datas.Add(model);
                    }
                    return datas;
                }
                else
                {
                    total = 0;
                    return new List<UserSessionViewModel>();
                }
            }
            catch (Exception ex)
            {
                total = 0;
                return new List<UserSessionViewModel>();
            }
        }
    }
}