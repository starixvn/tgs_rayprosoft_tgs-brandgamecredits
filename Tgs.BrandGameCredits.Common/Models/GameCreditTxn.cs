using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Common.Models;

public record GameCreditTxn(
    Guid Id,
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode,
    GameCreditTxnTypes Type,
    decimal Money,
    decimal Amount,
    string Remark,
    JsonNode Expansions,
    Guid CreatedUserId,
    string CreatedUsername,
    DateTimeOffset CreatedOn);
