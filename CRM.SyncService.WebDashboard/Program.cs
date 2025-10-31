using CRM.SyncService.WebDashboard.Data;
using CRM.SyncService.WebDashboard.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ClickUpService>();
//builder.Services.AddScoped<GoogleSheetService>();
//builder.Services.AddScoped<TelegramService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString().ToLower();
    var userName = context.Session.GetString("UserName");

    if (string.IsNullOrEmpty(userName) &&
        !path.StartsWith("/account/login") &&
        !path.StartsWith("/account/register"))
    {
        context.Response.Redirect("/Account/Login");
        return;
    }

    await next();
});
// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllers();

app.Run();
