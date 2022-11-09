using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Inklio.Auth.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Inklio.Auth.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly ILogger<AccountsController> logger;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IUserStore<IdentityUser> userStore;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IEmailSender emailSender;
    private readonly IUserEmailStore<IdentityUser> emailStore;

    public AccountsController(
        ILogger<AccountsController> logger,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        SignInManager<IdentityUser> signInManager,
        IEmailSender emailSender)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.userStore = userStore ?? throw new ArgumentNullException(nameof(userStore));
        this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));

        this.emailStore = GetEmailStore();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]AccountLogin accountLogin, CancellationToken cancellationToken)
    {
        var signInResult = await this.signInManager.PasswordSignInAsync(
            accountLogin.Username,
            accountLogin.Password,
            accountLogin.IsRememberMe,
            lockoutOnFailure: false);

        if (signInResult.Succeeded)
        {
            this.logger.LogInformation($"User loggend in as: {accountLogin.Username}");
            
            // Create a new claims principal
            var claims = new List<Claim>
            {
                // new Claim(OpenIddictConstants.Claims.Subject, result.Principal.Identity.Name),
                new Claim(ClaimTypes.Name, accountLogin.Username)
            };

            var claimsIdentity = new ClaimsIdentity(claims, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return this.SignIn(new ClaimsPrincipal(claimsIdentity));
            // return this.Ok(new { signedIn = true });
        }
        else if (signInResult.IsLockedOut)
        {
            this.logger.LogInformation($"User account has been locked: {accountLogin.Username}");
            return this.Unauthorized();
        }
        else
        {
            return this.Unauthorized($"Invalid login attempt for user: {accountLogin.Username}");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await this.signInManager.SignOutAsync();
        return this.NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]AccountCreate accountCreate, CancellationToken cancellationToken)
    {
        var user = new IdentityUser(accountCreate.Username);
        await this.userStore.SetUserNameAsync(user, accountCreate.Username, cancellationToken);
        await this.emailStore.SetEmailAsync(user, accountCreate.Email, cancellationToken);
        var userCreateResult = await this.userManager.CreateAsync(user, accountCreate.Password);

        if (userCreateResult.Succeeded == false)
        {
            var errors = userCreateResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            return this.BadRequest(errors);
        }

        this.logger.LogInformation($"Created new user: {user.UserName}");

        var userId = await userManager.GetUserIdAsync(user);
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        Uri callbackUri = new Uri($"https://{this.Request.Host}/accounts/confirm?userId={userId}&code={code}");
        await this.emailSender.SendEmailAsync(
            accountCreate.Email,
            "Inklio - Email Confirmation",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUri.ToString())}'>clicking here</a>.");

        await this.signInManager.SignInAsync(user, isPersistent: true);

        return this.NoContent();
    }

    [HttpPost("forget")]
    public async Task<IActionResult> ForgetPassword(AccountForgetPassword accountForgetPassword)
    {
        var user = await userManager.FindByEmailAsync(accountForgetPassword.Email);
        if (user == null)
        {
            return this.Accepted();
        }

        var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUri = new Uri($"https://{this.Request.Host}/account/reset?token={code}");

        await this.emailSender.SendEmailAsync(
            accountForgetPassword.Email,
            "Inklio - Reset Password",
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUri.ToString())}'>clicking here</a>.");

        return this.Accepted();
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword(AccountResetPassword accountResetPassword)
    {
        IdentityUser user = await userManager.FindByEmailAsync(accountResetPassword.Email);
        if (user == null)
        {
            return this.Accepted(); // Do not reveal user existence
        }

        IdentityResult resetResult = await userManager.ResetPasswordAsync(user, accountResetPassword.Code, accountResetPassword.Password);
        if (resetResult.Succeeded == false)
        {
            var errors = resetResult.Errors.Select(e => $"{e.Code}: {e.Description}");
            return this.BadRequest(errors);
        }

        return this.Accepted();
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!this.userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)this.userStore;
    }
    }
