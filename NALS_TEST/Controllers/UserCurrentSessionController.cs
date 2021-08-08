using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NALSTEST.Filters;
using NALSTEST.Helpers;
using NALSTEST.Models.FunctionModel;
using NALSTEST.Repository.FunctionStore;
using NALSTEST.Repository.Operation.UserSessionStore;
using NALSTEST.Repository.UserStore;
using NALSTEST.Service.User;
using NALSTEST.Service.UserSession;
using NALSTEST.Service.Config;

namespace NALSTEST.Controllers
{
    [HandleError]
    [Authorize]
    public class UserCurrentSessionController : Controller
    {
        private UserService _userStoreService;
        private IFunctionStore _functionStoreService;
        private UserSessionService _userSessionStoreService;

        public UserCurrentSessionController()
        {
            _userStoreService = new UserService();
            _userSessionStoreService = new UserSessionService();
            _functionStoreService = new FunctionStoreService();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncSessionhistLisId)]
        public ActionResult Index()
        {
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            ViewBag.UserLogin = userLogin;            
            GetFunctionsView(userLogin.UserId);
            return View();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncSessionhistLisId)]
        [HttpGet]
        public JsonResult GetList(string userName, string branch, string pageIndex, string pageSize)
        {
            userName = CommonHelper.ReplaceSpecialCharacter(userName);
            branch = CommonHelper.ReplaceSpecialCharacter(branch);
            pageIndex = CommonHelper.ReplaceSpecialCharacter(pageIndex);
            pageSize = CommonHelper.ReplaceSpecialCharacter(pageSize);
            int total = 0;
            var records = _userSessionStoreService.GetList(userName, branch, out total, Convert.ToInt32(pageIndex),
                Convert.ToInt32(pageSize));
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
        private void GetFunctionsView(decimal userId)
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
        }
    }
}