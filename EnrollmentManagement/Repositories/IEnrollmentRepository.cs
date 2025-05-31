using EnrollmentManagement.Data;
using EnrollmentManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EnrollmentManagement.Repositories
{
    public interface IEnrollmentRepository : IGenericRepository<Enrollment>
    {
        Task<bool> ExistsDuplicateAsync(int studentId, int courseId);
    }

    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Enrollment entity) =>
            await _context.Enrollments.AddAsync(entity);

        public void Delete(Enrollment entity) =>
            _context.Enrollments.Remove(entity);

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Enrollments.AnyAsync(e => e.Id == id);

        public async Task<bool> ExistsAsync(Expression<Func<Enrollment, bool>> predicate) =>
            await _context.Enrollments.AnyAsync(predicate);

        public async Task<bool> ExistsDuplicateAsync(int studentId, int courseId) =>
            await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);

        public async Task<IEnumerable<Enrollment>> GetAllAsync() =>
            await _context.Enrollments.Include(e => e.Student).Include(e => e.Course).ToListAsync();

        public async Task<Enrollment> GetByIdAsync(int id) =>
            await _context.Enrollments.Include(e => e.Student).Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id == id);

        public void Update(Enrollment entity) =>
            _context.Enrollments.Update(entity);
    }

}
