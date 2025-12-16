using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsRealm.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NewsRealmContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NewsRealmContext") ?? throw new InvalidOperationException("Connection string 'NewsRealmContext' not found.")));

// Add services to the container.
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
    name: "frontend",
    pattern: "{controller=Frontend}/{action=Index}/{id?}");

app.MapControllerRoute(name: "backend", pattern: "{controller=NewsModels}/{action?}/{id?}");


//app.MapControllers();
app.Run();
