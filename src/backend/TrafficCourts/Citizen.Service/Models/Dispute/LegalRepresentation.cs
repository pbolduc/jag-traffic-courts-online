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

    [JsonPropertyName("lawyer_email_address")]
    [SwaggerSchema(Format = "email")]
    [MaxLength(255)]
    public string? LawyerEmail { get; set; }

    /// <summary>
    /// The mailing address of the lawyer. 
    /// </summary>
    [JsonPropertyName("address")]
    [MaxLength(30)]
    public string? Address { get; set; }

    /// <summary>
    /// The mailing address city of the lawyer.
    /// </summary>
    [JsonPropertyName("city")]
    [MaxLength(30)]
    public string? City { get; set; }

    /// <summary>
    /// The mailing address province of the lawyer.
    /// </summary>
    [JsonPropertyName("province")]
    [MaxLength(30)]
    public string? Province { get; set; }

    /// <summary>
    /// The mailing address postal code or zip code of the lawyer.
    /// </summary>
    [JsonPropertyName("postal_code")]
    [MaxLength(6)]
    public string? PostalCode { get; set; }
}
