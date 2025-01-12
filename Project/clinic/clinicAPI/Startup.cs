using clinicAPI.Data;
using clinicAPI.Data.Interfaces;
using clinicAPI.Data.Model;
using clinicAPI.Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace clinicAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        //.WithOrigins("https://localhost:4200") // Allow specific origin
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
              
            });

            // Configure JWT authentication
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine(context.Exception);
                       // throw new Exception(context.Exception.ToString());
                        return Task.CompletedTask;
                    }
                };

            });
            services.AddAuthorization();

            // Add Swagger for API documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                    // Add the security definition for Bearer authentication
                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
                    });
                    // Apply the security scheme globally to all endpoints
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
                        Array.Empty<string>()
                     }
                        });
                });

            services.Configure<PasswordHasherOptions>(options =>
            {
                options.IterationCount = 100_000; // Set iteration count for PBKDF2
            });
            // Register UserRepository service
            // Add connection string from appsettings.json
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            services.AddScoped<IServiceRepository>(provider => new ServiceRepository(connectionString));
            services.AddScoped<IAppointmentRepository>(provider => new AppointmentRepository(connectionString));
            services.AddScoped<UserRepository>();
            services.AddScoped<JwtTokenHelper>();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAngularApp"); // Use the policy here

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            

            // Map controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }
    }
}
