using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace wSiteMvc.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AccessDenied()
        {
            //get user name
            var name = User.Claims.ToList().Where(p => p.Type.Equals("name", System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value;
            ViewBag.UserName = name;

            //get user role
            var role = User.Claims.ToList().Where(p => p.Type.Equals("role", System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value;
            ViewBag.UserRole = role;

            return View();
        }
    }
}