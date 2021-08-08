using System;
using System.Web;
using System.Web.Mvc;
using NALSTEST.Repository.FunctionStore;
using NALSTEST.Service.Config;
using NALSTEST.Service.User;

namespace NALSTEST.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
 
    public class FunctionAuthorize : ActionFilterAttribute
    {
        public int FuncId { get; set; }
        private IFunctionStore _functionStoreService = new FunctionStoreService();
        private UserService _userStoreService = new UserService();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if(context.Session == null){

                string redirectTo = "~/Account/Login";
                if (!string.IsNullOrEmpty(context.Request.RawUrl))
                {
                    redirectTo = string.Format("~/Account/Login?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl));
                    filterContext.Result = new RedirectResult(redirectTo);
                }
            }
            var userName = context.User.Identity.Name;
            var authorize = _functionStoreService.CheckUserAccessFunction(userName, this.FuncId);
            if (!authorize) {
                string redirecTo = "~/Error/Index";
                filterContext.Result = new RedirectResult(redirecTo);
            }
            ConfigService config = new ConfigService();
            UserService _userStoreService = new UserService();
            var lenglife = config.GetConfigByValue("PASS_X_LIFE").ToString();
            var userLogin = _userStoreService.GetUserByName(userName);
            if (_userStoreService.CheckLifePass(userLogin.DateUpdate, int.Parse(lenglife)))
            {
                string redirectTo = "~/Account/ChangePassword/" + userLogin.UserId ;
                filterContext.Result = new RedirectResult(redirectTo);
            }
            var ip = context.Request.ServerVariables["REMOTE_ADDR"];
            if (_userStoreService.CheckSession(userName, ip))
            {
                string redirectTo = "~/Account/Login";
                filterContext.Result = new RedirectResult(redirectTo);
            }
            base.OnActionExecuting(filterContext);
        }        
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    bool authorize = false;
        //    var userName = httpContext.User.Identity.Name;
        //    authorize = _functionStoreService.CheckUserAccessFunction(userName, this.FuncId);
        //    //foreach (var role in allowedroles)
        //    //{
        //    //    //var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&
        //    //    //m.IsActive == true); // checking active users with allowed roles.  
        //    //    //if (user.Count() > 0)
        //    //    //{
        //    //    //    authorize = true; /* return true if Entity has current user(active) with specific role */
        //    //    //}
        //    //}
        //    return authorize;
        //}
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new HttpUnauthorizedResult();
        //}
    }
}