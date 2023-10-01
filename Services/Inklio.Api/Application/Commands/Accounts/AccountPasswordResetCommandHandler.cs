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

public class AccountPasswordResetCommandHandler : IRequestHandler<AccountPasswordResetCommand, bool>
{
    private readonly ILogger<AccountPasswordResetCommandHandler> logger;
    private readonly UserManager<InklioIdentityUser> userManager;

    public AccountPasswordResetCommandHandler(
        ILogger<AccountPasswordResetCommandHandler> logger,
        UserManager<InklioIdentityUser> userManager,
        IEmailSender emailSender,
        WebConfiguration webConfiguration)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<bool> Handle(AccountPasswordResetCommand accountResetPassword, CancellationToken cancellationToken)
    {
        InklioIdentityUser? user = await userManager.FindByEmailAsync(accountResetPassword.Email);
        if (user is null)
        {
            return true;
        }

        if (string.Equals(accountResetPassword.Password, accountResetPassword.ConfirmPassword) == false)
        {
            throw new InvalidOperationException("The passwords do not match and they should have before this command was handled.");
        }

        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(accountResetPassword.Code));
        IdentityResult resetResult = await userManager.ResetPasswordAsync(user, code, accountResetPassword.Password);
        if (resetResult.Succeeded == false)
        {
            var errors = resetResult.Errors.ToDictionary(x => x.Code, x => new string[] { x.Description });
            throw new InklioDomainException(400, "Could not reset password", errors);
        }

        return true;
    }
}