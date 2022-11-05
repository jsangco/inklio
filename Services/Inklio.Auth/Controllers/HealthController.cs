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
[Route("/")]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return this.Ok(new { hello = "world!" });
    }
}