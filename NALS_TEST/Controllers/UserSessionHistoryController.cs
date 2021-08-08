using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NALSTEST.Filters;
using NALSTEST.Helpers;
using NALSTEST.Models.FunctionModel;
using NALSTEST.Repository.FunctionStore;
using NALSTEST.Repository.Operation.UserSessionHisStore;
using NALSTEST.Repository.UserStore;
using NALSTEST.Service.User;
using NALSTEST.Service.UserSessionHistory;
using NALSTEST.Service.Config;


namespace NALSTEST.Controllers
{
    [HandleError]
    [Authorize]
    public class UserSessionHistoryController : Controller
    {
        private UserService _userStoreService;
        private IFunctionStore _functionStoreService;
        private UserSessionHistoryService _userSessionHisService;
        public UserSessionHistoryController()
        {
            _userStoreService = new UserService();
            _functionStoreService = new FunctionStoreService();
            _userSessionHisService = new UserSessionHistoryService();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUsersessHisListId)]
        public ActionResult Index()
        {
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            ViewBag.UserLogin = userLogin;            
            GetFunctionsView(userLogin.UserId);
            return View();
        }
         [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUsersessHisListId)]
        [HttpGet]
        public JsonResult GetList(string userName, string branch, string frd,string td, string pageIndex, string pageSize)
        {
            userName = CommonHelper.ReplaceSpecialCharacter(userName);
            branch = CommonHelper.ReplaceSpecialCharacter(branch);
            frd = CommonHelper.ReplaceSpecialCharacter(frd);
            td = CommonHelper.ReplaceSpecialCharacter(td);
            pageIndex = CommonHelper.ReplaceSpecialCharacter(pageIndex);
            pageSize = CommonHelper.ReplaceSpecialCharacter(pageSize);
            decimal total = 0;
            var records = _userSessionHisService.GetList(userName, branch, frd, td, out total, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
         //public JsonResult ViewDetail(decimal id)
         //{
         //    var model = _userSessionHisService.GetById(id);
         //    return Json(model);
         //}
        private void GetFunctionsView(decimal userId)
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
        }
    }
}