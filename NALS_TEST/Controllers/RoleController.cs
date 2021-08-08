using NALSTEST.Filters;
using NALSTEST.Helpers;
using NALSTEST.Models.FunctionModel;
using NALSTEST.Models.RoleModel;
using NALSTEST.Repository.FunctionStore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NALSTEST.Repository.LogUserAction;
using NALSTEST.Models.LogUserAction;
using Newtonsoft.Json;
using NALSTEST.Service.User;
using NALSTEST.Service.Role;

namespace NALSTEST.Controllers
{
    [HandleError]
    [Authorize]
    public class RoleController : Controller
    {
        private UserService _userStoreService;
        private RoleService _roleStoreService;
        private IFunctionStore _functionStoreService;
        private ILogUserAction _logUserActionService;
        string loidb = "";
        public RoleController()
        {
            _userStoreService = new UserService();
            _roleStoreService = new RoleService();
            _functionStoreService = new FunctionStoreService();
            _logUserActionService = new LogUserActionService();
        }
        // GET: Role
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncRoleListId)]
        public ActionResult Index()
        {
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            ViewBag.UserLogin = userLogin;
            GetViewData();
            GetFunctionsView(userLogin.UserId);
            return View();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncRoleListId)]
        [HttpGet]
        public JsonResult GetListRole(string roleName, string roleStatus)
        {
            roleName = CommonHelper.ReplaceSpecialCharacter(roleName);
            roleStatus = CommonHelper.ReplaceSpecialCharacter(roleStatus);
            if (string.IsNullOrEmpty(roleName) && string.IsNullOrEmpty(roleStatus))
            {
                var records = _roleStoreService.GetListRole(null, null);
                return Json(records, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var records = _roleStoreService.GetListRole(roleName, roleStatus);
                return Json(records, JsonRequestBehavior.AllowGet);
            }
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncCreateRoleId)]
        public ActionResult CreateRole()
        {
            //GetLoginInfor();
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            ViewBag.UserLogin = userLogin;
            GetFunctionsView(userLogin.UserId);
            ViewBag.FuncsList = _functionStoreService.GetFunctionsByUserId(userLogin.UserId, null, null, null);
            return View();
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncCreateRoleId)]
        [HttpPost]
        public JsonResult CreateRole(CreateRoleModel model)
        {
            var check = _roleStoreService.CreateRole(model, ref loidb);
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = JsonConvert.SerializeObject(model);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncCreateRoleId,
                    LogActionTypeModel.ActionTypeEnum.INSERT, desc);
                _logUserActionService.Insert(logModel);
            }
            var msg = check ? "Thêm mới quyền thành công" : string.Format("Quyền đã tồn tại trong hệ thống hoặc lỗi: {0}.", loidb);
            return Json(msg);
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncEditRoleId)]
        public ActionResult EditRole(decimal id)
        {
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            //if (!_roleStoreService.CheckModifyRole(id, userLogin.UserId))
              //  return RedirectToAction("Login", "Account");
            ViewBag.UserLogin = userLogin;
            GetFunctionsView(userLogin.UserId);
            ViewBag.FuncsList = _functionStoreService.GetFunctionsByUserId(userLogin.UserId, null, null, null);
            var model = _roleStoreService.GetRoleById(id);
            return View(model);
        }
         [FunctionAuthorize(FuncId = FunctionIdConstant.FuncEditRoleId)]
        [HttpPost]
        public JsonResult EditRole(CreateRoleModel model)
        {
            var check = _roleStoreService.UpdateRole(model, ref loidb);
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = JsonConvert.SerializeObject(model);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncEditRoleId,
                    LogActionTypeModel.ActionTypeEnum.UPDATE, desc);
                _logUserActionService.Insert(logModel);
            }
            var msg = check ? "Cập  nhập quyền thành công." : "Cập  nhập quyền lỗi: " + loidb;
            return Json(msg);
        }
        //[HttpPost]
        //public JsonResult DeleteRole(decimal id)
        //{
        //    var check = _roleStoreService.DeleteRole(id);
        //    var msg = check ? "Delete Role Success" : "Delete Role Error";
        //    return Json(msg);
        //}
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncBlockRoleId)]
        [HttpPost]
        public JsonResult ChangeStatusRole(decimal id)
        {
            var currentStatus = _roleStoreService.GetRoleStatus(id);
            var status = currentStatus == "1" ? "0" : "1";
            var check = _roleStoreService.ChangeStatus(id, status, ref loidb);
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = string.Format("Thay đổi trạng thái: UserId: {0}, UserName: {1}, RoleId: {2}, Status: {3}", userLogin.UserId, userLogin.UserName, id, status);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncBlockRoleId,
                    LogActionTypeModel.ActionTypeEnum.CHANGESTATUS, desc);
                _logUserActionService.Insert(logModel);
            }
            var msg = check ? "Thay đổi trạng thái thành công" : "Thay đổi trạng thái lỗi: " + loidb;
            return Json(msg);
        }
        private void GetViewData()
            {
                ViewData["RoleStatus"] = new List<RoleStatusModel>()
                {
                    new RoleStatusModel() {StatusCode = "1", StatusName = "Active"},
                    new RoleStatusModel() {StatusCode = "0", StatusName = "Inactive"}
                };
            }

            private void GetFunctionsView(decimal userId)
            {
                var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
                ViewBag.FuncsLeftMenu = functions;
                ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
            }

    }
}