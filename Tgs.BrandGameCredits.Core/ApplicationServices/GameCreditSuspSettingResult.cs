using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditSuspSettingResult(
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
