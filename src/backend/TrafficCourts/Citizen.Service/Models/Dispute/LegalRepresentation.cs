using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrafficCourts.Citizen.Service.Models.Dispute;

/// <summary>
/// Represents the legal council the disputant is intended to be represented by.
/// </summary>
public class LegalRepresentation
{
    /// <summary>
    /// The name of the law firm.
    /// </summary>
    [JsonPropertyName("law_firm_name")]
    [MaxLength(255)]
    public string? LawFirmName { get; set; }

    /// <summary>
    /// The full name of the lawyer/
    /// </summary>
    [JsonPropertyName("lawyer_full_name")]
    [MaxLength(255)]
    public string? LawyerFullName { get; set; }

    /// <summary>
    /// The email address of the lawyer.
    /// </summary>
    [JsonPropertyName("email_address")]
    [SwaggerSchema(Format = "email")]
    [MaxLength(255)]
    public string? LawyerEmail { get; set; }

    /// <summary>
    /// The full mailing address of the lawyer, including street address, city, province and postal code.
    /// </summary>
    [JsonPropertyName("full_address")]
    [MaxLength(255)]
    public string? Address { get; set; }

}
