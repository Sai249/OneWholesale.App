using OneWholesale.Repository.DbFactory;

using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using OneWholesale.Repository.Repositories.Repository;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);


// ✅ Add Logging
builder.Logging.ClearProviders();  // Removes default logging providers
builder.Logging.AddConsole();      // Logs to console
builder.Logging.AddDebug();        // Logs debug output (for Visual Studio)
builder.Logging.AddEventLog();     // Optional: Logs to Windows Event Viewer (if applicable)

// ✅ Add services
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);



builder.Services.AddScoped<IDbOWConnection, DbOWConnection>();

builder.Services.AddScoped<IBrandRepository, BrandRepository>();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});



// ✅ Add Antiforgery for AJAX Requests
builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// ✅ Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ✅ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

// ✅ Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Brand}/{action=BrandList}/{id?}");

    endpoints.MapControllerRoute(
        name: "deleteBrand",
        pattern: "Brand/DeleteBrand/{id}",
        defaults: new { controller = "Brand", action = "DeleteBrand" });
});


app.Run();
