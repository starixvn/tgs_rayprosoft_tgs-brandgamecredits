using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditSuspSettingAdd(
	Guid BrandId,
	decimal LowRisk,
	BrandGameCreditSuspSettingNotificationMsg LowNotification,
	decimal ModerateRisk,
	BrandGameCreditSuspSettingNotificationMsg ModerateNotification,
	decimal HighRisk,
	BrandGameCreditSuspSettingNotificationMsg HighNotification,
	BrandGameCreditSuspSettingNotificationMsg ManualNotification);
