using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BusinessLogicLayer.DTO
{
	public class ClassDTO
	{
		public int Id { get; set; }

		public string ClassName { get; set; }
	}
    public class ClassSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(ClassDTO))
            {
                schema.Description = "The class id and class names are displayed by this DTO";
            }
        }
    }
}
