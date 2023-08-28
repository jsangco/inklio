using Microsoft.AspNetCore.OData.Routing.Controllers;

[Route("v1/test")]
public class TestController : ODataController
{
    [HttpPost]
    public IActionResult Post([FromBody] dynamic body)
    {
        Console.WriteLine("POST TEST");
        Console.WriteLine(body);
        return this.NoContent();
    }
}