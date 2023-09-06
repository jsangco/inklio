using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Inklio.Api.Application.Commands;

/// <summary>
/// Attribute to validate a Username complies with an expected format.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class UsernameAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var username = value as string;
        if (string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        var usernameRegx = new Regex("^(?=[a-zA-Z0-9_]{3,20}$)(?!.*[_.]{2})[^_.].*[^_.]$");
        if (usernameRegx.IsMatch(username))
        {
            return true;
        }

        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return "The Username must be within 3-20 characters and can only contain characters A-Z, 0-9, and underscores.";
    }
}