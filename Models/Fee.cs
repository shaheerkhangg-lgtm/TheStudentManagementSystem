using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TheStudentManagementSystem.Models
{
    public class Fee
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        [ValidateNever]
        public Student Student { get; set; }

        [Required(ErrorMessage = "Please enter the month, for example November 2026.")]
        [StringLength(30, ErrorMessage = "Month can have up to 30 characters.")]
        public string ForMonth { get; set; }

        [Range(1, 10000000, ErrorMessage = "Please enter a valid amount.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please choose a status.")]
        [StringLength(10)]
        public string Status { get; set; }
    }
}