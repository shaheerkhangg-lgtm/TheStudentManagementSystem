using System.ComponentModel.DataAnnotations;

namespace TheStudentManagementSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your email.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be 5 to 50 characters.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be 8 to 20 characters.")]
        public string Password { get; set; }
    }
}