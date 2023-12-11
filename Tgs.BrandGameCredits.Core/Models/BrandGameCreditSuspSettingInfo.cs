using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.Models;

public record BrandGameCreditSuspSettingInfo(
    Guid BrandId,
    decimal LowRisk,
    JsonNode LowNotification,
    decimal ModerateRisk,
    JsonNode ModerateNotification,
    decimal HighRisk,
    JsonNode HighNotification,
    decimal Credit,
    JsonNode CreditNotification,
    JsonNode ManualNotification,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);
