using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrafficCourts.Citizen.Service.Models.Dispute;

/// <summary>
/// Represents a violation ticket notice of dispute.
/// </summary>
public class NoticeOfDispute
{
    #region Violation ticket identification
    /// <summary>
    /// The violation ticket number.
    /// </summary>
    [JsonPropertyName("ticket_number")]
    [MaxLength(12)]
    public string? TicketNumber { get; set; }

    /// <summary>
    /// The provincial court hearing location named on the violation ticket.
    /// </summary>
    [JsonPropertyName("provincial_court_hearing_location")]
    [MaxLength(50)]
    public string? ProvincialCourtHearingLocation { get; set; }

    /// <summary>
    /// The date and time the violation ticket was issue. Time must only be hours and minutes.
    /// </summary>
    [JsonPropertyName("issued_date")]
    [SwaggerSchema(Format = "date")]
    public DateTime? IssuedDate { get; set; }
    #endregion

    #region Disputant
    /// <summary>
    /// The surname or corporate name.
    /// </summary>
    [JsonPropertyName("surname")]
    [MaxLength(30)]
    public string? Surname { get; set; }

    /// <summary>
    /// The given names or corporate name continued.
    /// </summary>
    [JsonPropertyName("given_names")]
    [MaxLength(30)]
    public string? GivenNames { get; set; }

    /// <summary>
    /// The drivers licence number. Note not all jurisdictions will use numeric drivers licence numbers.
    /// </summary>
    [JsonPropertyName("drivers_licence_number")]
    [MaxLength(20)]
    public string? DriversLicenceNumber { get; set; }

    /// <summary>
    /// The province or state the drivers licence was issued by.
    /// </summary>
    [JsonPropertyName("drivers_licence_province")]
    [MaxLength(30)]
    public string? DriversLicenceProvince { get; set; }

    /// <summary>
    /// The mailing address of the disputant. 
    /// </summary>
    [JsonPropertyName("address")]
    [MaxLength(30)]
    public string? Address { get; set; }

    /// <summary>
    /// The mailing address city of the disputant.
    /// </summary>
    [JsonPropertyName("city")]
    [MaxLength(30)]
    public string? City { get; set; }

    /// <summary>
    /// The mailing address province of the disputant.
    /// </summary>
    [JsonPropertyName("province")]
    [MaxLength(30)]
    public string? Province { get; set; }

    /// <summary>
    /// The mailing address postal code or zip code of the disputant.
    /// </summary>
    [JsonPropertyName("postal_code")]
    [MaxLength(6)]
    public string? PostalCode { get; set; }

    /// <summary>
    /// The disputant's home phone number.
    /// </summary>
    [JsonPropertyName("home_phone_number")]
    [MaxLength(20)]
    public string? HomePhoneNumber { get; set; }

    /// <summary>
    /// The disputant's work phone number.
    /// </summary>
    [JsonPropertyName("work_phone_number")]
    [MaxLength(20)]
    public string? WorkPhoneNumber { get; set; }

    /// <summary>
    /// The disputant's email address.
    /// </summary>
    /// <remarks>
    /// This field does not exist on the paper notice of dispute.
    /// </remarks>
    [JsonPropertyName("email_address")]
    [SwaggerSchema(Format = "email")]
    [MaxLength(255)]
    public string? Email { get; set; }
    #endregion

    #region Dispute Details (Part B)

    /// <summary>
    /// The organization or detatchment location. For example, Delta Police.
    /// </summary>
    [JsonPropertyName("organization_location")]
    [MaxLength(100)]
    public string? OrganizationLocation { get; set; }

    /// <summary>
    /// The count dispute details.
    /// </summary>
    [JsonPropertyName("counts")]
    [Range(1, 3)]
    [Required]
    public List<NoticeOfDisputeCount>? Counts { get; set; }

    #endregion

    #region Intentions and Requirements
    /// <summary>
    /// The disputant intends to be represented by a lawyer at the hearing. If not specified, defaults to false.
    /// </summary>
    [JsonPropertyName("represented_by_lawyer")]
    public bool? RepresentedByLawyer { get; set; }

    /// <summary>
    /// The legal council the disputant is intended to be represented by. Required if represented_by_lawyer is true.
    /// </summary>
    public LegalRepresentation? LegalRepresentation { get; set; }

    /// <summary>
    /// The disputant requires spoken language interpreter. The language name is indicated in this field.
    /// If not specified, no interpreter is required or will be provided.
    /// </summary>
    [JsonPropertyName("interpreter_language")]
    [MaxLength(50)]
    public string? InterpreterLanguage { get; set; }

    /// <summary>
    /// The number of witnesses the disputant intends to call. 
    /// </summary>
    [JsonPropertyName("call_witness_count")]
    [Range(0, 99)]
    public short NumberOfWitnesses { get; set; }

    #endregion
}
