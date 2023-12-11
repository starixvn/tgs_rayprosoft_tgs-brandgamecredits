using Microsoft.EntityFrameworkCore;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Repositories.Entities;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandGameCreditSuspSettingRepository(TgsContext context) : IBrandGameCreditSuspSettingRepository
{
    public async ValueTask AddAsync(BrandGameCreditSuspSettingAdd source, CancellationToken cancellationToken = default)
    {
        var entity = new BrandGameCreditSuspSetting
        {
            BrandId = source.BrandId,
            LowRisk = source.LowRisk,
            LowNotification = source.LowNotification.ToJsonString(),
            ModerateRisk = source.ModerateRisk,
            ModerateNotification = source.ModerateNotification.ToJsonString(),
            HighRisk = source.HighRisk,
            HighNotification = source.HighNotification.ToJsonString(),
            ManualNotification = source.ManualNotification.ToJsonString(),
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        _ = await context.BrandGameCreditSuspSettings.AddAsync(
            entity: entity,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        _ = await context.SaveChangesAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<BrandGameCreditSuspSettingInfo?> GetAsync(Guid brandId, CancellationToken cancellationToken = default)
    => await context.BrandGameCreditSuspSettings
        .Where(predicate: predicate => predicate.BrandId == brandId)
        .Select(selector: selector => new BrandGameCreditSuspSettingInfo(
            selector.BrandId,
            selector.LowRisk,
            selector.LowNotification,
            selector.ModerateRisk,
            selector.ModerateNotification,
            selector.HighRisk,
            selector.HighNotification,
            selector.Credit,
            selector.CreditNotification,
            selector.ManualNotification,
            selector.CreatedOn,
            selector.UpdatedOn
            ))
        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask UpdateAsync(BrandGameCreditSuspSettingUpdate source, CancellationToken cancellationToken = default)
    {
        var entity = await context.BrandGameCreditSuspSettings
            .SingleAsync(predicate: predicate => predicate.BrandId == source.BrandId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        entity.LowRisk = source.LowRisk;
        entity.LowNotification = source.LowNotification.ToJsonString();
        entity.ModerateRisk = source.ModerateRisk;
        entity.ModerateNotification = source.ModerateNotification.ToJsonString();
        entity.HighRisk = source.HighRisk;
        entity.HighNotification = source.HighNotification.ToJsonString();
        entity.ManualNotification = source.ManualNotification.ToJsonString();
        entity.UpdatedOn = DateTime.UtcNow;

        _ = context.BrandGameCreditSuspSettings.Update(entity: entity);

        _ = await context.SaveChangesAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
