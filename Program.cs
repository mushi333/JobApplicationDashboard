using JobApplicationDashboard.Data;
using JobApplicationDashboard.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
    .AddDbContext<JobApplicationDashboardContext>(
        options =>
            options.UseSqlite(
                builder.Configuration.GetConnectionString("JobApplicationDashboardContext")
                    ?? throw new InvalidOperationException(
                        "Connection string 'JobApplicationDashboardContext' not found."
                    )
            )
    );

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

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

app.MapControllerRoute(name: "default", pattern: "{controller=Applications}/{action=Index}/{id?}");

app.Run();
