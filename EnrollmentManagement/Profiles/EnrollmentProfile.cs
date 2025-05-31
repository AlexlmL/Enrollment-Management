using AutoMapper;
using EnrollmentManagement.Dtos;
using EnrollmentManagement.Models;

namespace EnrollmentManagement.Profiles
{
    public class EnrollmentProfile : Profile
    {
        public EnrollmentProfile()
        {
            CreateMap<Enrollment, EnrollmentDto>()
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
                .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title));

            CreateMap<CreateEnrollmentDto, Enrollment>();
        }
    }
}
