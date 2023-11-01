/*
 
Add NuGet:
        Microsoft.AspNetCore.Mvc.NewtonsoftJson;
        Microsoft.AspNetCore.JsonPatch;
        Microsoft.EntityFraemworkCore.SqlServer
        Microsoft.EntityFraemworkCore.Tools    
        AutoMapper
        AutoMapper.Extension,Microsoft.DependencyInjection
            
        
*/

using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container:

                //registration service Repository(pattern Repository)
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();

                //Service for AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

                //Service for EntityFraemwork
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    //DefaultSQLConnection write in  appsettings.json
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

                //Service for Controller
builder.Services.AddControllers().
    AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();