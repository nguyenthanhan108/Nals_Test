using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NALSTEST.Models.SystemAdministrator.UserModel;
using NALSTEST.Models.UserModel;

namespace NALSTEST.Repository.SystemAdminstrator.UserStore
{
    public interface IUserIntegrateBIDV
    {
        UserInforBidv LoginBIDV(string userName, string password, string ip);
        bool CreateUser(string userName, string fullName, string branch, string role, string userCreate);
        void InsertSessionHis(string userId, string ip, int timeOut);
    }
}
