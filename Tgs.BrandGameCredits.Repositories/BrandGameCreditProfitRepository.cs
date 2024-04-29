using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Entities;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandGameCreditProfitRepository(TgsContext context) : IBrandGameCreditProfitRepository
{
	public IAsyncEnumerable<BrandGameCreditProfitInfo> QueryAsync(Guid brandId, DateOnly month, CancellationToken cancellationToken = default)
		=> context.BrandGameCreditProfits
		.Where(predicate: predicate => predicate.BrandId == brandId
		&& predicate.Month == month)
		.Join(
			inner: context.GameProviders,
			outerKeySelector: brandGameCreditProfits => brandGameCreditProfits.GameProviderId,
			innerKeySelector: gameProvider => gameProvider.Id,
			resultSelector: (brandGameCreditProfits, gameProvider) => new BrandGameCreditProfitInfo(
				brandGameCreditProfits.BrandId,
				brandGameCreditProfits.Month,
				brandGameCreditProfits.GameProviderId,
				gameProvider.Code,
				gameProvider.Name,
				brandGameCreditProfits.Count,
				brandGameCreditProfits.Bet,
				brandGameCreditProfits.ValidBet,
				brandGameCreditProfits.Settle,
				brandGameCreditProfits.Commission,
				brandGameCreditProfits.Turnover,
				brandGameCreditProfits.WinLoss,
				brandGameCreditProfits.ProfitAmount))
		.ToAsyncEnumerable();
}
