using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTO
{
	public class TeacherDTO
	{
		public int Id { get; set; }
		public string TeacherName { get; set; }
		public string Email { get; set; }
		public int PhoneNo { get; set; }
	}
    public class CreateTeacherDTO
    {
        public string TeacherName { get; set; }
        public string Email { get; set; }
        public int PhoneNo { get; set; }
    }
    public class TeacherSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(TeacherDTO))
            {
                schema.Description = "Returns the values ​​of Teacher";

            }
        }
    }
}
