using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// The structure of an OData response returned from the Inklio API
/// </summary>
public class ODataResponse<T>
{
    /// <summary>
    /// Gets or sets the OData Context.
    /// </summary>
    [DataMember(Name = "@odata.context")]
    [JsonPropertyName("@odata.context")]
    public string ODataContext { get; set; } = "";

    /// <summary>
    /// Gets or sets the contents of the API result.
    /// </summary>
    [DataMember(Name = "value")]
    [JsonPropertyName("value")]
    public IEnumerable<T> Value { get; set; } = new T[] { };
}