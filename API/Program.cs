using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection"));
        });
        builder.Services.AddCors();
        var app = builder.Build();
        
        app.UseCors(opt => opt.AllowAnyHeader()
                              .AllowAnyMethod()
                              .WithOrigins("http://localhost:4200", 
                              "https://localhost:4200"));
        
        app.UseRouting();  // Uncommenting this line for routing

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
