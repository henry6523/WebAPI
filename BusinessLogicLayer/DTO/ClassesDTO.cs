﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;

namespace ModelsLayer.DTO
{
    public class ClassesDTO
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
    }

    public class CreateClassDTO
    {
        [Required(ErrorMessage = "CLass name is required.")]
        [StringLength(50, ErrorMessage = "Class name cannot exceed 50 characters.")]
        public string ClassName { get; set; }
    }
    public class ClassSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ClassesDTO))
            {
                schema.Description = "The class id and class names are displayed by this DTO";
            }
        }
    }
}
