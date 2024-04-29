using Microsoft.Extensions.Options;
using Tgs.BrandGameCredits.Client;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBrandGameCreditsClient(
		this IServiceCollection services,
		Action<BrandGameCreditsClientOptions, IServiceProvider> configure)
		=>
		services
		.AddOptions<BrandGameCreditsClientOptions>()
		.Configure(configure)
		.Services
		.AddHttpClient<IBrandGameCreditsClient, BrandGameCreditsClient>()
		.ConfigureHttpClient((sp, httpClient) =>
		{
			var options = sp.GetRequiredService<IOptions<BrandGameCreditsClientOptions>>().Value;
			httpClient.BaseAddress = options.BaseUrl;
		})
		.AddHttpMessageHandler<LoggingHandler>()
		.Services
		.AddTransient<LoggingHandler>();
}
