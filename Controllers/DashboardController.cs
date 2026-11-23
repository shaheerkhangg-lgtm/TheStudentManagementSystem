using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    [Authorize(Roles = AppRoles.AllStaff)]
    public class DashboardController : Controller
    {
        // The welcome screen shown right after login
        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult Index()
        {
            // Fake list for now. Part 3 makes it real.
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Ali Khan", ClassName = "Class 8", Email = "ali@gmail.com" },
                new Student { Id = 2, Name = "Sara Ahmed", ClassName = "Class 9", Email = "sara@gmail.com" },
                new Student { Id = 3, Name = "Usman Ali", ClassName = "Class 7", Email = "usman@gmail.com" }
            };

            return View(students);
        }
    }
}