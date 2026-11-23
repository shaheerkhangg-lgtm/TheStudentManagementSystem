namespace TheStudentManagementSystem.Models
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Accountant = "Accountant";
        public const string Receptionist = "Receptionist";
        public const string Pending = "Pending";

        // Everyone who has been approved
        public const string AllStaff = "Admin,Teacher,Accountant,Receptionist";
    }
}