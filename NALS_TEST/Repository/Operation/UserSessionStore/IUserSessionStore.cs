using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NALSTEST.Models.Operation.UserSession;
using System.Data;
namespace NALSTEST.Repository.Operation.UserSessionStore
{
    public interface IUserSessionStore
    {
        DataTable GetList(string userName, int pageIndex = 1, int pageSize = 10);
        void Update(string status, string userName);
        void UpdateEndSession(string userName, string status);
    }
}
