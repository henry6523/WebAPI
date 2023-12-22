
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.DTO
{
	public class StudentDTO
	{
        public string StudentCard { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public int BirthDate { get; set; }
        public int PhoneNo { get; set; }

    }
    public class StudentSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(StudentDTO))
            {
                schema.Description = "Student Card, StudentId, email and date of birth are all displayed in structured form for editing";

            }
        }
    }
}
