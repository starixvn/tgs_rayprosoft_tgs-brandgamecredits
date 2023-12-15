using Tgs.BrandGameCredits.Common;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandGameCreditRiskUtnsil
{
    ValueTask<BrandGameCreditRiskTypes> GetRiskTypeAsync(Guid brandId, decimal creditUsedPer, CancellationToken cancellationToken = default);
}
