using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BusinessLogicLayer.DTO
{
	public class CategoryDTO
	{
        public int Id { get; set; }
        public string CategoriesName { get; set; }
	}

    public class CreateCategoryDTO
    {
        public string CategoriesName { get; set; }
    }
    public class CategorySchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(CategoryDTO))
            {
                schema.Description = "Displays the category Id and name of the Category";
                // Các cấu hình khác nếu cần thiết
                
            }
        }
    }
}
