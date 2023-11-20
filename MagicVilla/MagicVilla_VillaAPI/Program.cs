/*
 
Add NuGet:
        Microsoft.AspNetCore.Mvc.NewtonsoftJson;
        Microsoft.AspNetCore.JsonPatch;
        Microsoft.EntityFraemworkCore.SqlServer
        Microsoft.EntityFraemworkCore.Tools    
        AutoMapper
        AutoMapper.Extension,Microsoft.DependencyInjection
        Microsoft.AspNetCore.Mvc.Versioning
        Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
*/

using System.Text;
using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container:
                
                //registration service Repository(pattern Repository)(our Services)
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddEndpointsApiExplorer();
                //Service for Caching
builder.Services.AddResponseCaching();
                //Service for AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
                //Service for control Version Api
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.ReportApiVersions = true;
});
                //Service for add VersionApi in Explorer
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
                //Service Setting Autentification
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(u =>
{
    u.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    u.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(u =>
    {
        u.RequireHttpsMetadata = false;
        u.SaveToken = true;
        u.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
                //Service for EntityFraemwork
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    //DefaultSQLConnection write in  appsettings.json
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});
                //Service for Controller
builder.Services.AddControllers(option =>
    {
        option.CacheProfiles.Add("Default30",
            new CacheProfile()
            {
                Duration = 30
            });
    }).
    AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
                //Service for setting Swagger (Autentification(Token))
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
            "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
            "Example: \"Bearer eyJhbGciOiJIU(can find api/UserAuth/login Response\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Magic Villa V1",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "SiteVillas",
            Url = new Uri("https://localhost:7002")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Magic Villa V2",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "SiteVillas",
            Url = new Uri("https://localhost:7002")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json","Magic_VillaV2");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();