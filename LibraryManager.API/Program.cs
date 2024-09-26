using LibraryManagement.API;
using LibraryManager.Application;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// configure DI for service factory
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
