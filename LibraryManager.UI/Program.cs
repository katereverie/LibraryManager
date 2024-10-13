using LibraryManager.UI;

await RunAsync();

async Task RunAsync()
{
    var app = new App();
    await app.Run();
}
