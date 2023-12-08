using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.Models;

public record BrandGameCreditTxnInfo(
    Guid Id,
    Guid BrandId,
    BrandGameCreditTxnTypes Type,
    decimal Money,
    decimal Amount,
    string Remark,
    JsonNode Expansions,
    Guid CreatedUserId,
    string CreatedUsername,
    DateTimeOffset CreatedOn);
