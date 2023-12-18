namespace Tgs.BrandGameCredits.Core.Models;

public record BrandGameCreditProfitInfo(
     Guid BrandId,
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
