using System.Linq;
using System.Web.Mvc;
using NALSTEST.Models.UserModel;
using NALSTEST.Repository.FunctionStore;
using NALSTEST.Service.User;
using NALSTEST.Service.Role;

namespace NALSTEST.Controllers
{
    public class HomeController : Controller
    {
        private UserService _userStoreService;
        private RoleService _roleStoreService;
        private IFunctionStore _functionStoreService;

        public HomeController()
        {
            _userStoreService = new UserService();
            _roleStoreService = new RoleService();
            _functionStoreService = new FunctionStoreService();
        }
        public ActionResult Index()
        {
            var model = new UserProfileViewModel();
            if (User.Identity.IsAuthenticated)
            {
                var userLogin = _userStoreService.GetUserByName(User.Identity.Name);
                if (userLogin == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.UserLogin = userLogin;
                GetFunctionsView(userLogin.UserId);
                model = _userStoreService.GetProfileUser(userLogin.UserId);
               
                return View(model);
            }
            return RedirectToAction("Login", "Account");

        }
        private void GetFunctionsView(decimal userId)
        {
            var functions = _functionStoreService.GetFunctionsByUserId(userId, null, null, "1");
            ViewBag.FuncsLeftMenu = functions;
            ViewBag.FuncsTopMenu = functions.Where(x => x.FuncParentId == 0).ToList();
        }

    }
}