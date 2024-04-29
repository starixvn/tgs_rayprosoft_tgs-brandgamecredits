using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditSuspSettingMergeCommand(
	string BrandCode,
	decimal LowRisk,
	BrandGameCreditSuspSettingNotificationMsg LowNotification,
	decimal ModerateRisk,
	BrandGameCreditSuspSettingNotificationMsg ModerateNotification,
	decimal HighRisk,
	BrandGameCreditSuspSettingNotificationMsg HighNotification,
	BrandGameCreditSuspSettingNotificationMsg ManualNotification);
