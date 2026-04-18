using AppCore.Interfaces;
using AppCore.Module;
using AppCore.Services;
using Infrastructure;
using Infrastructure.Memory;
using Infrastructure.Security;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddContactsModule();
        builder.Services.AddControllers();
        builder.Services.AddContactsEfModule(builder.Configuration);
        builder.Services.AddSingleton<JwtSettings>();
        builder.Services.AddJwt(new JwtSettings(builder.Configuration));

        builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();
        builder.Services.AddProblemDetails();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
       

        var app = builder.Build();

        app.UseExceptionHandler();
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            using var scope = app.Services.CreateScope(); // zasięg dostepu do kontenera DI
            using (scope)
            {
                // "wyciągniecie" z kontenera instacji klasy implementującej IDataSeeder
                var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
                await seeder.SeedAsync();    
            }

        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        

        app.MapControllers();

        app.Run();
    }
}