using Inklio.Api.Application.Commands;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.OData.ModelBuilder;
using Inklio.Api.Domain;
using System.Security.Claims;

namespace Inklio.Api.Application.Commands.Accounts;

public class AccountForgetPasswordCommandHandler : IRequestHandler<AccountForgetPasswordCommand, bool>
{
    private readonly ILogger<AccountForgetPasswordCommandHandler> logger;
    private readonly UserManager<InklioIdentityUser> userManager;
    private readonly IEmailSender emailSender;
    private readonly WebConfiguration webConfiguration;

    public AccountForgetPasswordCommandHandler(
        ILogger<AccountForgetPasswordCommandHandler> logger,
        UserManager<InklioIdentityUser> userManager,
        IUserStore<InklioIdentityUser> userStore,
        IEmailSender emailSender,
        WebConfiguration webConfiguration)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        this.webConfiguration = webConfiguration;
    }

    public async Task<bool> Handle(AccountForgetPasswordCommand accountForgetPassword, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(accountForgetPassword.Email);
        if (user == null)
        {
            this.logger.LogInformation("User tried to reset invalid email. {Email}", accountForgetPassword.Email);
            return false;
        }

        var code = await this.userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        Uri baseUrl = new Uri(this.webConfiguration.BaseUrl);
        var callbackUri = new Uri(baseUrl, $"/accounts/reset?token={code}");

        await this.emailSender.SendEmailAsync(
            accountForgetPassword.Email,
            "Inklio - Reset Password",
            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUri.ToString())}'>clicking here</a>.");

        this.logger.LogInformation("Sent password reset email. {Email}", accountForgetPassword.Email);
        return true;
    }
}