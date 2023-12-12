using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditSuspSettingInfo(
    Guid BrandId,
    decimal LowRisk,
    string LowNotification,
    decimal ModerateRisk,
    string ModerateNotification,
    decimal HighRisk,
    string HighNotification,
    decimal Credit,
    string CreditNotification,
    string ManualNotification,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);
