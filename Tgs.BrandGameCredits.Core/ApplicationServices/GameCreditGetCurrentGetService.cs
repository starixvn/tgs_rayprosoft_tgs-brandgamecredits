using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditGetCurrentGetService(
    IBrandRepository brandRepository,
    IBrandGameCreditRepository brandGameCreditRepository,
    IComputingUtnsil computingUtnsil) : IGetService<GameCreditGetCurrentQuery, GameCreditResult?>
{
    public async ValueTask<GameCreditResult?> GetAsync(GameCreditGetCurrentQuery query, CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: query.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var info = await brandGameCreditRepository.GetCurrentAsync(
            brandInfo.BrandId,
            cancellationToken: cancellationToken);

        if (info is null)
        {
            return null;
        }

        var creditCalculationInfo = computingUtnsil.CreditCalculation(info.InitialAmount, info.CreditAmount, info.DebitAmount, info.ProfitAmount);

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
            CreatedOn: info.CreatedOn,
            UpdatedOn: info.UpdatedOn);
    }
}
