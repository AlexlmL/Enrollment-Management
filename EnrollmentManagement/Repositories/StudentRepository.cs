using EnrollmentManagement.Data;
using EnrollmentManagement.Models;

namespace EnrollmentManagement.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
    }

    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AppDbContext context) : base(context)
        {
        }
    }
}
