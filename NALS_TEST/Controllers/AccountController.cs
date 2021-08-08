using NALSTEST.Models.UserModel;
using NALSTEST.Repository.FunctionStore;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using NALSTEST.Helpers;
using NALSTEST.Repository.LogUserAction;
using NALSTEST.Models.FunctionModel;
using NALSTEST.Models.LogUserAction;
using NALSTEST.Service.User;
using NALSTEST.Service.Config;

namespace NALSTEST.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        private UserService _userStoreService;
        private IFunctionStore _functionStoreService;
        private static readonly log4net.ILog log =
         log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ILogUserAction _logUserActionService;
        ConfigService config = new ConfigService();
        public AccountController()
        {
            _userStoreService = new UserService();
            _functionStoreService = new FunctionStoreService();
            _logUserActionService = new LogUserActionService();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {          
            try
            {
                if (CommonHelper.ReplaceSpecialCharacter(model.UserName).Length <= 0 || model.UserName == null || model.UserName == "")
                {
                    ModelState.AddModelError("LoginError", "Vui lòng nhập Tên đăng nhập.");
                }
                else if (CommonHelper.ReplaceSpecialCharacter(model.Password).Length <= 0 || model.Password == null || model.Password == "")
                {
                    ModelState.AddModelError("LoginError", "Vui lòng nhập Mật khẩu.");
                }
                else if (CommonHelper.ReplaceSpecialCharacter(model.Password).Length < 6)
                {
                    ModelState.AddModelError("LoginError", "Mật khẩu phải từ 6-20 ký tự, bao gồm ít nhất 1 ký tự số và 1 ký tự chữ");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        string codelogin = "";
                        var ip = Request.ServerVariables["REMOTE_ADDR"];
                        //if (_userStoreService.CheckSessionOut(model.UserName, DateTime.Now.ToString()))
                        //{
                            var check = _userStoreService.Login(model.UserName, model.Password, ip, ref codelogin);
                            if (check)
                            {
                                FormsAuthentication.SetAuthCookie(model.UserName, false);
                                {
                                    var userLogin = _userStoreService.GetUserByName(model.UserName);
                                    var id = userLogin.UserId;
                                    var logModel = _logUserActionService.GetLogModel(id, FunctionIdConstant.FuncLoginId,
                                    LogActionTypeModel.ActionTypeEnum.LOGIN, "Login success");
                                    if (logModel != null)
                                        _logUserActionService.Insert(logModel);
                                    var lenglife = config.GetConfigByValue("PASS_X_LIFE").ToString(); //"PASS_X_LIFE" de kiem tra lan doi pass gan nhat va duoc confix duoi db
                                    if (_userStoreService.CheckLifePass(userLogin.DateUpdate, int.Parse(lenglife)))
                                    {
                                        Session["life"] = "1";
                                        return RedirectToAction("ChangePassword", new { id = id });
                                    }
                                    var passChangeCheck = _userStoreService.CheckPassChange(model.UserName);
                                    var passResetCheck = _userStoreService.CheckPassReset(model.UserName);
                                    
                                    if (passResetCheck)
                                    {
                                        return RedirectToAction("ChangePassword", new { id = id });
                                        
                                    }
                                    else if (passChangeCheck)
                                    {
                                     
                                        return RedirectToAction("Index", "Home");
                                    }

                                    return RedirectToAction("Index", "Home");
                                }
                            }
                            else
                            {
                                if (codelogin == "0")
                                {
                                    ModelState.AddModelError("LoginError", "Thông tin không hợp lệ. Vui lòng kiểm tra lại");
                                    log.FatalFormat("Error: {0}, ModelState: {1}", "Role Account has been locked.", ModelState.IsValid);
                                }
                                else if (codelogin == "3")
                                {
                                    ModelState.AddModelError("LoginError", "Thông tin không hợp lệ. Vui lòng kiểm tra lại");
                                    log.FatalFormat("Error: {0}, ModelState: {1}", "Account or password is incorrect.", ModelState.IsValid);
                                }
                                else if (codelogin == "2")
                                {
                                    ModelState.AddModelError("LoginError", "Tài khoản của quý khách đã bị khóa. Vui lòng liên hệ với Admin để được hỗ trợ");
                                    log.FatalFormat("Error: {0}, ModelState: {1}", "Your account has been locked.", ModelState.IsValid);
                                }
                                else if (codelogin == "4")
                                {
                                    ModelState.AddModelError("LoginError", "Thông tin không hợp lệ. Vui lòng kiểm tra lại");
                                    log.FatalFormat("Error: {0}, ModelState: {1}", "This Username does not exist.", ModelState.IsValid);
                                }
                                else
                                {
                                    ModelState.AddModelError("LoginError", "Hệ thống đang có sự cố database. Vui lòng kiểm tra lại.");
                                    log.FatalFormat("Error: {0}, ModelState: {1}", codelogin, ModelState.IsValid);
                                }
                                Session["dem"] = Session["dem"] + "1";
                                var configValue = config.GetConfigByValue("LOGIN_BACKEND_FAILMAX"); // cau hinh o db ve so lan sai pass
                                if (Session["dem"].ToString().Length.ToString() == configValue)
                                {
                                    Session.Remove("dem");
                                    string kq = String.Format("Tài khoản của quý khách đã bị khóa do nhập sai mật khẩu quá {0} lần. Vui lòng liên hệ với Admin để được hỗ trợ!", configValue);
                                    var checkuser = _userStoreService.CheckBlockUser(model.UserName);
                                    if (checkuser)
                                    {
                                        ModelState.AddModelError("LoginError", kq);
                                    }
                                }
                            }
                    //    }
                    //    ModelState.AddModelError("LoginError", "You are kicked out from the system because another person is logging by this account at in same time.");
                    }

                }
                return View(model);
            }
            catch (Exception ex)
            {
                log.FatalFormat("ex: {0}", ex);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            try
            {
                log.Info("LogOff Successed");
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                _userStoreService.UpdateSession(User.Identity.Name.Trim(), DateTime.Now.ToString());
                var logModel = _logUserActionService.GetLogModel(userLogin.UserId, FunctionIdConstant.FuncLogoutId,
                    LogActionTypeModel.ActionTypeEnum.LOGOUT, "Logout");
                if (logModel != null)
                    _logUserActionService.Insert(logModel);
                FormsAuthentication.SignOut();
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
            //FormsAuthentication.SignOut();
            //return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult ChangePassword(decimal id)
        {           
            //if (!CheckPermissionChangePassword(id)) return RedirectToAction("Login");
            var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
            if (userLogin == null) return RedirectToAction("Login", "Account");
            if (User.Identity.Name != userLogin.UserName) return RedirectToAction("Login");
            ViewBag.UserLogin = userLogin;
            GetFunctionsView(id);
            var model = new ChangePasswordModel() { UserId = id };
            if (Session["life"] == "1")
            {
                model.Life = "1";              
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult ChangePassword(ChangePasswordModel model)
        {
      
            int leng = int.Parse( config.GetConfigByValue("PASS_X_LAST").ToString());
            if (_userStoreService.CheckHistoryPassWord(model.UserId, model.NewPassword, leng))
            {
                if (_userStoreService.CheckPolicyPassword(model.NewPassword))
                {
                    string error ="";
                    var check = _userStoreService.ChangePassword(model.UserId, model.OldPassword, model.NewPassword,ref error);
                    String mesg = "";
                    switch (check)
                    {
                        case "0":
                            mesg = "Lỗi hệ thống: " + error;
                            break;
                        case "-1":
                            mesg = "Mật khẩu cũ không chính xác. Vui lòng kiểm tra lại";
                            break;
                        case "1":
                            mesg = "Đổi mật khẩu thành công. Qúy khách vui lòng đăng nhập lại";

                            break;
                        default:
                             mesg = "Lỗi hệ thống";
                            break;
                    }
                   
                     
                    if (check == "1")
                    {
                        var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                        var logModel = _logUserActionService.GetLogModel(userLogin.UserId,
                            FunctionIdConstant.FuncChangePasswordId, LogActionTypeModel.ActionTypeEnum.CHANGEPASSWORD,
                            string.Format("Change password: oldpassword {0}, newpassword {1}", model.OldPassword,
                                "*******"));
                        if (logModel != null) _logUserActionService.Insert(logModel);

                    }
                    return Json(mesg);
                   
                    //return RedirectToAction("Login");
                   
                   
                }
                return Json("Mật khẩu mới phải từ 6 -20 ký tự, bao gồm ít nhất 1 ký tự số và 1 ký tự chữ");
            }
            return Json(String.Format("Mật khẩu mới không được trùng với {0} mật khẩu cũ gần nhất", leng));
        }
        private void GetFunctionsView(decimal userId) // ham xu ly menu cua backend, me nu duoc cau hinh duoi db
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
            
        }
    }
}