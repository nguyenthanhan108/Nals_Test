using NALSTEST.Models.LogUserAction;
using NALSTEST.Models.Operation.UserActionHistory;
using NALSTEST.Repository.Operation.UserActionHistoryStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NALSTEST.Helpers;

namespace NALSTEST.Service.UserActionHistory
{
    public class UserActionHistoryService
    {
        UserActionHisService useractionhisStore = new UserActionHisService();
         public List<UserActionHistoryViewModel> GetList(string userName, string actionType, string frdate, string todate, out decimal total, int pageIndex = 1, int pageSize = 10)
         {
             try
             {
                 var dt = useractionhisStore.GetList(userName, actionType, frdate, todate, pageIndex, pageSize);
                 if (dt.Rows.Count > 0)
                 {
                     total = dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0]["TOTALROWS"].ToString()) : 0;
                     var datas = new List<UserActionHistoryViewModel>();
                     for (int i = 0; i < dt.Rows.Count; i++)
                     {
                         var model = new UserActionHistoryViewModel()
                         {
                             Ordinal = (pageIndex - 1) * pageSize + i + 1,
                             Id = Convert.ToDecimal(dt.Rows[i]["USEH_ID"].ToString()),
                             UserId = Convert.ToDecimal(dt.Rows[i]["USER_ID"].ToString()),
                             UserName = dt.Rows[i]["USERNAME"].ToString(),
                             FuncId = Convert.ToDecimal(dt.Rows[i]["FUNC_ID"].ToString()),
                             FuncName = dt.Rows[i]["FUNC_URL"].ToString(),
                             ActionType = LogActionTypeModel.ActionTypeDicts[dt.Rows[i]["ACTION_TYPE"].ToString()],
                             ActionTime = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["ACTION_TIME"].ToString())
                         };
                         datas.Add(model);
                     }
                     return datas;
                 }
                 else
                 {
                     total = 0;
                     return new List<UserActionHistoryViewModel>();
                 }
             }
             catch (Exception ex)
             {
                 total = 0;
                 return new List<UserActionHistoryViewModel>(); 
             }
         }
         public UserActionHistoryViewDetailModel GetById(decimal id)
         {
             try
             {
                 var dt = useractionhisStore.GetById(id);
                 if (dt.Rows.Count > 0)
                 {
                     return new UserActionHistoryViewDetailModel()
                     {
                         UserName = dt.Rows[0]["USERNAME"].ToString(),
                         FullName = dt.Rows[0]["FULLNAME"].ToString(),
                         Function = dt.Rows[0]["FUNC_URL"].ToString(),
                         Action = LogActionTypeModel.ActionTypeDicts[dt.Rows[0]["ACTION_TYPE"].ToString()],
                         ActionTime = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[0]["ACTION_TIME"].ToString()),
                         ActionDesc = dt.Rows[0]["ACTION_DESC"].ToString(),
                         Branch = dt.Rows[0]["BRANCH_CODE"].ToString()
                     };
                 }
                 return null;
             }
             catch (Exception ex)
             {
                 return null;
             }
         }
    }
}