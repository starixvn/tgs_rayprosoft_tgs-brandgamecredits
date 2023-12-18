using System.Runtime.CompilerServices;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditProfitQueryService(
    IBrandRepository brandRepository,
    IBrandGameCreditProfitRepository brandGameCreditProfitRepository) : IQueryService<GameCreditProfitQuery, GameCreditProfitResult>
{
    public async IAsyncEnumerable<GameCreditProfitResult> QueryAsync(GameCreditProfitQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: query.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var infos = await brandGameCreditProfitRepository.QueryAsync(
            brandInfo.BrandId,
            query.Month,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BrandGameCreditProfitInfo>();

        if (!infos.Any())
        {
            yield break;
        }

        foreach (var info in infos)
        {
            yield return new GameCreditProfitResult(
                ResellerId: brandInfo.ResellerId,
                ResellerCode: brandInfo.ResellerCode,
                LicenseeId: brandInfo.LicenseeId,
                LicenseeCode: brandInfo.LicenseeCode,
                BrandId: info.BrandId,
                BrandCode: brandInfo.BrandCode,
                Month: info.Month,
                GameProviderId: info.GameProviderId,
                GameProviderCode: info.GameProviderCode,
                GameProviderName: info.GameProviderName,
                Count: info.Count,
                Bet: info.Bet,
                ValidBet: info.ValidBet,
                Settle: info.Settle,
                Commission: info.Commission,
                Turnover: info.Turnover,
                WinLoss: info.WinLoss,
                ProfitAmount: info.ProfitAmount);
        }
    }
}
