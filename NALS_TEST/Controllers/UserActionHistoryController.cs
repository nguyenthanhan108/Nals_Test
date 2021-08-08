using System;
using System.Data;
using System.Web.Helpers;
using NALSTEST.Filters;
using NALSTEST.Helpers;
using NALSTEST.Models.FunctionModel;
using NALSTEST.Repository.FunctionStore;
using NALSTEST.Repository.Operation.UserActionHistoryStore;
using NALSTEST.Repository.UserStore;
using System.Linq;
using System.Web.Mvc;
using NALSTEST.Service.User;
using NALSTEST.Service.UserActionHistory;

namespace NALSTEST.Controllers
{
    [HandleError]
    [Authorize]
    public class UserActionHistoryController : Controller
    {
        private UserService _userStoreService;
        private IFunctionStore _functionStoreService;
        private UserActionHistoryService _userActionHisStoreService;
        public UserActionHistoryController()
        {
            _userStoreService = new UserService();
            _functionStoreService = new FunctionStoreService();
            _userActionHisStoreService = new UserActionHistoryService();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUserHisListId)]
        public ActionResult Index()
        {
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            ViewBag.UserLogin = userLogin;            
            GetFunctionsView(userLogin.UserId);
            return View();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUserHisListId)]
        [HttpGet]
        public JsonResult GetList(string userName, string actionType, string frd, string td, string pageIndex, string pageSize )
        {
            userName = CommonHelper.ReplaceSpecialCharacter(userName);
            actionType = CommonHelper.ReplaceSpecialCharacter(actionType);
            frd = CommonHelper.ReplaceSpecialCharacter(frd);
            td = CommonHelper.ReplaceSpecialCharacter(td);
            pageIndex = CommonHelper.ReplaceSpecialCharacter(pageIndex);
            pageSize = CommonHelper.ReplaceSpecialCharacter(pageSize);
            decimal total = 0;
            var records = _userActionHisStoreService.GetList(userName, actionType, frd, td, out total,Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUserHisViewId)]
        [HttpPost]
        public JsonResult ViewDetail(decimal id)
        {
            var model = _userActionHisStoreService.GetById(id);
            return Json(model);
        }
        private void GetFunctionsView(decimal userId)
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
        }
    }
}