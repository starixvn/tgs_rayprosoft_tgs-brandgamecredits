using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Repositories.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandGameCreditTxnRepository(TgsContext context) : IBrandGameCreditTxnRepository
{
    public async ValueTask AddAsync(GameCreditTxnAddCredit source, CancellationToken cancellationToken = default)
    {
        var entity = new BrandGameCreditTxn
        {
            Id = source.Id,
            BrandId = source.BrandId,
            Type = (int)BrandGameCreditTxnTypes.Credit,
            Money = source.Money,
            Amount = source.Amount,
            Remark = source.Remark,
            Expansions = source.Expansions.ToJsonString(),
            CreatedUserId = source.CreatedUserId,
            CreatedUsername = source.CreatedUsername,
            CreatedOn = DateTime.UtcNow
        };

        _ = await context.BrandGameCreditTxns.AddAsync(
            entity: entity,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = await context.SaveChangesAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask AddAsync(GameCreditTxnAddDebit source, CancellationToken cancellationToken = default)
    {
        var entity = new BrandGameCreditTxn
        {
            Id = source.Id,
            BrandId = source.BrandId,
            Type = (int)BrandGameCreditTxnTypes.Debit,
            Money = source.Money,
            Amount = source.Amount,
            Remark = source.Remark,
            Expansions = source.Expansions.ToJsonString(),
            CreatedUserId = source.CreatedUserId,
            CreatedUsername = source.CreatedUsername,
            CreatedOn = DateTime.UtcNow
        };

        _ = await context.BrandGameCreditTxns.AddAsync(
            entity: entity,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = await context.SaveChangesAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public IAsyncEnumerable<GameCreditTxnInfo> QueryAsync(Guid brandId, BrandGameCreditTxnTypes? type, DateTimeOffset startOn, DateTimeOffset endOn, int offset = 0, int limit = 20, CancellationToken cancellationToken = default)
    {
        var query = context.BrandGameCreditTxns.Where(predicate => predicate.CreatedOn >= startOn.UtcDateTime
             && predicate.CreatedOn < endOn.UtcDateTime
             && predicate.BrandId == brandId);

        if (type.HasValue)
        {
            query = query.Where(predicate => predicate.Type == (int)type);
        }

        return query
         .OrderByDescending(keySelector: keySelector => keySelector.CreatedOn)
         .Skip(limit * offset)
         .Take(limit)
         .Select(x => new GameCreditTxnInfo(x.Id,
             x.BrandId,
             (BrandGameCreditTxnTypes)x.Type,
             x.Money,
             x.Amount,
             x.Remark,
             x.Expansions,
             x.CreatedUserId,
             x.CreatedUsername,
             x.CreatedOn))
         .ToAsyncEnumerable();
    }

    public async ValueTask<int> QueryCountAsync(Guid brandId, BrandGameCreditTxnTypes? type, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default)
    {
        var query = context.BrandGameCreditTxns.Where(predicate => predicate.CreatedOn >= startOn.UtcDateTime
                     && predicate.CreatedOn < endOn.UtcDateTime
                     && predicate.BrandId == brandId);

        if (type.HasValue)
        {
            query = query.Where(predicate => predicate.Type == (int)type);
        }
        return await query
            .CountAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
