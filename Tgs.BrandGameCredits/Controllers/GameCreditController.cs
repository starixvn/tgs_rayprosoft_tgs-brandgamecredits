using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core;
using Tgs.ServiceUtility;
using Tgs.BrandGameCredits.Core.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Tgs.BrandGameCredits.Repositories.Entities;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameCreditController : ControllerBase
{
    /// <summary>
    /// Queries the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="brandCode">The brand code.</param>
    /// <returns></returns>
    [HttpGet("{brandCode}/query")]
    public async IAsyncEnumerable<GameCredit> QueryAsync(
        [FromServices] IQueryService<GameCreditQuery, GameCreditResult> service,
        string brandCode)
    {
        var results = await service.QueryAsync(
            query: new GameCreditQuery(
                BrandCode: brandCode),
            cancellationToken: HttpContext.RequestAborted)
             .ToArrayAsync(
            cancellationToken: HttpContext.RequestAborted)
             .ConfigureAwait(false)
             ?? Enumerable.Empty<GameCreditResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameCredit(
                ResellerId: result.ResellerId,
                ResellerCode: result.ResellerCode,
                LicenseeId: result.LicenseeId,
                LicenseeCode: result.LicenseeCode,
                BrandId: result.BrandId,
                BrandCode: result.BrandCode,
                Month: result.Month,
                InitialAmount: result.InitialAmount,
                CreditAmount: result.CreditAmount,
                DebitAmount: result.DebitAmount,
                ProfitAmount: result.ProfitAmount,
                CreditLimit: result.CreditLimit,
                CreditUsed: result.CreditUsed,
                CreditUsedPer: result.CreditUsedPer,
                CreditAvailable: result.CreditAvailable,
                CreditAvailablePer: result.CreditAvailablePer,
                CreatedOn: result.CreatedOn,
                UpdatedOn: result.UpdatedOn);
        }
    }

    /// <summary>
    /// Gets the current asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="brandCode">The brand code.</param>
    /// <returns></returns>
    [HttpGet("{brandCode}/current")]
    public async ValueTask<GameCredit?> GetCurrentAsync(
        [FromServices] IGetService<GameCreditGetCurrentQuery, GameCreditResult?> service,
        string brandCode)
    {
        var result = await service.GetAsync(
            query: new GameCreditGetCurrentQuery(
                BrandCode: brandCode),
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return result is null
            ? null
            : new GameCredit(
                ResellerId: result.ResellerId,
                ResellerCode: result.ResellerCode,
                LicenseeId: result.LicenseeId,
                LicenseeCode: result.LicenseeCode,
                BrandId: result.BrandId,
                BrandCode: result.BrandCode,
                Month: result.Month,
                InitialAmount: result.InitialAmount,
                CreditAmount: result.CreditAmount,
                DebitAmount: result.DebitAmount,
                ProfitAmount: result.ProfitAmount,
                CreditLimit: result.CreditLimit,
                CreditUsed: result.CreditUsed,
                CreditUsedPer: result.CreditUsedPer,
                CreditAvailable: result.CreditAvailable,
                CreditAvailablePer: result.CreditAvailablePer,
                CreatedOn: result.CreatedOn,
                UpdatedOn: result.UpdatedOn);
    }
}
