using Microsoft.AspNetCore.Mvc;

namespace TheStudentManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
