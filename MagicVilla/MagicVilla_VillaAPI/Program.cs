/*
 
 //Установить через терминал dotnet tool install --global dotnet-ef
Add NuGet:
        Microsoft.AspNetCore.Mvc.NewtonsoftJson;
        Microsoft.AspNetCore.JsonPatch;
        Microsoft.EntityFraemworkCore.SqlServer
        Microsoft.EntityFraemworkCore.Tools        
        
*/

using MagicVilla_VillaAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    //DefaultSQLConnection прописываем  appsettings.json
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddControllers(option =>
{
    //option.ReturnHttpNotAcceptable = true;
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