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
			?? await repository.InitAsync(
				brandId: brandId,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return settingInfo.HighRisk < creditUsedPer
			? BrandGameCreditRiskTypes.HighRisk
			: settingInfo.ModerateRisk < creditUsedPer
			? BrandGameCreditRiskTypes.ModerateRisk
			: settingInfo.LowRisk < creditUsedPer
			? BrandGameCreditRiskTypes.LowRisk
			: BrandGameCreditRiskTypes.NormalRisk;
	}
}
