using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditSuspSettingAdd(
    Guid BrandId,
    decimal LowRisk,
    JsonNode LowNotification,
    decimal ModerateRisk,
    JsonNode ModerateNotification,
    decimal HighRisk,
    JsonNode HighNotification,
    JsonNode ManualNotification);
