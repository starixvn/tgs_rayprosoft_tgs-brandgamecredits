using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditTxnQueryCountGetService(
	IBrandRepository brandRepository,
	IBrandGameCreditTxnRepository brandGameCreditTxnRepository) : IGetService<GameCreditTxnQueryCountQuery, int>
{
	public async ValueTask<int> GetAsync(GameCreditTxnQueryCountQuery query, CancellationToken cancellationToken = default)
	{
		var brandInfo = await brandRepository.GetAsync(
			code: query.BrandCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return await brandGameCreditTxnRepository.QueryCountAsync(
			 brandId: brandInfo.BrandId,
			 type: query.Type,
			 startOn: query.StartOn,
			 endOn: query.EndOn,
			 cancellationToken: cancellationToken)
			 .ConfigureAwait(false);
	}
}
