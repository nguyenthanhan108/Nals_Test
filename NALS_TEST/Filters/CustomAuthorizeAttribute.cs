using DBConnection;
using System.Data.Common;
using System.Web;
using System.Web.Mvc;


namespace NALSTEST.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private OracleDB db = new OracleDB();
        private readonly string[] allowedroles;  
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userName = httpContext.User.Identity.Name;
           

            //foreach (var role in allowedroles)
            //{
            //    //var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&
            //    //m.IsActive == true); // checking active users with allowed roles.  
            //    //if (user.Count() > 0)
            //    //{
            //    //    authorize = true; /* return true if Entity has current user(active) with specific role */
            //    //}
            //}
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}