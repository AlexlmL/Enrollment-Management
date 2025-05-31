namespace EnrollmentManagement.Dtos
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string StudentName { get; set; }
        public string CourseTitle { get; set; }
    }
}
