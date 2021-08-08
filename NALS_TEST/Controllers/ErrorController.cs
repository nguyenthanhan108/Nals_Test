using NALSTEST.Repository.FunctionStore;
using NALSTEST.Service.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NALSTEST.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/
        private UserService _userStoreService;
        private IFunctionStore _functionStoreService;
        public ErrorController()
        {
            _userStoreService = new UserService();
            _functionStoreService = new FunctionStoreService();
        }
        public ActionResult Index(AccountController model)
        {
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            ViewBag.UserLogin = userLogin;
            //GetViewData();
            GetFunctionsView(userLogin.UserId);
            return View();
        }
         private void GetFunctionsView(decimal userId)
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
        }
    }
}
