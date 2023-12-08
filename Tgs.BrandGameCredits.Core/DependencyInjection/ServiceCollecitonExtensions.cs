using System.Reflection;
using Tgs.BrandGameCredits.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
    public static IServiceCollection AddBrandGameCreditsCore(
        this IServiceCollection services)
        => services
        .RegisterAllServiceUtilityTypes(
            Assembly.GetExecutingAssembly(),
            ServiceLifetime.Transient)
        .AddSerialNumberGenerator()
        .AddSingleton<ISystemClock, SystemClock>()
        .AddSingleton<IGameCreditTimeUtnsil, GameCreditTimeUtnsil>()
        .AddSingleton<IComputingUtnsil, ComputingUtnsil>();
}
