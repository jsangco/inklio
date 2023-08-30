using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Inklio.Api.Application.Commands.Accounts;

/// <summary>
/// Command used to logout
/// </summary>
public class AccountLogoutCommand : IRequest<bool>
{
}