namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditTxnQueryCountQuery(
     string BrandCode,
     DateTimeOffset StartOn,
     DateTimeOffset EndOn,
     BrandGameCreditTxnTypes? Type);
