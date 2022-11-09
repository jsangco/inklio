using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Inklio.Auth.Controllers;

[Authorize]
[ApiController]
[Route("/claims")]
public class ClaimsController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        Console.WriteLine("Authorized user requested claims");
        var claims = this.User.Claims.Select(c => new {name = c.Type, value = c.Value}).ToArray();
        return this.Ok(claims);
    }
}