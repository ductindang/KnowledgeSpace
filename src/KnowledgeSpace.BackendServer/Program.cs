using FluentValidation;
using FluentValidation.AspNetCore;
using KnowledgeSpace.BackendServer.Data;
using KnowledgeSpace.BackendServer.Data.Entities;
using KnowledgeSpace.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region =============== LOGGING ===============
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
#endregion =============== LOGGING ===============

// -------------------------
// 1. Setup Entity Framework
// -------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// -------------------------
// 2. Setup Identity
// -------------------------
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // Sign-in settings
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;

    // Password settings
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;

    // User settings
    options.User.RequireUniqueEmail = true;
});

//builder.Services.AddIdentityServer(options =>
//    {
//        options.Events.RaiseErrorEvents = true;
//        options.Events.RaiseInformationEvents = true;
//        options.Events.RaiseFailureEvents = true;
//        options.Events.RaiseSuccessEvents = true;
//    })
//    .AddInMemoryApiResources()
//    .AddInMemoryClients()
//    .AddInMemoryIdentityResources()
//    .AddAspNetIdentity<User>();





// -------------------------
// Add other services
// -------------------------
// Use Fluent Validator
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RoleVMValidator>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<DbInitializer>(); // Registing seeding class

// -------------------------
// Build app
// -------------------------
var app = builder.Build();

#region ================ SEEDING DATA =================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Log.Information("Seeding data...");
        var dbInitializer = services.GetRequiredService<DbInitializer>();
        await dbInitializer.Seed();
        Log.Information("Seeding completed successfully!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
#endregion ================ SEEDING DATA =================

// -------------------------
// Configure middleware
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// To use scafford
app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// -------------------------
// Run app
// -------------------------
try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
