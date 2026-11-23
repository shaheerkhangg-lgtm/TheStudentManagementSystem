using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Data;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    [Authorize]
    public class ResultsController : Controller
    {
        private readonly AppDbContext _db;

        public ResultsController(AppDbContext db)
        {
            _db = db;
        }

        // Shows all results
        public IActionResult Index()
        {
            var results = _db.ExamResults
                .Include(r => r.Student)
                .OrderByDescending(r => r.Id)
                .ToList();

            return View(results);
        }

        // Opens the empty Add Result form
        [HttpGet]
        public IActionResult Create()
        {
            LoadStudents();
            return View();
        }

        // Runs when the user clicks Save Result
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("StudentId,Subject,MarksObtained,TotalMarks")] ExamResult exam)
        {
            if (!_db.Students.Any(s => s.Id == exam.StudentId))
            {
                ModelState.AddModelError("StudentId", "Please select a student.");
            }

            if (exam.MarksObtained > exam.TotalMarks)
            {
                ModelState.AddModelError("MarksObtained", "Marks cannot be more than total marks.");
            }

            if (!ModelState.IsValid)
            {
                LoadStudents();
                return View(exam);
            }

            exam.Subject = exam.Subject.Trim();

            try
            {
                _db.ExamResults.Add(exam);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                LoadStudents();
                return View(exam);
            }

            TempData["Success"] = "Result added successfully.";
            return RedirectToAction("Index");
        }

        // Deletes one result (only an Admin can do this)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Delete(int id)
        {
            var result = _db.ExamResults.Find(id);

            if (result == null)
            {
                TempData["Error"] = "Result not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _db.ExamResults.Remove(result);
                _db.SaveChanges();
                TempData["Success"] = "Result deleted.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Could not delete this result. Please try again.";
            }

            return RedirectToAction("Index");
        }

        // Fills the student dropdown
        private void LoadStudents()
        {
            ViewBag.Students = _db.Students
                .OrderBy(s => s.Name)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name + " (" + s.ClassName + ")"
                })
                .ToList();
        }
    }
}