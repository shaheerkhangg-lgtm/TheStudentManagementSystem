using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Data;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    [Authorize]
    public class FeesController : Controller
    {
        private readonly AppDbContext _db;

        public FeesController(AppDbContext db)
        {
            _db = db;
        }

        // Shows all fees
        public IActionResult Index()
        {
            var fees = _db.Fees
                .Include(f => f.Student)
                .OrderByDescending(f => f.Id)
                .ToList();

            return View(fees);
        }

        // Opens the empty Add Fee form
        [HttpGet]
        public IActionResult Create()
        {
            LoadStudents();
            return View(new Fee { Status = "Unpaid" });
        }

        // Runs when the user clicks Save Fee
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("StudentId,ForMonth,Amount,Status")] Fee fee)
        {
            if (!_db.Students.Any(s => s.Id == fee.StudentId))
            {
                ModelState.AddModelError("StudentId", "Please select a student.");
            }

            if (fee.Status != "Paid" && fee.Status != "Unpaid")
            {
                ModelState.AddModelError("Status", "Please choose a status.");
            }

            if (!ModelState.IsValid)
            {
                LoadStudents();
                return View(fee);
            }

            fee.ForMonth = fee.ForMonth.Trim();

            try
            {
                _db.Fees.Add(fee);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                LoadStudents();
                return View(fee);
            }

            TempData["Success"] = "Fee added successfully.";
            return RedirectToAction("Index");
        }

        // Deletes one fee (only an Admin can do this)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Delete(int id)
        {
            var fee = _db.Fees.Find(id);

            if (fee == null)
            {
                TempData["Error"] = "Fee not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _db.Fees.Remove(fee);
                _db.SaveChanges();
                TempData["Success"] = "Fee deleted.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Could not delete this fee. Please try again.";
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