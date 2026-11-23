using Microsoft.AspNetCore.Mvc;

namespace TheStudentManagementSystem.Models
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
