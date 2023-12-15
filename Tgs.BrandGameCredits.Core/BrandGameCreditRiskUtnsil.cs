using Tgs.BrandGameCredits.Common;

namespace Tgs.BrandGameCredits.Core;

internal class BrandGameCreditRiskUtnsil(IBrandGameCreditSuspSettingRepository repository) : IBrandGameCreditRiskUtnsil
{
    public async ValueTask<BrandGameCreditRiskTypes> GetRiskTypeAsync(Guid brandId, decimal creditUsedPer, CancellationToken cancellationToken = default)
    {
        var settingInfo = await repository.GetAsync(
            brandId: brandId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? await repository.GetAsync(
                brandId: Guid.Empty,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NullReferenceException($"This brand does not exist.{brandId}");

        return settingInfo.HighRisk < creditUsedPer
            ? BrandGameCreditRiskTypes.HighRisk
            : settingInfo.ModerateRisk < creditUsedPer
            ? BrandGameCreditRiskTypes.ModerateRisk
            : settingInfo.LowRisk < creditUsedPer
            ? BrandGameCreditRiskTypes.LowRisk
            : BrandGameCreditRiskTypes.NormalRisk;
    }
}
