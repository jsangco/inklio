using Inklio.Api.Application.Commands;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OData.ModelBuilder;
using Inklio.Api.Domain;
using System.Security.Claims;

public class AccountCreateCommandHandler : IRequestHandler<AccountCreateCommand, bool>
{
    private readonly ILogger<AccountCreateCommandHandler> logger;
    private readonly UserManager<InklioIdentityUser> userManager;
    private readonly IUserStore<InklioIdentityUser> userStore;
    private readonly SignInManager<InklioIdentityUser> signInManager;
    private readonly IEmailSender emailSender;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly WebConfiguration webConfiguration;
    private readonly IUserEmailStore<InklioIdentityUser> emailStore;

    public AccountCreateCommandHandler(
        ILogger<AccountCreateCommandHandler> logger,
        UserManager<InklioIdentityUser> userManager,
        IUserStore<InklioIdentityUser> userStore,
        SignInManager<InklioIdentityUser> signInManager,
        IEmailSender emailSender,
        WebConfiguration webConfiguration)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this.webConfiguration = webConfiguration;
        this.emailStore = GetEmailStore();
    }

    public async Task<bool> Handle(AccountCreateCommand accountCreate, CancellationToken cancellationToken)
    {
        var user = new InklioIdentityUser(accountCreate.Username);
        await this.userStore.SetUserNameAsync(user, accountCreate.Username, cancellationToken);
        await this.emailStore.SetEmailAsync(user, accountCreate.Email, cancellationToken);
        var userCreateResult = await this.userManager.CreateAsync(user, accountCreate.Password);

        if (userCreateResult.Succeeded == false)
        {
            var errors = userCreateResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            this.logger.LogInformation("Could not create account {Errors}", errors);
            throw new InklioDomainException(400, "Unable to create new account.");
        }

        var roleResult = await this.userManager.AddToRoleAsync(user, UserRoles.User);
        if (roleResult.Succeeded == false)
        {
            var errors = userCreateResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            this.logger.LogError("Failed to add role to account. {Role} | {Error}", UserRoles.User, errors);
            throw new InvalidOperationException($"Failed to add {UserRoles.User} role to account. {errors}");
        }

        this.logger.LogInformation($"Created new user: {user.UserName}");

        // Send Email conformation to new user.
        string userId = await userManager.GetUserIdAsync(user);
        string code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        string codeBase64 = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        Uri baseUrl = new Uri(this.webConfiguration.BaseUrl);
        Uri callbackUri = new Uri(baseUrl, "/accounts/confirm?userId={userId}&code={codeBase64}");
        await this.emailSender.SendEmailAsync(
            accountCreate.Email,
            "Inklio - Email Confirmation",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUri.ToString())}'>clicking here</a>.");

        return true;
    }

    private IUserEmailStore<InklioIdentityUser> GetEmailStore()
    {
        if (!this.userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<InklioIdentityUser>)this.userStore;
    }
}