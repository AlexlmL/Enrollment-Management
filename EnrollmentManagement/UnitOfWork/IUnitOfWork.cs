using EnrollmentManagement.Repositories;

namespace EnrollmentManagement.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEnrollmentRepository Enrollments { get; }

        IStudentRepository Students { get; }
        ICourseRepository Courses { get; }

        Task<int> CompleteAsync();
    }

}
