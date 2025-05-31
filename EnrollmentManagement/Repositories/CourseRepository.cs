using EnrollmentManagement.Data;
using EnrollmentManagement.Models;

namespace EnrollmentManagement.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
    }

    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(AppDbContext context) : base(context)
        {
        }
    }
}
