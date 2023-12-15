using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Tgs.BrandGameCredits.Common.Models;

public record BrandGameCreditSuspSetting(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode,
    decimal LowRisk,
    BrandGameCreditSuspSettingNotificationMsg LowNotification,
    decimal ModerateRisk,
    BrandGameCreditSuspSettingNotificationMsg ModerateNotification,
    decimal HighRisk,
    BrandGameCreditSuspSettingNotificationMsg HighNotification,
    BrandGameCreditSuspSettingNotificationMsg ManualNotification,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);
