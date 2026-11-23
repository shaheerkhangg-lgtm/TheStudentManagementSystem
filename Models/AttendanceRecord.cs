using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheStudentManagementSystem.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ValidateNever]
        public Student Student { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please choose a status.")]
        [StringLength(10)]
        public string Status { get; set; }
    }
}