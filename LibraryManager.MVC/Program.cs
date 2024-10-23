using LibraryManager.Application;
using LibraryManager.MVC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = new AppConfiguration();
var serviceFactory = new ServiceFactory(config);

builder.Services.AddScoped(provider => serviceFactory.CreateBorrowerService());
builder.Services.AddScoped(provider => serviceFactory.CreateMediaService());
builder.Services.AddScoped(provider => serviceFactory.CreateCheckoutService());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
