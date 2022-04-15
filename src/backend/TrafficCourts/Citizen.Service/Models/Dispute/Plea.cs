using System.Text.Json.Serialization;

namespace TrafficCourts.Citizen.Service.Models.Dispute;

/// <summary>
/// Represents the dispuant plea on count.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Plea
{
    /// <summary>
    /// The dispuant is pleading not guilty on a specific count.
    /// </summary>
    NotGuilty,
    /// <summary>
    /// The dispuant is pleading guilty on a specific count.
    /// </summary>
    Guilty
}
