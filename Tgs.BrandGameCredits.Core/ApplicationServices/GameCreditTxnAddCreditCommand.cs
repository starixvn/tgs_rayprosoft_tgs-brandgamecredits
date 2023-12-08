using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditTxnAddCreditCommand(
    string BrandCode,
    decimal Money,
    decimal Amount,
    string Remark,
    JsonNode Expansions,
    Guid CreatedUserId,
    string CreatedUsername);
