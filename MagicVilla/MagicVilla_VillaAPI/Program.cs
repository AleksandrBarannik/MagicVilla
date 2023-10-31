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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container:

                //Service for AutoMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

                //Service for EntityFraemwork
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    //DefaultSQLConnection write in  appsettings.json
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

                //Service for Controller
builder.Services.AddControllers(option =>
{
    
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


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