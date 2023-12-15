using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Common.Models;

public record BrandGameCreditSuspSettingMerge
{
    [Required]
    public string BrandCode { get; init; } = default!;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal LowRisk { get; init; }
    [Required]
    public BrandGameCreditSuspSettingNotificationMsg LowNotification { get; init; } = default!;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal ModerateRisk { get; init; }
    public BrandGameCreditSuspSettingNotificationMsg ModerateNotification { get; init; } = default!;
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal HighRisk { get; init; }
    [Required]
    public BrandGameCreditSuspSettingNotificationMsg HighNotification { get; init; } = default!;

    public BrandGameCreditSuspSettingNotificationMsg ManualNotification { get; init; } = default!;
}
