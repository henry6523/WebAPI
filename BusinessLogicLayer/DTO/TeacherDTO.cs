using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.DTO
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
        [Required(ErrorMessage = "Teacher name is required.")]
        [StringLength(50, ErrorMessage = "Teacher name cannot exceed 50 characters.")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Invalid phone number. It must be a 9-digit number.")]
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
