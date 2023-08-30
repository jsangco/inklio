using Microsoft.AspNetCore.Identity;
using Inklio.Api.Domain;
using System.Security.Claims;

namespace Inklio.Api.Application.Commands.Accounts;

public class AccountLogoutCommandHandler : IRequestHandler<AccountLogoutCommand, bool>
{
    private readonly ILogger<AccountLogoutCommandHandler> logger;
    private readonly UserManager<InklioIdentityUser> userManager;
    private readonly SignInManager<InklioIdentityUser> signInManager;

    public AccountLogoutCommandHandler(
        ILogger<AccountLogoutCommandHandler> logger,
        UserManager<InklioIdentityUser> userManager,
        SignInManager<InklioIdentityUser> signInManager)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<bool> Handle(AccountLogoutCommand accountLogout, CancellationToken cancellationToken)
    {
        await this.signInManager.SignOutAsync();
        return true;
    }
}