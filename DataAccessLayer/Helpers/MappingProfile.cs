using AutoMapper;
using DataAccessLayer.Models;
using BusinessLogicLayer.DTO;

namespace DataAccessLayer.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{

			CreateMap<StudentDTO, Students>().ReverseMap(); 
			CreateMap<TeacherDTO, Teachers>().ReverseMap(); 
			CreateMap<CourseDTO, Courses>().ReverseMap();   
			CreateMap<ClassDTO, Classes>().ReverseMap();    
			CreateMap<CategoryDTO, Categories>().ReverseMap();
			CreateMap<AddressDTO, Addresses>().ReverseMap();
			CreateMap<Users, UserDTO>().ReverseMap();
			CreateMap<RoleDTO, Roles>().ReverseMap();
            CreateMap<CreateCategoryDTO, Categories>().ReverseMap();
            CreateMap<CreateClassDTO, Classes>().ReverseMap();
            CreateMap<CreateCourseDTO, Courses>().ReverseMap();
            CreateMap<CreateTeacherDTO, Teachers>().ReverseMap();
        }
    }
}
