using Inklio.Api.Application.Commands;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OData.ModelBuilder;
using Inklio.Api.Domain;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class AccountLoginCommandHandler : IRequestHandler<AccountLoginCommand, bool>
{
    private readonly ILogger<AccountLoginCommandHandler> logger;
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;

    public AccountLoginCommandHandler(
        ILogger<AccountLoginCommandHandler> logger,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<bool> Handle(AccountLoginCommand accountLogin, CancellationToken cancellationToken)
    {
        var user = await this.userManager.FindByNameAsync(accountLogin.Username);
        if (user is null)
        {
            throw new InklioDomainException(401, "Invalid login attempt.");
        }

        bool isCorrectPassword = await this.userManager.CheckPasswordAsync(user, accountLogin.Password);
        if (isCorrectPassword == false)
        {
            throw new InklioDomainException(401, "Invalid login attempt.");
        }

        this.logger.LogInformation($"User loggend in as: {accountLogin.Username}");

        // Create user claims
        IEnumerable<Claim> roles = (await this.userManager.GetRolesAsync(user))
            .Select(r => new Claim(ClaimTypes.Role, r));

        IEnumerable<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        }.Concat(roles);

        await this.signInManager.SignInWithClaimsAsync(user, accountLogin.IsRememberMe, claims);

        return true;
    }
}