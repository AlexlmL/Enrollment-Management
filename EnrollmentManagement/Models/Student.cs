namespace EnrollmentManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
