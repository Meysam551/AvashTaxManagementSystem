
using System.Reflection.Metadata;
using ATMS.ApplicationService;
using ATMS.Infrastructure;
using ATMS.UI.Components;
using MediatR;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddInfrastructureServices(
    configuration: builder.Configuration
);

builder.Services.AddApplicationServices();

// ✅ MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));

// ✅ AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

builder.Services.AddMudServices();

#region config serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logs/app-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] " +
            "{Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));
#endregion

builder.Services
    .AddAuthentication("DevCookie")
    .AddCookie("DevCookie");

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ATMS.UI.Client._Imports).Assembly);

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Unhandled exception: {ex}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        throw;
    }
});

app.Run();

