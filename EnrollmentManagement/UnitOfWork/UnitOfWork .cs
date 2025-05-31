using EnrollmentManagement.Data;
using EnrollmentManagement.Repositories;

namespace EnrollmentManagement.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IEnrollmentRepository Enrollments { get; }
        public IStudentRepository Students { get; }
        public ICourseRepository Courses { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Enrollments = new EnrollmentRepository(_context);
            Students = new StudentRepository(_context);
            Courses = new CourseRepository(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }

}
