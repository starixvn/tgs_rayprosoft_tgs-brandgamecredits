using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditSuspSettingMergeCommand(
    string BrandCode,
    decimal LowRisk,
    JsonNode LowNotification,
    decimal ModerateRisk,
    JsonNode ModerateNotification,
    decimal HighRisk,
    JsonNode HighNotification,
    JsonNode ManualNotification);
