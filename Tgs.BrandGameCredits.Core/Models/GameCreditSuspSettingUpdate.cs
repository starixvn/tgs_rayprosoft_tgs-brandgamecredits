using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditSuspSettingUpdate(
    Guid BrandId,
    decimal LowRisk,
    BrandGameCreditSuspSettingNotificationMsg LowNotification,
    decimal ModerateRisk,
    BrandGameCreditSuspSettingNotificationMsg ModerateNotification,
    decimal HighRisk,
    BrandGameCreditSuspSettingNotificationMsg HighNotification,
    BrandGameCreditSuspSettingNotificationMsg ManualNotification);
