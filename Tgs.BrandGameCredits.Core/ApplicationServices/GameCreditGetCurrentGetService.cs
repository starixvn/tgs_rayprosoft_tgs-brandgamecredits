using Tgs.BrandGameCredits.Handler.Common;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditGetCurrentGetService(
	IBrandRepository brandRepository,
	IBrandGameCreditRepository brandGameCreditRepository,
	IBrandGameCreditComputingUtnsil computingUtnsil,
	IBrandGameCreditRiskUtnsil riskUtnsil) : IGetService<GameCreditGetCurrentQuery, GameCreditResult?>
{
	public async ValueTask<GameCreditResult?> GetAsync(GameCreditGetCurrentQuery query, CancellationToken cancellationToken = default)
	{
		var brandInfo = await brandRepository.GetAsync(
			code: query.BrandCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var info = await brandGameCreditRepository.GetCurrentAsync(
			brandInfo.BrandId,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		if (info is null)
		{
			return null;
		}

		var creditCalculationInfo = computingUtnsil.CreditCalculation(
			initialAmount: info.InitialAmount,
			creditAmount: info.CreditAmount,
			debitAmount: info.DebitAmount,
			profitAmount: info.ProfitAmount);

		var creditRiskType = await riskUtnsil.GetRiskTypeAsync(
			brandId: brandInfo.BrandId,
			creditUsedPer: creditCalculationInfo.CreditUsedPer,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new GameCreditResult(
			ResellerId: brandInfo.ResellerId,
			ResellerCode: brandInfo.ResellerCode,
			LicenseeId: brandInfo.LicenseeId,
			LicenseeCode: brandInfo.LicenseeCode,
			BrandId: brandInfo.BrandId,
			BrandCode: brandInfo.BrandCode,
			Month: info.Month,
			InitialAmount: info.InitialAmount,
			CreditAmount: info.CreditAmount,
			DebitAmount: info.DebitAmount,
			ProfitAmount: info.ProfitAmount,
			CreditLimit: creditCalculationInfo.CreditLimit,
			CreditUsed: creditCalculationInfo.CreditUsed,
			CreditUsedPer: creditCalculationInfo.CreditUsedPer,
			CreditAvailable: creditCalculationInfo.CreditAvailable,
			CreditAvailablePer: creditCalculationInfo.CreditAvailablePer,
			CreditRiskType: creditRiskType,
			CreatedOn: info.CreatedOn,
			UpdatedOn: info.UpdatedOn);
	}
}
