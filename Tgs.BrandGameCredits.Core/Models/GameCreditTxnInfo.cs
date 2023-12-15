using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common;

namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditTxnInfo(
    Guid Id,
    Guid BrandId,
    BrandGameCreditTxnTypes Type,
    decimal Money,
    decimal Amount,
    string Remark,
    string Expansions,
    Guid CreatedUserId,
    string CreatedUsername,
    DateTimeOffset CreatedOn);
