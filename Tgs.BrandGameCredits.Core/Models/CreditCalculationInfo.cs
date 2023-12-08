namespace Tgs.BrandGameCredits.Core.Models;

public record CreditCalculationInfo(
    decimal CreditLimit,
    decimal CreditUsed,
    decimal CreditUsedPer,
    decimal CreditAvailable,
    decimal CreditAvailablePer);
