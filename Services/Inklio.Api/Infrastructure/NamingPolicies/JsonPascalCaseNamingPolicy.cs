namespace System.Text.Json;

/// <summary>
/// Converts a JSON property name into pascal case.
/// </summary>
public class JsonPascalCaseNamingPolicy : JsonNamingPolicy
{
    private static JsonNamingPolicy camelCasePolicy = JsonNamingPolicy.CamelCase;

    public override string ConvertName(string name)
    {
        if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
        {
            return camelCasePolicy.ConvertName(name);
        }

        string camelCaseName = camelCasePolicy.ConvertName(name);
        StringBuilder stringBuilder = new StringBuilder(camelCaseName);
        stringBuilder[0] = Char.ToUpperInvariant(stringBuilder[0]);

        return stringBuilder.ToString();
    }
}