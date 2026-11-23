using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheStudentManagementSystem.Data;
using TheStudentManagementSystem.Models;

namespace TheStudentManagementSystem.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly AppDbContext _db;

        public AttendanceController(AppDbContext db)
        {
            _db = db;
        }

        // Shows all attendance records
        public IActionResult Index()
        {
            var records = _db.AttendanceRecords
                .Include(a => a.Student)
                .OrderByDescending(a => a.Date)
                .ThenBy(a => a.Student.Name)
                .ToList();

            return View(records);
        }

        // Opens the empty Mark Attendance form
        [HttpGet]
        public IActionResult Create()
        {
            LoadStudents();
            return View(new AttendanceRecord { Date = DateTime.Today, Status = "Present" });
        }

        // Runs when the user clicks Save Attendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("StudentId,Date,Status")] AttendanceRecord entry)
        {
            if (!_db.Students.Any(s => s.Id == entry.StudentId))
            {
                ModelState.AddModelError("StudentId", "Please select a student.");
            }

            if (entry.Status != "Present" && entry.Status != "Absent" && entry.Status != "Late")
            {
                ModelState.AddModelError("Status", "Please choose a status.");
            }

            var day = entry.Date.Date;

            // Stop two records for the same student on the same day
            if (ModelState.IsValid && _db.AttendanceRecords.Any(a => a.StudentId == entry.StudentId && a.Date == day))
            {
                ModelState.AddModelError("", "Attendance for this student on this date is already saved.");
            }

            if (!ModelState.IsValid)
            {
                LoadStudents();
                return View(entry);
            }

            entry.Date = day;

            try
            {
                _db.AttendanceRecords.Add(entry);
                _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Something went wrong. Please try again.");
                LoadStudents();
                return View(entry);
            }

            TempData["Success"] = "Attendance saved successfully.";
            return RedirectToAction("Index");
        }

        // Deletes one attendance record (only an Admin can do this)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public IActionResult Delete(int id)
        {
            var record = _db.AttendanceRecords.Find(id);

            if (record == null)
            {
                TempData["Error"] = "Attendance record not found.";
                return RedirectToAction("Index");
            }

            try
            {
                _db.AttendanceRecords.Remove(record);
                _db.SaveChanges();
                TempData["Success"] = "Attendance record deleted.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Could not delete this record. Please try again.";
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