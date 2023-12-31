﻿// CourseDTO.cs
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.DTO
{
    public class CoursesDTO
    {
        public int Id { get; set; }
        public string CourseName { get; set; }

    }
    public class CreateCourseDTO
    {
        [Required(ErrorMessage = "Course name is required.")]
        [StringLength(50, ErrorMessage = "Course name cannot exceed 50 characters.")]
        public string CourseName { get; set; }
    }
    public class CourseSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CoursesDTO))
            {
                schema.Description = "Shows the Course Id structure and Course names";

            }
        }
    }
}
