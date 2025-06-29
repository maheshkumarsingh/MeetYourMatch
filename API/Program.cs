using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR.MessageHub;
using API.SignalR.PresenceHub;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API;
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddIdentityServices(builder.Configuration);

        var app = builder.Build();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseCors(opt => opt.AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .WithOrigins("http://localhost:4200",
                              "https://localhost:4200"));

        app.UseRouting();  // Uncommenting this line for routing
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseApiVersioning();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.MapControllers();
        app.MapHub<PresenceHub>("hubs/Presence");
        app.MapHub<MessageHub>("hubs/Message");

        app.MapFallbackToController("Index", "Fallback");

        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
            await context.Database.MigrateAsync();
            await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections];");
            await Seed.SeedUsers(userManager, roleManager);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred during migration");
        }
        app.Run();
    }
}
