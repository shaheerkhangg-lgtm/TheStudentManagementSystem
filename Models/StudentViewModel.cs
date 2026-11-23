using System.ComponentModel.DataAnnotations;

namespace TheStudentManagementSystem.Models
{
    public class StudentViewModel
    {
        [Required(ErrorMessage = "Please enter the student name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be 2 to 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the class.")]
        [StringLength(30, ErrorMessage = "Class can have up to 30 characters.")]
        public string ClassName { get; set; }

        [Required(ErrorMessage = "Please enter the email.")]
        [StringLength(100, ErrorMessage = "Email can have up to 100 characters.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}