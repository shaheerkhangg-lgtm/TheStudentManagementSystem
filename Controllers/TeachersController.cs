using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Data;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    [Authorize]
    public class TeachersController : Controller
    {
        private readonly AppDbContext _db;

        public TeachersController(AppDbContext db)
        {
            _db = db;
        }

        // Shows the list of teachers
        public IActionResult Index()
        {
            var teachers = _db.Teachers.OrderBy(t => t.Name).ToList();
            return View(teachers);
        }

        // Opens the empty Add Teacher form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Runs when the user clicks Save Teacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Subject,Email")] Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return View(teacher);
            }

            teacher.Name = teacher.Name.Trim();
            teacher.Subject = teacher.Subject.Trim();
            teacher.Email = teacher.Email.Trim().ToLower();

            // Stop two teachers with the same email
            if (_db.Teachers.Any(t => t.Email == teacher.Email))
            {
                ModelState.AddModelError("Email", "A teacher with this email already exists.");
                return View(teacher);
            }

            try
            {
                _db.Teachers.Add(teacher);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View(teacher);
            }

            TempData["Success"] = "Teacher added successfully.";
            return RedirectToAction("Index");
        }

        // Deletes one teacher (only an Admin can do this)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Delete(int id)
        {
            var teacher = _db.Teachers.Find(id);

            if (teacher == null)
            {
                TempData["Error"] = "Teacher not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _db.Teachers.Remove(teacher);
                _db.SaveChanges();
                TempData["Success"] = "Teacher deleted.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Could not delete this teacher. Please try again.";
            }

            return RedirectToAction("Index");
        }
    }
}