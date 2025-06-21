using API.Extensions;
using API.Middleware;


namespace API;
public class Program
{
    public static void Main(string[] args)
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
                              .WithOrigins("http://localhost:4200",
                              "https://localhost:4200"));

        app.UseRouting();  // Uncommenting this line for routing
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseApiVersioning();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}
