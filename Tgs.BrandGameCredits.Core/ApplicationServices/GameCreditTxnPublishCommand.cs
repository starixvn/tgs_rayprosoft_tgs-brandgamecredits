namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditTxnPublishCommand(
    string Subject,
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode);
