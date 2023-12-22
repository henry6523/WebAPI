// CourseDTO.cs
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTO
{
	public class CourseDTO
	{
		public int Id { get; set; }
		public string CourseName { get; set; }

	}
    public class CreateCourseDTO
    {
        public string CourseName { get; set; }
    }
    public class CourseSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CourseDTO))
            {
                schema.Description = "Shows the Course Id structure and Course names";
                
            }
        }
    }
}
