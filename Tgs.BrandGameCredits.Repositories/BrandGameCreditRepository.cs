using Microsoft.EntityFrameworkCore;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Entities;
using Tgs.BrandGameCredits.Handler.Common;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandGameCreditRepository(
	TgsContext context,
	TimeProvider timeProvider,
	IBrandGameCreditTimeUtnsil timeUtnsil) : IBrandGameCreditRepository
{
	public async ValueTask<GameCreditInfo?> GetCurrentAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var monthTimeZone = timeUtnsil.Get1stOfEveryMonthTimeZone(timeProvider.GetUtcNow(), 480);
		var monthDateOnly = timeUtnsil.Get1stOfEveryMonthDateOnly(monthTimeZone, 480);

		return await context.BrandGameCredits.AsQueryable()
			.Where(predicate: predicate => predicate.Month == monthDateOnly
			&& predicate.BrandId == brandId)
			.Select(selector => new GameCreditInfo(
				selector.ResellerId,
				selector.LicenseeId,
				selector.BrandId,
				selector.Month,
				selector.InitialAmount,
				selector.CreditAmount,
				selector.DebitAmount,
				selector.ProfitAmount,
				selector.CreatedOn,
				selector.UpdatedOn))
			.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public IAsyncEnumerable<GameCreditInfo> HistoriesAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var monthTimeZone = timeUtnsil.Get1stOfEveryMonthTimeZone(timeProvider.GetUtcNow(), 480);
		var monthDateOnly = timeUtnsil.Get1stOfEveryMonthDateOnly(monthTimeZone, 480);

		return context.BrandGameCredits.AsQueryable()
			.Where(predicate: predicate => predicate.Month >= monthDateOnly.AddMonths(-13)
			&& predicate.Month < monthDateOnly
			&& predicate.BrandId == brandId)
			.Take(12)
			.OrderByDescending(keySelector: keySelector => keySelector.Month)
			.Select(selector => new GameCreditInfo(
				selector.ResellerId,
				selector.LicenseeId,
				selector.BrandId,
				selector.Month,
				selector.InitialAmount,
				selector.CreditAmount,
				selector.DebitAmount,
				selector.ProfitAmount,
				selector.CreatedOn,
				selector.UpdatedOn))
			.ToAsyncEnumerable();
	}
}
