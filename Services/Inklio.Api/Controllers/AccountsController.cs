using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Inklio.Api.Application.Commands;
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
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IMediator mediator;

    public AccountsController(
        ILogger<AccountsController> logger,
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IMediator mediator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]AccountLoginCommand accountLogin, CancellationToken cancellationToken)
    {
        var claimsPrincipal = await this.mediator.Send(accountLogin, cancellationToken);
        return this.NoContent();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await this.signInManager.SignOutAsync();
        return this.NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]AccountCreateCommand accountCreate, CancellationToken cancellationToken)
    {
        await this.mediator.Send(accountCreate, cancellationToken);

        var accountLogin = new AccountLoginCommand()
        {
            IsRememberMe = true,
            Password = accountCreate.Password,
            Username = accountCreate.Username
        };
        await this.mediator.Send(accountLogin, cancellationToken);

        return this.NoContent();
    }

    // [HttpPost("forget")]
    // public async Task<IActionResult> ForgetPassword(AccountForgetPassword accountForgetPassword)
    // {
    //     var user = await userManager.FindByEmailAsync(accountForgetPassword.Email);
    //     if (user == null)
    //     {
    //         return this.Accepted();
    //     }

    //     var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
    //     code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    //     var callbackUri = new Uri($"https://{this.Request.Host}/account/reset?token={code}");

    //     await this.emailSender.SendEmailAsync(
    //         accountForgetPassword.Email,
    //         "Inklio - Reset Password",
    //         $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUri.ToString())}'>clicking here</a>.");

    //     return this.Accepted();
    // }

    // [HttpPost("reset")]
    // public async Task<IActionResult> ResetPassword(AccountResetPassword accountResetPassword)
    // {
    //     IdentityUser user = await userManager.FindByEmailAsync(accountResetPassword.Email);
    //     if (user == null)
    //     {
    //         return this.Accepted(); // Do not reveal user existence
    //     }

    //     IdentityResult resetResult = await userManager.ResetPasswordAsync(user, accountResetPassword.Code, accountResetPassword.Password);
    //     if (resetResult.Succeeded == false)
    //     {
    //         var errors = resetResult.Errors.Select(e => $"{e.Code}: {e.Description}");
    //         return this.BadRequest(errors);
    //     }

    //     return this.Accepted();
    // }
}
