using Microsoft.EntityFrameworkCore;
using MyWebAPI;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using DataAccessLayer.Data;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using DataAccessLayer.Repositories;
using ModelsLayer.Entity;
using Microsoft.AspNetCore.Identity;
using ModelsLayer.DTO;
using static IdentityModel.ClaimComparer;

namespace MyWebAPI
{

    public class CustomSchemaFilter<T> : ISchemaFilter where T : class
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(T))
                return;

            schema.Description = "To describe the meaning, " +
                                 "function or detailed information about the Address in that object.";
            if (schema.Properties.ContainsKey("zipCode"))
            {
                schema.Properties["zipCode"].Description = "Zip Code is a system used to facilitate the delivery of mail.";
            }
        }
    }

    public class RemoveSchemaFilter<T> : ISchemaFilter where T : class
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type != typeof(T))
                return;
        }
    }
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
            {
                
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Student Management API",
					Description = "**DESCRIPTION**\n" +
						  "\n" +
			                  "This Web API is used to manage student's information. " +
			                  "It consists of endpoints that facilitate tasks: add students, updating their information," +
			                  " and executing administrative functions, add couses, and accessing a comprehensive list" +
			                  " of all students and teachers. The API operates around 3 primary objects: teachers, " +
			                  "students, and courses.\n" +
							  "\nThere are 3 primary access Roles: **Editor, Writer, Reader.**\n" +
			                  "\nAdministrators hold complete access to all endpoints, enabling them to perform CRUD " +
			                  "(Create, Read, Update, Delete) operations across categories like category, class, " +
			                  "course, student, and teacher namespaces. Furthermore, administrators have the authority " +
			                  "to manage user roles by adding or removing roles, providing control over user management." +
			                  ".\n" +
			                  "\n" +
							  "**Roles:**\n" +
			                  "\n" +
							  "**Reader:** Accessible to view all information and retrieve data by its unique identifier.\n" +
			                  "\n" +
							  "**Writer:** Permitted to input new information into the system\n" +
			                  "\n" +
							  "**Editor:** Empowered to modify and remove existing information\n" +
			                  "\n" +
							  "This section represents a mock project developed by **Group 1** from class **HCM_23_CPL_NET_08.** " +
			                  "It stands as a comprehensive collaboration effort, featuring the following team members: \n" +
						                  "\n" +
			                  "- Ly Nguyen (Leader) \n" +
			                  "- Tran Quang Hien \n" +
			                  "- Nguyen Nhat Thanh \n" +
			                  "- Dong Sy Nhat Thanh",
					TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Support - Website",
						Url = new Uri("https://support.smartbear.com/swaggerhub/message/")
					},

				});


                options.SchemaFilter<RemoveSchemaFilter<AddressesDTO>>();
                options.SchemaFilter<CustomSchemaFilter<AddressesDTO>>();
                options.SchemaFilter<CategorySchemaFilter>();
                options.SchemaFilter<ClassSchemaFilter>();
                options.SchemaFilter<CourseSchemaFilter>();
                options.SchemaFilter<StudentSchemaFilter>();
                options.SchemaFilter<TeacherSchemaFilter>();


                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            builder.Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AuthResponsesOperationFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {   
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            var mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile<MappingProfile>(); 
			});
			var mapper = new Mapper(mapperConfiguration);

			builder.Services.AddAutoMapper(Assembly.Load("ModelsLayer"));

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<CustomValidationResultFilter>();
            });


            builder.Services.AddSingleton<IMapper>(mapper);
			builder.Services.AddScoped<ICoursesRepository, CoursesRepository>();
			builder.Services.AddScoped<IClassesRepository, ClassesRepository>();
			builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();
			builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
			builder.Services.AddScoped<IAddressesRepository, AddressesRepository>();
			builder.Services.AddScoped<ITeachersRepository, TeachersRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
            builder.Services.AddScoped<IResponseServiceRepository, ResponseServiceRepository>();

            builder.Services.AddScoped<Seed>();

			builder.Services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("DataAccessLayer"));
			});

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            });


            var app = builder.Build();

            if (args.Length == 1 && args[0].ToLower() == "seeddata")
                SeedData(app);

            void SeedData(IHost app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<Seed>();
                    service.SeedDataContext();
                }
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
			{
				app.UseSwagger();
				app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Management API");
                    options.RoutePrefix = "swagger";
                    options.InjectStylesheet("/css/swagger-ui/custom.css");

                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
	}
}

