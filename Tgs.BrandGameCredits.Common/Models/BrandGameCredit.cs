namespace Tgs.BrandGameCredits.Common.Models;

public record BrandGameCredit(
	Guid ResellerId,
	string ResellerCode,
	Guid LicenseeId,
	string LicenseeCode,
	Guid BrandId,
	string BrandCode,
	DateOnly Month,
	decimal InitialAmount,
	decimal CreditAmount,
	decimal DebitAmount,
	decimal ProfitAmount,
	decimal CreditLimit,
	decimal CreditUsed,
	decimal CreditUsedPer,
	decimal CreditAvailable,
	decimal CreditAvailablePer,
	BrandGameCreditRiskTypes CreditRiskType,
	DateTimeOffset CreatedOn,
	DateTimeOffset UpdatedOn);
