using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PRN232.LMS.API.Middleware;
using PRN232.LMS.Repositories.Data;
using PRN232.LMS.Repositories.Repositories;
using PRN232.LMS.Services.Mappings;
using PRN232.LMS.Services.Services;
using PRN232.LMS.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

// ─── Database ────────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LmsDbContext>(options =>
    options.UseSqlServer(connectionString));

// ─── Dependency Injection — Repositories ─────────────────────────────────────
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ─── Dependency Injection — Services ─────────────────────────────────────────
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// ─── AutoMapper ───────────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(typeof(MappingProfile));

// ─── FluentValidation (runs alongside DataAnnotations, not replacing them) ───
builder.Services.AddFluentValidationAutoValidation(config =>
{
    // Run FluentValidation in addition to DataAnnotations
    config.DisableDataAnnotationsValidation = false;
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<PRN232.LMS.API.Validators.StudentRequestValidator>();

// ─── JWT Authentication ───────────────────────────────────────────────────────
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "PRN232.LMS.API";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "PRN232.LMS.Client";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ─── API Versioning ───────────────────────────────────────────────────────────
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// ─── Controllers + Content Negotiation ───────────────────────────────────────
builder.Services.AddControllers(options =>
{
    // Return 406 for unsupported Accept headers
    options.ReturnHttpNotAcceptable = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
})
.AddXmlSerializerFormatters()          // application/xml support
.AddXmlDataContractSerializerFormatters(); // application/xml (DataContract) support

// ─── Swagger / OpenAPI ────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // v1 document
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PRN232 LMS API",
        Version = "v1",
        Description = "Learning Management System REST API — Lab 2 (Advanced Security & Features)",
        Contact = new OpenApiContact
        {
            Name = "LMS Team",
            Email = "lms@fpt.edu.vn"
        }
    });

    // v2 document
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "PRN232 LMS API",
        Version = "v2",
        Description = "Learning Management System REST API v2 — Enhanced responses with computed fields",
        Contact = new OpenApiContact
        {
            Name = "LMS Team",
            Email = "lms@fpt.edu.vn"
        }
    });

    // JWT Security Definition — adds the Authorize button in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token below.\n\nExample: eyJhbGciOiJIUzI1NiIsInR..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

    // Include XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// ─── Run Migrations ───────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LmsDbContext>();
    dbContext.Database.Migrate();
}

// ─── Middleware Pipeline ──────────────────────────────────────────────────────
// 1. Global Exception Handler (must be first)
app.UseMiddleware<GlobalExceptionMiddleware>();

// 2. Request Logging
app.UseMiddleware<RequestLoggingMiddleware>();

// 3. Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(d => d.ApiVersion))
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            $"PRN232 LMS API {description.GroupName.ToUpperInvariant()}");
    }
    options.RoutePrefix = "swagger";
    options.DisplayRequestDuration();
    options.EnableFilter();
});

// 4. Routing
app.UseRouting();

// 5. Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// 6. Map Controllers
app.MapControllers();

app.Run();
