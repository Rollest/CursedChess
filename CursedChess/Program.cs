using CursedChess.Components;
using Microsoft.EntityFrameworkCore;

namespace CursedChess;

/// <summary>
/// Точка входа приложения.
/// </summary>
public class Program
{
    /// <summary>
    /// Запускает веб-приложение и инициализирует зависимости.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents(options =>
            {
                options.DetailedErrors = builder.Environment.IsDevelopment();
            });

        // Domain/Application services
        builder.Services.AddScoped<Application.Contracts.ISolutionSearchService, Application.Services.SolutionSearchService>();
        builder.Services.AddScoped<Application.Contracts.IBoardRepository, Infrastructure.Repositories.BoardRepository>();
        builder.Services.AddScoped<Application.Contracts.ISolutionRepository, Infrastructure.Repositories.SolutionRepository>();

        // EF Core + SQLite
        builder.Services.AddDbContext<Infrastructure.BoardDbContext>(options =>
            options.UseSqlite("Data Source=board.db"));

        var app = builder.Build();

        // Ensure database is created
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<Infrastructure.BoardDbContext>();
            db.Database.EnsureCreated();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}
