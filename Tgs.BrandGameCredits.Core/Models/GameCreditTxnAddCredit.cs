using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditTxnAddCredit(
    Guid Id,
    Guid BrandId,
    decimal Money,
    decimal Amount,
    string Remark,
    JsonNode Expansions,
    Guid CreatedUserId,
    string CreatedUsername);
