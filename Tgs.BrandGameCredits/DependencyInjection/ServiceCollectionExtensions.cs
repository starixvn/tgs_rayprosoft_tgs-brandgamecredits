using Microsoft.EntityFrameworkCore;
using Tgs.BrandGameCredits;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrandGameCredits(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure,
        Action<RedisOptions, IServiceProvider> redisOptions)
        => services
        .AddOptions<RedisOptions>()
        .Configure(redisOptions)
        .Services
        .AddSingleton<RedisConnectionManager>()
        .AddBrandGameCreditsCore()
        .AddBrandGameCreditsRepositories(
            tgsConfigure: tgsConfigure,
            redisDatabaseResolver: (sp) => sp.GetRequiredService<RedisConnectionManager>().Database);
}
