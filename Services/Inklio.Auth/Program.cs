using Inklio.Auth;
using Inklio.Auth.HealthCheck;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

Console.WriteLine("Starting Auth server");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("Database");
builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.Name = "Inklio";
    });
// builder.Services.AddInklioIdentity();
builder.Services.AddDbContext<IdentityDataContext>(options =>
    {
        // Configure the context to use an in-memory store.
        var connectionString = builder.Configuration.GetConnectionString("IdentityDataContextConnection");
        options.UseSqlServer(connectionString);
        // .UseInMemoryDatabase(nameof(DbContext));

    });

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<IdentityDataContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsDevelopment() == false)
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/", new HealthCheckOptions
{
    ResponseWriter = HealthCheckWriter.WriteResponse,
    AllowCachingResponses = false,
    Predicate = healthCheck => healthCheck.Name == "Database",
});

app.Run();
