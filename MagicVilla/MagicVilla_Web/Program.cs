using MagicVilla_Web;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;

/*
 
Add NuGet:
        AutoMapper
        AutoMapper.Extension,Microsoft.DependencyInjection
        JSON CONVERT
        In appsettings.json прописал URL адрес VillaAPI:
        "ServiceUrls": {"VillaAPI": "https://localhost:7001"}
            
        
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

                //AutiMapper
builder.Services.AddAutoMapper(typeof(MappingConfig));
                //VillaServices
builder.Services.AddHttpClient<IVillaService, VillaService>();
builder.Services.AddScoped<IVillaService, VillaService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();