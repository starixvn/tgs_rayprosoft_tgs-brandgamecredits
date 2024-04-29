using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandGameCreditProfitRepository
{
	public IAsyncEnumerable<BrandGameCreditProfitInfo> QueryAsync(Guid brandId, DateOnly month, CancellationToken cancellationToken = default);
}
