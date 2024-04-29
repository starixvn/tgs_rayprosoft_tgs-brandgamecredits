using Microsoft.AspNetCore.Mvc;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Controllers;

[Route("[controller]")]
[ApiController]
public class GameCreditController : ControllerBase
{
	/// <summary>
	/// Queries the asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="brandCode">The brand code.</param>
	/// <returns></returns>
	[HttpGet("{brandCode}/histories")]
	public async IAsyncEnumerable<Common.Models.BrandGameCredit> HistoryQueryAsync(
		[FromServices] IQueryService<GameCreditHistoryQuery, GameCreditResult> service,
		string brandCode)
	{
		var results = await service.QueryAsync(
			query: new GameCreditHistoryQuery(
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
			yield return new Common.Models.BrandGameCredit(
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
				CreditRiskType: (BrandGameCreditRiskTypes)result.CreditRiskType,
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
	public async ValueTask<Common.Models.BrandGameCredit?> GetCurrentAsync(
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
			: new Common.Models.BrandGameCredit(
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
				CreditRiskType: (BrandGameCreditRiskTypes)result.CreditRiskType,
				CreatedOn: result.CreatedOn,
				UpdatedOn: result.UpdatedOn);
	}
}
