using FluentValidation;
using FluentValidation.AspNetCore;
using Maqha.Core.IRepository;
using Maqha.Extensions;
using Maqha.MiddleWares;
using Maqha.Repository.Data;
using Maqha.Repository.Repository;
using Maqha.Utilities.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Maqha
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Add to Context and Identity
            builder.Services.AddService(builder.Configuration);
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            //Generc Repository
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Add Application Services
            builder.Services.AddApplicationServices();
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new Maqha.Utilities.Helpers.DateTimeConverter());
            });
            //Add Fluent Validation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Maqha API",
                    Version = "v1",
                    Description = "Maqha API with JWT Authentication"
                });

                // ✅ Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and your token.\nExample: Bearer eyJhbGciOiJIUzI1..."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.SetIsOriginAllowed(origin => true) // Allow any origin
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // Add this if you're sending credentials
                });
            });
            builder.Services.AddHttpContextAccessor();
            //Authentication and Authorization
            #region Authentication and Authorization
            var JWTSettings = builder.Configuration.GetSection("JWT");
            var validIssuer = JWTSettings["ValidIssuer"] ;
            var validAudience = JWTSettings["ValidAudience"] ;
            var secretKey = JWTSettings["Key"];


            builder.Services.AddAuthentication(c => {
                c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = validIssuer,
                        ValidAudience = validAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                       RoleClaimType = ClaimTypes.Role
                    };
                });
            //builder.Services.AddAuthorization(options=> 
            //{
            //    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
            //    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("user", "admin"));
            //});
            #endregion
            // builder.Services.AddHttpContextAccessor();
            Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Configuration)
           .Enrich.FromLogContext()
           .CreateLogger();
            builder.Host.UseSerilog();
            var app = builder.Build();
            //Data Seeding
            #region Data Seeding
            using var Scop=app.Services.CreateScope();
            var Services = Scop.ServiceProvider;
            try
            {
                //DbContext Seeding
                var DbContext = Services.GetRequiredService<MaqhaDbContext>();
                await DbContext.Database.MigrateAsync(); // Ensure the database is created and migrations are applied
                await MaqhaContextSeed.SeedAsync(DbContext); // Call the Seed method to populate initial data
                //await DbContext.SeedAsync(); // Call the Seed method to populate initial data
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during data seeding.");
            }
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Maqha API V1");
                    options.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
                });
                app.MapOpenApi(pattern: "api/document.json");
                app.MapScalarApiReference("/api-docs", options =>
                {
                    options.WithOpenApiRoutePattern("api/document.json")
                           .WithTitle("Maqha API v1")
                           .WithLayout(ScalarLayout.Modern)
                           .WithDarkMode(true)
                           .WithSearchHotKey("Ctrl+K");// Custom search hotkey
                           
                });
            }
            //test entpoint 
            app.MapGet("/", () => "Maqha API is running!");
            app.MapGet("/health", () => new { status = "healthy", time = DateTime.UtcNow });
            //use Middleware for Serilog
            app.UseMiddleware<SerilogMiddleWare>();
            //app.UseIpRateLimiting();
            ///     app.UseMiddleware<SerliogMid>
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // Enable serving static files (e.g., images, CSS, JS)
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseAuthentication(); // Must be before UseAuthorization
            app.Use(async (context, next) =>
            {
                var user = context.User;
                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    Console.WriteLine("User is authenticated: " + user.Identity.Name);
                    foreach (var claim in user.Claims)
                    {
                        Console.WriteLine($"{claim.Type} = {claim.Value}");
                    }
                }
                else
                {
                    Console.WriteLine("User is NOT authenticated");
                }
                await next();
            });
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
