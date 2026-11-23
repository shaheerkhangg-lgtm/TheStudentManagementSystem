using System.ComponentModel.DataAnnotations;

namespace TheStudentManagementSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the teacher name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be 2 to 50 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the subject.")]
        [StringLength(50, ErrorMessage = "Subject can have up to 50 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter the email.")]
        [StringLength(100, ErrorMessage = "Email can have up to 100 characters.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
    }
}