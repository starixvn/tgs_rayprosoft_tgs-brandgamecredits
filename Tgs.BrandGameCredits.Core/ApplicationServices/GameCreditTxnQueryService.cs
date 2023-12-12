using System.Runtime.CompilerServices;
using Tgs.BrandGameCredits.Core.Expansions;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditTxnQueryService(
    IBrandRepository brandRepository,
    IBrandGameCreditTxnRepository brandGameCreditTxnRepository) : IQueryService<GameCreditTxnQuery, GameCreditTxnResult>
{
    public async IAsyncEnumerable<GameCreditTxnResult> QueryAsync(GameCreditTxnQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: query.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var infos = await brandGameCreditTxnRepository.QueryAsync(
            brandId: brandInfo.BrandId,
            type: query.Type,
            startOn: query.StartOn,
            endOn: query.EndOn,
            offset: query.PageNumber - 1,
            limit: query.PageSize,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameCreditTxnInfo>();

        if (!infos.Any())
        {
            yield break;
        }

        foreach (var info in infos)
        {
            yield return new GameCreditTxnResult(
                Id: info.Id,
                ResellerId: brandInfo.ResellerId,
                ResellerCode: brandInfo.ResellerCode,
                LicenseeId: brandInfo.LicenseeId,
                LicenseeCode: brandInfo.LicenseeCode,
                BrandId: brandInfo.BrandId,
                BrandCode: brandInfo.BrandCode,
                Type: info.Type,
                Money: info.Money,
                Amount: info.Amount,
                Remark: info.Remark,
                Expansions: info.Expansions.ToJsonNode(),
                CreatedUserId: info.CreatedUserId,
                CreatedUsername: info.CreatedUsername,
                CreatedOn: info.CreatedOn);
        }
    }
}
