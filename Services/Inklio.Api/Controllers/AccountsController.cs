using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Inklio.Api.Application.Commands.Accounts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace Inklio.Api.Controllers;

[ApiController]
[Route("v1/accounts")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> logger;
    private readonly SignInManager<InklioIdentityUser> signInManager;
    private readonly IMediator mediator;

    public AccountsController(
        ILogger<AccountsController> logger,
        SignInManager<InklioIdentityUser> signInManager,
        UserManager<InklioIdentityUser> userManager,
        IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    [HttpGet("admins")]
    [Authorize(Roles = "Administrator")]
    public IActionResult Admins()
    {
        return this.Ok("Super Secret Admin Page");
    }

    [HttpGet("claims")]
    public IActionResult Claims()
    {
        string claims = this.User == null || this.User.Claims.Any() == false ? "No Claims" :
            string.Join(
                "\n",
                this.User.Claims.Select(c => $"{c.Type}, {c.Value}").ToArray());
        return this.Ok(claims);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountLoginCommand accountLogin, CancellationToken cancellationToken)
    {
        Account account = await this.mediator.Send(accountLogin, cancellationToken);
        return this.Ok(account);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await this.mediator.Send(new AccountLogoutCommand(), cancellationToken);
        return this.NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AccountCreateCommand accountCreate, CancellationToken cancellationToken)
    {
        await this.mediator.Send(accountCreate, cancellationToken);

        var accountLogin = new AccountLoginCommand()
        {
            IsRememberMe = true,
            Password = accountCreate.Password,
            Username = accountCreate.Username
        };
        Account account = await this.mediator.Send(accountLogin, cancellationToken);

        return this.Ok(account);
    }

    [HttpPost("forget")]
    public async Task<IActionResult> ForgetPassword([FromBody]AccountPasswordForgetCommand accountForgetPassword, CancellationToken cancellationToken)
    {
        await this.mediator.Send(accountForgetPassword, cancellationToken);
        return this.Accepted();
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword(AccountPasswordResetCommand accountResetPassword, CancellationToken cancellationToken)
    {
        await this.mediator.Send(accountResetPassword, cancellationToken);
        return this.Accepted();
    }
}
