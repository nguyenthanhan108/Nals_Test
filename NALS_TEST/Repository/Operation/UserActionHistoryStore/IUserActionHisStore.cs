using System.Collections.Generic;
using NALSTEST.Models.Operation.UserActionHistory;
using System.Data;
namespace NALSTEST.Repository.Operation.UserActionHistoryStore
{
    public interface IUserActionHisStore
    {
        DataTable GetList(string userName, string actionType, string frdate, string todate, int pageIndex = 1, int pageSize = 10);
        DataTable GetById(decimal id);
    }
}