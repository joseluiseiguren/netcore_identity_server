using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wSiteMvc2.Models;

namespace wSiteMvc2.Controllers
{
    //solo admin's pueden ingresar a este controller
    [Authorize(Policy = "policyadmin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //get user name
            var name = User.Claims.ToList().Where(p => p.Type.Equals("name", System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value;
            ViewBag.UserName = name;

            //get user role
            var role = User.Claims.ToList().Where(p => p.Type.Equals("role", System.StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value;
            ViewBag.UserRole = role;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
