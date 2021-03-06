using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

//Retrieve the Connection String from the secrets manager 
var connectionString = builder.Configuration.GetConnectionString("AppConfig");

// Add services to the container.

builder.Host.ConfigureAppConfiguration(builder =>
{
    //Connect to your App Config Store using the connection string
    // get values with no labels and labels that match HostingEnvironment (Production, Staging, etc)
    builder.AddAzureAppConfiguration(options =>
        options
            .Connect(connectionString)
            .Select(KeyFilter.Any, LabelFilter.Null)
            .Select(KeyFilter.Any, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            );
});
builder.Services.AddControllersWithViews();

var app = builder.Build();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
