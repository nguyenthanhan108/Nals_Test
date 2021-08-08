using NALSTEST.Filters;
using NALSTEST.Helpers;
using NALSTEST.Models.FunctionModel;
using NALSTEST.Models.UserModel;
using System.Linq;
using System.Web.Mvc;
using NALSTEST.Repository.LogUserAction;
using NALSTEST.Models.LogUserAction;
using Newtonsoft.Json;
using NALSTEST.Service.User;
using NALSTEST.Service.Role;
using NALSTEST.Repository.FunctionStore;

using NALSTEST.Service.Config;
using System;
using NALSTEST.Models.SystemAdministrator.UserModel;
using NALSTEST.Models.Common;


namespace NALSTEST.Controllers
{
        [HandleError]
        [Authorize]
        public class AdminController : Controller
        {
            // GET: Admin
            private static readonly log4net.ILog log =
                 log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            private UserService _userStoreService;
            private RoleService _roleStoreService;
            private FunctionStoreService _functionStoreService;
            private ILogUserAction _logUserActionService;
           // private BranchService _branchStoreService;
            private ConfigService _configStoreService;
           // private PosDetailService _posStoreService;
            string loidb = "";
            public AdminController()
            {
                _userStoreService = new UserService();
                _roleStoreService = new RoleService();
                _functionStoreService = new FunctionStoreService();
                //_branchStoreService = new BranchService();
               // _posStoreService = new PosDetailService();
                _logUserActionService = new LogUserActionService();
                _configStoreService = new ConfigService();
            }
            [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUserListId)]
            public ActionResult Index()
            {
                if (User.Identity.IsAuthenticated)
                {
                    var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                    ViewBag.UserLogin = userLogin;                  
                    GetFunctionsView(userLogin.UserId);
                    ViewData["RoleList"] = _roleStoreService.GetListRolePerUserId(userLogin.UserId);
                    var model = new BranchViewModel();
                    //_configStoreService.GetConfigByValues("BRANCHCODE");
                    //.listbranch = _branchStoreService.GetListBranch(null, null, "1","","").ToList();                    
                    //model.listpos = _posStoreService.GetListPosDetail("", "", "", "", "","", "1").ToList();
                    model.listrole = _roleStoreService.GetListRole("", "1").ToList();
                    return View(model);
                }
                return RedirectToAction("Login", "Account");
            }
             [FunctionAuthorize(FuncId = FunctionIdConstant.FuncUserListId)]
            [HttpGet]
            public JsonResult GetListUser(string un, string status,string branch, string bankpos, string role, string fromdate, string todate)
            {
                un = CommonHelper.ReplaceSpecialCharacter(un);
                status = CommonHelper.ReplaceSpecialCharacter(status).ToUpper();
                bankpos = CommonHelper.ReplaceSpecialCharacter(bankpos);
                branch = CommonHelper.ReplaceSpecialCharacter(branch);
                role = CommonHelper.ReplaceSpecialCharacter(role);
                fromdate = CommonHelper.ReplaceSpecialCharacter(fromdate);
                todate = CommonHelper.ReplaceSpecialCharacter(todate);
                if (string.IsNullOrEmpty(un) && string.IsNullOrEmpty(status) && string.IsNullOrEmpty(branch)&& string.IsNullOrEmpty(bankpos))
                {
                    var records = _userStoreService.GetListUser(null, null, null, null, role, fromdate, todate);
                    return Json(records, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var records = _userStoreService.GetListUser(un, status, branch, bankpos, role,fromdate,todate);
                    return Json(records, JsonRequestBehavior.AllowGet);
                }

            }
          [FunctionAuthorize(FuncId = FunctionIdConstant.FuncCreateUserId)]
            public ActionResult CreateUser()
            {
                if (User.Identity.IsAuthenticated)
                {
                    UserLoginViewModel userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                    if (userLogin == null) return RedirectToAction("Login", "Account");
                    ViewBag.UserLogin = userLogin;
                    GetFunctionsView(userLogin.UserId);
                    ViewData["RoleList"] = _roleStoreService.GetListRolePerUserId(userLogin.UserId);
                    UserListViewModel model = new UserListViewModel();
                   // model.listBranch = _branchStoreService.GetListBranch(null, null, "1","","").ToList();
                   // model.listPos = _posStoreService.GetListPosDetail("", "", "", "", "", "", "1").ToList();
                    return View(model);
                }
                return RedirectToAction("Login", "Account");
            }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncCreateUserId)]
        [HttpPost]
        public JsonResult CreateUser(UserCreateViewModel model)
        {
            var userCreate = User.Identity.Name;
            int result;
            string mesg;
            string userName = model.UserName;
            string Password = RandomString(8);

            if (_userStoreService.CheckPolicyPassword(Password)) //model.Password
            {
                result = _userStoreService.CreateUser(model, userCreate, Password, ref loidb);
                mesg = result == 1 ? string.Format("Thêm mới người dùng thành công.", Password) : string.Format("Tên đăng nhập đã tồn tại trên hệ thống", loidb);
            if (result == 1)
            {
                var userLogin = _userStoreService.GetUserByName(userCreate);
                model.Password = "********";
                var desc = JsonConvert.SerializeObject(model);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncCreateUserId,
                    LogActionTypeModel.ActionTypeEnum.INSERT, desc);
                _logUserActionService.Insert(logModel);               
            }
                 return Json(new { mesg, result });
            }
            mesg = "Mật khẩu phải từ 6 -20 ký tự, bao gồm ít nhất 1 ký tự số và 1 ký tự chữ";
             result=  1;
            return Json(new { mesg, result });
           
        }       
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncEditUserId)]
        public ActionResult EditUser(decimal id)
        {
            if (User.Identity.IsAuthenticated)
            {
                //if (!_userStoreService.CheckModifyUser(User.Identity.Name, id))
                //return RedirectToAction("Login", "Account");
                UserLoginViewModel userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                if (userLogin == null) return RedirectToAction("Login", "Account");
                ViewBag.UserLogin = userLogin;
                GetFunctionsView(userLogin.UserId);
                var model = _userStoreService.GetUserById(id);
                //ViewData["RoleList"] = _roleStoreService.GetListRolePerUserId(userLogin.UserId);
                model.listRole = _roleStoreService.GetListRolePerUserId(userLogin.UserId).ToList();
                //////model.listBranch = _branchStoreService.GetListBranch(null, null, "1","","").ToList();
               // model.listPos = _posStoreService.GetListPosDetail("", "", "", "", "", "", "1").ToList();
                return View(model);
            }
            return RedirectToAction("Login", "Account");

        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncEditUserId)]
        [HttpPost]
        public ActionResult EditUser(UserCreateViewModel model)
        {
            var msg = "";
            UserLoginViewModel userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            if (userLogin == null) return RedirectToAction("Login", "Account"); ;
             
            ViewBag.UserLogin = userLogin;
            var check = _userStoreService.UpdateUser(model,ref loidb);
            msg = check ? "Cập nhật người dùng thành công" : "Cập nhật người dùng thất bại: ";
           if (check)
           {
               var desc = JsonConvert.SerializeObject(model);
               var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncEditUserId,
                   LogActionTypeModel.ActionTypeEnum.UPDATE, desc);
               _logUserActionService.Insert(logModel);
           }
           return Json(msg);
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncDeleteUserId)]
        [HttpPost]
        public JsonResult DeleteUser(decimal id)
        {
            var check = _userStoreService.DeleteUser(id);
            var mesg = check ? "Delete Success" : "Delete Error";
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = string.Format("Delete City: City Code: {0}, UserName {1}", id, userLogin.UserName,
                    mesg);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncDeleteUserId,
                    LogActionTypeModel.ActionTypeEnum.DELETE, desc);
                _logUserActionService.Insert(logModel);
            }
            return Json(mesg);
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncBlockUserId)]
        [HttpPost]
        public JsonResult ChangeStatusUser(decimal id, string username)
        {
            var currentStatus = _userStoreService.GetUserStatus(id);
            var status = currentStatus == "1" ? "0" : "1";
            var check = _userStoreService.ChangeStatus(id, status,ref loidb);
            var mesg = check ? "Thay đổi trạng thái thành công" : "Thay đổi trạng thái lỗi: " + loidb;
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = string.Format("Thay đổi trạng thái: UserId: {0}, UserName {1}, Reset by: {2}", id, username, userLogin.UserName );
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncBlockUserId,
                    LogActionTypeModel.ActionTypeEnum.CHANGESTATUS, desc);
                _logUserActionService.Insert(logModel);
            }
            return Json(mesg);
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncResetPasswordId)]
        [HttpPost]
        public JsonResult ResetPassword(decimal id, string username)
        {
            string newPassword = RandomString(8);
            string loidb = "";
            var check = _userStoreService.ResetPassword(id, newPassword, ref loidb);
            var mesg = check ? string.Format("Cấp lại mật khẩu thành công.", newPassword) : "Cấp lại mật khẩu thất bại.: " + loidb;
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = string.Format("Reset password: UserId: {0}, UserName:{1}, Reset by: {2}", id, username, userLogin.UserName);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncResetPasswordId,
                    LogActionTypeModel.ActionTypeEnum.RESETPASSWORD, desc);
                _logUserActionService.Insert(logModel);
            }
            return Json(mesg);
        }
        [FunctionAuthorize(FuncId = FunctionIdConstant.FuncAuthorizationId)]
        [HttpPost]
        public JsonResult AssignRole(decimal id, string roleIds)
        {
            roleIds = roleIds.Replace("\"", "").TrimEnd(',');
            string loidb = "";
            var check = _userStoreService.AssignRole(id, roleIds, User.Identity.Name, ref loidb);
            if (check)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                var desc = string.Format("Assign role: UserId: {0}, By UserName: {1}, Role: {2}", id, userLogin.UserName,
                    roleIds);
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncAuthorizationId,
                    LogActionTypeModel.ActionTypeEnum.ASSIGNRULE, desc);
                _logUserActionService.Insert(logModel);
            }
            var msg = check ? "Thay đổi Role thành công" : "Thay đổi Role lỗi: " + loidb;
            return Json(msg);
        }      
        private void GetFunctionsView(decimal userId)
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}