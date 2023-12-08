using System.Runtime.CompilerServices;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditQueryService(
    IBrandRepository brandRepository,
    IBrandGameCreditRepository brandGameCreditRepository,
    IComputingUtnsil computingUtnsil) : IQueryService<GameCreditQuery, GameCreditResult>
{
    public async IAsyncEnumerable<GameCreditResult> QueryAsync(GameCreditQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: query.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var infos = await brandGameCreditRepository.QueryAsync(
            brandInfo.BrandId,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameCreditInfo>();

        if (!infos.Any())
        {
            yield break;
        }

        foreach (var info in infos)
        {
            var creditCalculationInfo = computingUtnsil.CreditCalculation(info.InitialAmount, info.CreditAmount, info.DebitAmount, info.ProfitAmount);
            yield return new GameCreditResult(
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
}
