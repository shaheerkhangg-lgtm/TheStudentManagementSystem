using System.ComponentModel.DataAnnotations;

namespace TheStudentManagementSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = AppRoles.Pending;
    }
}