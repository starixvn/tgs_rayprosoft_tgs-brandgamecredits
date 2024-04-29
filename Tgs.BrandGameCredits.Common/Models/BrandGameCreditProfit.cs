namespace Tgs.BrandGameCredits.Common.Models;

public record BrandGameCreditProfit(
	Guid ResellerId,
	string ResellerCode,
	Guid LicenseeId,
	string LicenseeCode,
	Guid BrandId,
	string BrandCode,
	DateOnly Month,
	Guid GameProviderId,
	string GameProviderCode,
	string GameProviderName,
	int Count,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal Commission,
	decimal Turnover,
	decimal WinLoss,
	decimal ProfitAmount);
