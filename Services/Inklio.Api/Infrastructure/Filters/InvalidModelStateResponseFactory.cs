public static class InvalidModelStateResponseFactory
{
    /// <summary>
    /// Converts the errors in the ModelState (i.e. the POST body) to a standardized error object.
    /// </summary>
    public static Func<ActionContext, IActionResult> Instance = context =>
    {
        string wasInvalid = " was invalid.";
        var details = context.ModelState.Select(m => $"{m.Value?.Errors.FirstOrDefault()?.ErrorMessage ?? m.Key + wasInvalid}");
        string detail = string.Join(" | ", details);
        var problemDetails = new ValidationProblemDetails()
        {
            Instance = context.HttpContext.Request.Path,
            Status = 400,
            Detail = detail,
        };

        var errors = context.ModelState.Select(i =>
            new KeyValuePair<string, string[]>(
                i.Key,
                i.Value?.Errors?.Select(e => e.ErrorMessage)?.ToArray() ?? new string[] { $"{i.Key} was invalid." }));
        foreach (var e in errors)
        {
            problemDetails.Errors.Add(e);
        }

        return new BadRequestObjectResult(problemDetails);
    };
}