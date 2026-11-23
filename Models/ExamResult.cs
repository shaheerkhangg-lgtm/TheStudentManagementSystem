using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheStudentManagementSystem.Models
{
    public class ExamResult
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ValidateNever]
        public Student Student { get; set; }

        [Required(ErrorMessage = "Please enter the subject.")]
        [StringLength(50, ErrorMessage = "Subject can have up to 50 characters.")]
        public string Subject { get; set; }

        [Range(0, 1000, ErrorMessage = "Marks must be between 0 and 1000.")]
        public int MarksObtained { get; set; }

        [Range(1, 1000, ErrorMessage = "Total marks must be between 1 and 1000.")]
        public int TotalMarks { get; set; }
    }
}