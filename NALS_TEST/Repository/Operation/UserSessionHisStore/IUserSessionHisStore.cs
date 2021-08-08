using System.Collections.Generic;
using NALSTEST.Models.Operation.UserSessionHis;
using System.Data;
namespace NALSTEST.Repository.Operation.UserSessionHisStore
{
    public interface IUserSessionHisStore
    {
        DataTable GetList(string userName, string frd, string td,
            int pageIndex = 1, int pageSize = 20);
    }
}