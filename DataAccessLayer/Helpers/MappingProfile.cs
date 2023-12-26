using AutoMapper;
using ModelsLayer.DTO;
using ModelsLayer.Entity;

namespace DataAccessLayer.Helpers
{
    public class MappingProfile : Profile
	{
		public MappingProfile()
		{

			CreateMap<StudentsDTO, Students>().ReverseMap(); 
			CreateMap<Students, StudentsDTO>().ReverseMap();
            CreateMap<TeachersDTO, Teachers>().ReverseMap(); 
			CreateMap<CoursesDTO, Courses>().ReverseMap();   
			CreateMap<ClassesDTO, Classes>().ReverseMap();    
			CreateMap<CategoriesDTO, Categories>().ReverseMap();
			CreateMap<AddressesDTO, Addresses>().ReverseMap();
			CreateMap<Users, UsersDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)).ReverseMap();
			CreateMap<RolesDTO, Roles>().ReverseMap();
            CreateMap<CreateCategoryDTO, Categories>().ReverseMap();
            CreateMap<CreateClassDTO, Classes>().ReverseMap();
            CreateMap<CreateCourseDTO, Courses>().ReverseMap();
            CreateMap<CreateTeacherDTO, Teachers>().ReverseMap();
        }
    }
}
