using Microsoft.AspNetCore.Mvc;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Controllers;

[Route("[controller]")]
[ApiController]
public class GameCreditProfitController : ControllerBase
{
    /// <summary>
    /// Queries the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="brandCode">The brand code.</param>
    /// <param name="month">The month.</param>
    /// <returns></returns>
    [HttpGet("{brandCode}/query")]
    public async IAsyncEnumerable<Common.Models.BrandGameCreditProfit> QueryAsync(
        [FromServices] IQueryService<GameCreditProfitQuery, GameCreditProfitResult> service,
        string brandCode,
        DateOnly month)
    {
        var results = await service.QueryAsync(
            query: new GameCreditProfitQuery(
                BrandCode: brandCode,
                Month: month),
            cancellationToken: HttpContext.RequestAborted)
             .ToArrayAsync(
            cancellationToken: HttpContext.RequestAborted)
             .ConfigureAwait(false)
             ?? Enumerable.Empty<GameCreditProfitResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new Common.Models.BrandGameCreditProfit(
                ResellerId: result.ResellerId,
                ResellerCode: result.ResellerCode,
                LicenseeId: result.LicenseeId,
                LicenseeCode: result.LicenseeCode,
                BrandId: result.BrandId,
                BrandCode: result.BrandCode,
                Month: result.Month,
                GameProviderId: result.GameProviderId,
                GameProviderCode: result.GameProviderCode,
                GameProviderName: result.GameProviderName,
                Count: result.Count,
                Bet: result.Bet,
                ValidBet: result.ValidBet,
                Settle: result.Settle,
                Commission: result.Commission,
                Turnover: result.Turnover,
                WinLoss: result.WinLoss,
                ProfitAmount: result.ProfitAmount);
        }
    }
}
