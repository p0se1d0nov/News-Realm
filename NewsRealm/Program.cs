using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NewsRealm.Data;
using System.Diagnostics;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<NewsRealmContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("NewsRealmContext") ?? throw new InvalidOperationException("Connection string 'NewsRealmContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка и запуск таймера для выполнения скрипта
Timer? spiderTimer = null;

// Метод для запуска Python скрипта
void RunSpiderScript(object? state)
{
    try
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "python", // или "python3" в зависимости от системы
            Arguments = "parser/parser/the_news/run_all_spiders.py",
            WorkingDirectory = Directory.GetCurrentDirectory(),
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = false, // Не перенаправляем вывод
            RedirectStandardError = false  // Не перенаправляем ошибки
        };

        using var process = Process.Start(processStartInfo);
        // Не ждем завершения, запускаем и забываем
        process?.Dispose();
    }
    catch
    {
        // Игнорируем ошибки как требовалось
    }
}

// Запускаем таймер только после построения приложения, но до его запуска
spiderTimer = new Timer(
    callback: RunSpiderScript,
    state: null,
    dueTime: TimeSpan.Zero, // Запустить сразу при старте
    period: TimeSpan.FromMinutes(5) // Повторять каждые 5 минут
);

// Освобождаем таймер при завершении приложения
var appLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
appLifetime.ApplicationStopping.Register(() =>
{
    spiderTimer?.Dispose();
});

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
