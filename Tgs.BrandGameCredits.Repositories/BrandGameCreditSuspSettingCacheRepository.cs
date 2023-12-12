using System.Text.Json;
using StackExchange.Redis;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandGameCreditSuspSettingCacheRepository(
    IDatabase database,
    IBrandGameCreditSuspSettingRepository repository,
    TimeSpan Expiry) : IBrandGameCreditSuspSettingRepository
{
    public async ValueTask AddAsync(GameCreditSuspSettingAdd source, CancellationToken cancellationToken = default)
    {
        await repository.AddAsync(
            source: source,
            cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        _ = await DeleteAsync(
            source.BrandId)
            .ConfigureAwait(false);
    }

    public async ValueTask<GameCreditSuspSettingInfo?> GetAsync(Guid brandId, CancellationToken cancellationToken = default)
    {
        var info = await GetAsync(brandId)
            .ConfigureAwait(false);

        if (info is null)
        {
            info = await repository.GetAsync(
                brandId: brandId,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            if (info is not null)
            {
                _ = await SetAsync(
                    source: info)
                    .ConfigureAwait(false);
            }
        }

        return info;
    }

    public async ValueTask UpdateAsync(GameCreditSuspSettingUpdate source, CancellationToken cancellationToken = default)
    {
        await repository.UpdateAsync(
            source: source,
            cancellationToken: cancellationToken)
             .ConfigureAwait(false);

        _ = await DeleteAsync(
            source.BrandId)
            .ConfigureAwait(false);
    }

    private static string GetKey(Guid brandId)
    => string.Concat("brandgamecredit:suspsetting:", brandId);

    private async ValueTask<GameCreditSuspSettingInfo?> GetAsync(Guid brandId)
    {
        var key = GetKey(brandId);
        if (await database.KeyExistsAsync(key).ConfigureAwait(false))
        {
            var value = await database.StringGetAsync(
                key: key)
                .ConfigureAwait(false);

            if (!value.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<GameCreditSuspSettingInfo>(value.ToString());
            }
        }
        return null;
    }

    private async ValueTask<bool> SetAsync(GameCreditSuspSettingInfo source)
        => await database.StringSetAsync(
            key: GetKey(source.BrandId),
            value: JsonSerializer.Serialize(source),
            expiry: Expiry)
        .ConfigureAwait(false);

    private async ValueTask<bool> DeleteAsync(Guid brandId)
        => await database.KeyDeleteAsync(
            key: GetKey(brandId),
            flags: CommandFlags.FireAndForget)
        .ConfigureAwait(false);
}
