using System.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Repositories;
using Tgs.BrandGameCredits.Repositories.Entities;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollecitonExtensions
{
    public static IServiceCollection AddBrandGameCreditsRepositories(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> tgsConfigure)
        => services
        .AddDbContext<TgsContext>(tgsConfigure)
        .AddScoped<IDbConnection, NpgsqlConnection>((sp) => (NpgsqlConnection)sp.GetRequiredService<TgsContext>().Database.GetDbConnection())
        .AddTransient<IBrandGameCreditRepository, BrandGameCreditRepository>()
        .AddTransient<IBrandGameCreditTxnRepository, BrandGameCreditTxnRepository>()
        .AddTransient<IBrandRepository, BrandRepository>();
}
