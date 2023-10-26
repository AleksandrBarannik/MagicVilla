
using MagicVilla_VillaAPI.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*
Add NuGet:
        Microsoft.AspNetCore.Mvc.NewtonsoftJson;
        Microsoft.AspNetCore.JsonPatch;
        
*/

/*
 //Для использования SereLog вместо стандартного
 //Установить пакеты SereLog.AspNet;Serelog.Sinks.File;
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/villaLogs.txt",rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();
*/

builder.Services.AddControllers(option =>
{
    //Будете выдавать ошибку если тип формата не Json /Xml;
    //option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Ilogging, Logging>();

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