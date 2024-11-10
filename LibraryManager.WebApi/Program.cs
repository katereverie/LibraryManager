using LibraryManagement.API;
using LibraryManager.Application;
using System.Reflection;
using System.Text.Json.Serialization;

namespace LibraryManager.WebAPI;

/// <summary>
/// Entry point for the Web API application. Configures services, logging, middleware, and runs the application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main method that configures and runs the Web API application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        // Configure DI for service factory
        var config = new AppConfiguration();
        var serviceFactory = new ServiceFactory(config);

        builder.Services.AddScoped(_ => serviceFactory.CreateBorrowerService());
        builder.Services.AddScoped(_ => serviceFactory.CreateMediaService());
        builder.Services.AddScoped(_ => serviceFactory.CreateCheckoutService());

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
