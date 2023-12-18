using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tgs.BrandGameCredits.Client;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.IntegrationUnitTest;

public partial class BrandGameCreditsClientUnitTest
{
    [Fact]
    public async Task GameCreditProfit_History_Query()
    {
        var brandCode = "GPI";
        var month = DateOnly.Parse("2023-12-01");

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditProfitQuery, GameCreditProfitResult>>();

                var query = new GameCreditProfitQuery(
                    brandCode,
                    month);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new GameCreditProfitResult[] {
                    new(
                        ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                        ResellerCode: "TGS",
                        LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                        LicenseeCode: "TGS",
                        BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                        BrandCode: "GPI",
                        Month: DateOnly.Parse("2023-12-01"),
                        GameProviderId: Guid.Parse("6214260a-cf3c-4fb5-ba78-9214fe5608bf"),
                        GameProviderCode: "AWC_PGS",
                        GameProviderName: "PGSoft",
                        Count: 20,
                        Bet: 10m,
                        ValidBet: 10m,
                        Settle: 9.7m,
                        Commission: 0m,
                        Turnover: 10m,
                        WinLoss: -0.3m,
                        ProfitAmount: 0.3m) }.ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditProfitQueryAsync(
            brandCode: brandCode,
            month: month)
            .ToArrayAsync()
            ?? Enumerable.Empty<BrandGameCreditProfit>();

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.First().ResellerId);
        Assert.Equal("TGS", actual.First().ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.First().LicenseeId);
        Assert.Equal("TGS", actual.First().LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.First().BrandId);
        Assert.Equal("GPI", actual.First().BrandCode);
        Assert.Equal(DateOnly.Parse("2023-12-01"), actual.First().Month);
        Assert.Equal(Guid.Parse("6214260a-cf3c-4fb5-ba78-9214fe5608bf"), actual.First().GameProviderId);
        Assert.Equal(20, actual.First().Count);
        Assert.Equal(10m, actual.First().Bet);
        Assert.Equal(10m, actual.First().ValidBet);
        Assert.Equal(9.7m, actual.First().Settle);
        Assert.Equal(0m, actual.First().Commission);
        Assert.Equal(10, actual.First().Turnover);
        Assert.Equal(-0.3m, actual.First().WinLoss);
        Assert.Equal(0.3m, actual.First().ProfitAmount);
    }

    [Fact]
    public async Task GameCreditProfit_History_Query_No_Data()
    {
        var brandCode = "GPI";
        var month = DateOnly.Parse("2023-12-01");

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditProfitQuery, GameCreditProfitResult>>();

                var query = new GameCreditProfitQuery(
                    brandCode,
                    month);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(Enumerable.Empty<GameCreditProfitResult>().ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditProfitQueryAsync(
            brandCode: brandCode,
            month: month)
            .ToArrayAsync()
            ?? Enumerable.Empty<BrandGameCreditProfit>();

        Assert.False(actual.Any());
    }
}
