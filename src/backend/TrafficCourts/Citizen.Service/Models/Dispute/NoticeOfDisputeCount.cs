using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TrafficCourts.Citizen.Service.Models.Dispute;

/// <summary>
/// Represents the disputant's decision on a count on the violation ticket.
/// </summary>
public class NoticeOfDisputeCount
{
    /// <summary>
    /// The count number. Must be between 1 and 3.
    /// </summary>
    [JsonPropertyName("count")]
    [Range(1, 3)]
    [Required]
    public short Count { get; set; }

    /// <summary>
    /// Does the disputant plea (Guilty or NotGuilty).
    /// </summary>
    [JsonPropertyName("plea")]
    [Required]
    [EnumDataType(typeof(Plea))]
    public Plea Plea { get; set; }

    #region Guilty Properties
    /// <summary>
    /// Does the want to appear in court?
    /// </summary>
    [JsonPropertyName("appear_in_court")]
    public bool? AppearInCourt { get; set; }

    /// <summary>
    /// The disputant is requesting a reduction of the ticketed amount.
    /// </summary>
    [JsonPropertyName("request_reduction")]
    public bool? RequestReduction { get; set; }

    /// <summary>
    /// The disputant is requesting time to pay the ticketed amount.
    /// </summary>
    [JsonPropertyName("request_time_to_pay")]
    public bool? RequestTimeToPay { get; set; }

    /// <summary>
    /// The disputant does not want to appear in court and has supplied a violation ticket statement and written reasons
    /// to request a reduction of the ticketed amount(s) and/or time to pay the ticketed amount(s).
    /// </summary>
    /// <remarks>
    /// If not null, either <see cref="RequestReduction"/> or <see cref="RequestTimeToPay"/> should be true.
    /// If not null, Plea should be Guilty.
    /// </remarks>
    [JsonPropertyName("written_reasons")]
    [MaxLength(1024)]
    public string? WrittenReasons { get; set; }

    #endregion
}
