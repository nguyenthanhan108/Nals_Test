using NALSTEST.Models.Operation.UserSessionHis;
using NALSTEST.Repository.Operation.UserSessionHisStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NALSTEST.Helpers;

namespace NALSTEST.Service.UserSessionHistory
{
    public class UserSessionHistoryService
    {
        UserSessionHisService usersessionhisStore = new UserSessionHisService();
        public IList<UserSessionHisViewModel> GetList(string userName, string branch, string frd, string td, out decimal total, int pageIndex = 1, int pageSize = 20)
        {
            try
            {
                var dt = usersessionhisStore.GetList(userName, frd, td, pageIndex, pageSize);
                if (dt.Rows.Count > 0)
                {
                    total = dt.Rows.Count > 0 ? Convert.ToDecimal(dt.Rows[0]["TOTALROWS"].ToString()) : 0;
                    var datas = new List<UserSessionHisViewModel>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var model = new UserSessionHisViewModel()
                        {
                            Ordinal = (pageIndex - 1) * pageSize + i + 1,
                            UserName = dt.Rows[i]["USERNAME"].ToString(),
                            IP = dt.Rows[i]["USES_IP"].ToString(),
                            FromDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["FROMDATE"].ToString()),
                            ToDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[i]["TODATE"].ToString())
                        };
                        datas.Add(model);
                    }
                    return datas;
                }
                total = 0;
                return new List<UserSessionHisViewModel>();
            }
            catch (Exception ex)
            {
                total = 0;
                return new List<UserSessionHisViewModel>();
            }
        }

        //public UserSessionLogViewModel GetById(decimal id)
        //{
        //    try
        //    {

        //        var dt = mobileStore.GetById(id);
        //        return new MobileBankingLogViewModel()
        //        {
        //            Id = Convert.ToDecimal(dt.Rows[0]["SEQ_NO"].ToString()),
        //            MobileNo = dt.Rows[0]["USER_NAME"].ToString(),
        //            Device = dt.Rows[0]["DEVICE_TYPE"].ToString(),
        //            OS = dt.Rows[0]["DEVICE_KIND"].ToString(),
        //            Request = dt.Rows[0]["REQUEST_DESC"].ToString(),
        //            RespondsedDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[0]["SENT_TIME"].ToString()),
        //            RequestDate = CommonHelper.ToDateTimeStringDDMMYYY(dt.Rows[0]["RECEIVED_TIME"].ToString()),
        //            Status = dt.Rows[0]["RES_CODE"].ToString() == "00" ? "Success" : "Fail",
        //            Description = dt.Rows[0]["MSG_CONTENT"].ToString(),
        //            ResCode = dt.Rows[0]["RES_CODE"].ToString(),
        //            MsgCode = dt.Rows[0]["MSG_CODE"].ToString(),
        //            DeviceID = dt.Rows[0]["DEVICE_ID"].ToString()
        //        };
        //    }
        //    catch (Exception ex)
        //    {

        //        return new MobileBankingLogViewModel();
        //    }
        //} 
        
    }
}