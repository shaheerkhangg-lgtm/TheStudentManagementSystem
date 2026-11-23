using System.ComponentModel.DataAnnotations;

namespace TheStudentManagementSystem.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Please enter your full name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must be 2 to 50 characters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be 5 to 50 characters")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be 8 to 20 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
