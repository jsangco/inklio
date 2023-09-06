using Microsoft.AspNetCore.Identity;
using Inklio.Api.Domain;
using System.Security.Claims;

namespace Inklio.Api.Application.Commands.Accounts;

public class AccountLoginCommandHandler : IRequestHandler<AccountLoginCommand, Account>
{
    private readonly ILogger<AccountLoginCommandHandler> logger;
    private readonly UserManager<InklioIdentityUser> userManager;
    private readonly SignInManager<InklioIdentityUser> signInManager;

    public AccountLoginCommandHandler(
        ILogger<AccountLoginCommandHandler> logger,
        UserManager<InklioIdentityUser> userManager,
        SignInManager<InklioIdentityUser> signInManager)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<Account> Handle(AccountLoginCommand accountLogin, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByNameAsync(accountLogin.Username);
        if (user is null)
        {
            string msg = "Invalid username or password.";
            throw new InklioDomainException(401, msg, ("credentials", msg));
        }

        bool isCorrectPassword = await this.userManager.CheckPasswordAsync(user, accountLogin.Password);
        if (isCorrectPassword == false)
        {
            string msg = "Invalid username or password.";
            throw new InklioDomainException(401, msg, ("credentials", msg));
        }

        this.logger.LogInformation($"User loggend in as: {accountLogin.Username}");

        // Create user claims
        IEnumerable<string> userRoles = await this.userManager.GetRolesAsync(user);
        IEnumerable<Claim> roles = userRoles.Select(r => new Claim(ClaimTypes.Role, r));

        IEnumerable<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        }.Concat(roles);

        await this.signInManager.SignInWithClaimsAsync(user, accountLogin.IsRememberMe, claims);

        var account = new Account()
        {
            Id = user.Id.ToString(),
            Username = user.UserName,
            Roles = userRoles,
            IsEmailVerified = user.EmailConfirmed,
        };

        return account;
    }
}