using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Inklio.Api.Infrastructure.EFCore;

public static class IdentityExtensions
{
    public static IServiceCollection AddInklioIdentity(
        this IServiceCollection services,
        IWebHostEnvironment environment,
        string sqlConnectionString)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "Inklio";
        });

        services.AddDbContext<IdentityContext>(options =>
        {
            // Configure the context to use an in-memory store.
            options.UseSqlServer(sqlConnectionString);

            if (environment.IsDevelopment())
            {
                options.LogTo(Console.WriteLine);
            }
        });

        services.AddDefaultIdentity<InklioIdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<IdentityContext>();

        services.AddIdentityCore<object>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            options.User.RequireUniqueEmail = false;
        });

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            // options.Cookie.HttpOnly = true;
            // options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/login";
            options.AccessDeniedPath = "/login";
            options.SlidingExpiration = true;
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = (context) =>
                {
                    context.HttpContext.Response.Redirect(context.RedirectUri.Replace("http://", "https://"));
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}