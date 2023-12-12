using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Tgs.BrandGameCredits.Common.Models;

public record GameCreditSuspSetting(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode,
    decimal LowRisk,
    JsonNode LowNotification,
    decimal ModerateRisk,
    JsonNode ModerateNotification,
    decimal HighRisk,
    JsonNode HighNotification,
    JsonNode ManualNotification,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);
