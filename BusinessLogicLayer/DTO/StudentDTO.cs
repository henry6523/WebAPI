using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.DTO
{
    public class StudentDTO
    {
        [Required(ErrorMessage = "StudentCard is required")]
        public string StudentCard { get; set; }

        [Required(ErrorMessage = "StudentName is required")]
        public string StudentName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Range(typeof(DateTime), "1/1/1900", "1/1/2023", ErrorMessage = "BirthDate should be between 1/1/1900 and 1/1/2023")]
        public DateTime BirthDate { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "PhoneNo should be a 9-digit number")]
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
