using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandGameCreditSuspSettingRepository
{
    public ValueTask<BrandGameCreditSuspSettingInfo?> GetAsync(Guid brandId, CancellationToken cancellationToken = default);

    public ValueTask AddAsync(BrandGameCreditSuspSettingAdd source, CancellationToken cancellationToken = default);

    public ValueTask UpdateAsync(BrandGameCreditSuspSettingUpdate source, CancellationToken cancellationToken = default);
}
