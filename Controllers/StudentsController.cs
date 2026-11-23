using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Data;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly AppDbContext _db;

        public StudentsController(AppDbContext db)
        {
            _db = db;
        }

        // Shows the list of students
        public IActionResult Index()
        {
            var students = _db.Students.OrderBy(s => s.Name).ToList();
            return View(students);
        }

        // Opens the empty Add Student form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Runs when the user clicks Save Student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string email = model.Email.Trim().ToLower();

            // Stop two students with the same email
            if (_db.Students.Any(s => s.Email == email))
            {
                ModelState.AddModelError("Email", "A student with this email already exists.");
                return View(model);
            }

            var student = new Student
            {
                Name = model.Name.Trim(),
                ClassName = model.ClassName.Trim(),
                Email = email
            };

            try
            {
                _db.Students.Add(student);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                return View(model);
            }

            TempData["Success"] = "Student added successfully.";
            return RedirectToAction("Index");
        }

        // Deletes one student (only an Admin can do this) (NEW)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Delete(int id)
        {
            var student = _db.Students.Find(id);

            if (student == null)
            {
                TempData["Error"] = "Student not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _db.Students.Remove(student);
                _db.SaveChanges();
                TempData["Success"] = "Student deleted.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Could not delete this student. Please try again.";
            }

            return RedirectToAction("Index");
        }
    }
}