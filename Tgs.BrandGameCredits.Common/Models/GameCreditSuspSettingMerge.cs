using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Common.Models;

public record GameCreditSuspSettingMerge
{
    [Required]
    public string BrandCode { get; init; } = default!;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal LowRisk { get; init; }
    [Required]
    public JsonNode LowNotification { get; init; } = default!;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal ModerateRisk { get; init; }
    public JsonNode ModerateNotification { get; init; } = default!;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal HighRisk { get; init; }
    [Required]
    public JsonNode HighNotification { get; init; } = default!;
    [Required]
    public JsonNode ManualNotification { get; init; } = default!;
}