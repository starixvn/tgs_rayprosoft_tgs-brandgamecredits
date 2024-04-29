namespace Tgs.BrandGameCredits.Core.Models;

public record GameCreditInfo(
	 Guid ResellerId,
	 Guid LicenseeId,
	 Guid BrandId,
	 DateOnly Month,
	 decimal InitialAmount,
	 decimal CreditAmount,
	 decimal DebitAmount,
	 decimal ProfitAmount,
	 DateTimeOffset CreatedOn,
	 DateTimeOffset UpdatedOn);
