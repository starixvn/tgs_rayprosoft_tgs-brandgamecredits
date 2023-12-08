using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandGameCreditRepository
{
    public ValueTask<GameCreditInfo?> GetCurrentAsync(Guid brandId, CancellationToken cancellationToken = default);

    public IAsyncEnumerable<GameCreditInfo> QueryAsync(Guid brandId, CancellationToken cancellationToken = default);
}
