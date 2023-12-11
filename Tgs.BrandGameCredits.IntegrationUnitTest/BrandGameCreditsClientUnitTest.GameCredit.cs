using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tgs.BrandGameCredits.Client;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.IntegrationUnitTest;

public partial class BrandGameCreditsClientUnitTest
{
    [Fact]
    public async Task GameCredit_Query()
    {
        var brandCode = "GPI";

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditQuery, GameCreditResult>>();

                var query = new GameCreditQuery(brandCode);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new GameCreditResult[] {
                    new(
                        ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                        ResellerCode: "TGS",
                        LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                        LicenseeCode: "TGS",
                        BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                        BrandCode: "GPI",
                        Month: DateOnly.Parse("2023-12-01"),
                        InitialAmount: 2m,
                        CreditAmount: 100m,
                        DebitAmount: 1m,
                        ProfitAmount: -33m,
                        CreditLimit: 101m,
                        CreditUsed: 33m,
                        CreditUsedPer: 32.6733m,
                        CreditAvailable: 68m,
                        CreditAvailablePer: 67.3267m,
                        CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
                        UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00")) }.ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditQueryAsync(
            brandCode: brandCode)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameCredit>();

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.First().ResellerId);
        Assert.Equal("TGS", actual.First().ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.First().LicenseeId);
        Assert.Equal("TGS", actual.First().LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.First().BrandId);
        Assert.Equal("GPI", actual.First().BrandCode);
        Assert.Equal(DateOnly.Parse("2023-12-01"), actual.First().Month);
        Assert.Equal(2m, actual.First().InitialAmount);
        Assert.Equal(100m, actual.First().CreditAmount);
        Assert.Equal(1, actual.First().DebitAmount);
        Assert.Equal(-33m, actual.First().ProfitAmount);
        Assert.Equal(101m, actual.First().CreditLimit);
        Assert.Equal(33m, actual.First().CreditUsed);
        Assert.Equal(32.6733m, actual.First().CreditUsedPer);
        Assert.Equal(68m, actual.First().CreditAvailable);
        Assert.Equal(67.3267m, actual.First().CreditAvailablePer);
        Assert.Equal(DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"), actual.First().CreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"), actual.First().UpdatedOn);
    }

    [Fact]
    public async Task GameCredit_Query_No_Data()
    {
        var brandCode = "GPI";

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditQuery, GameCreditResult>>();

                var query = new GameCreditQuery(brandCode);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(Enumerable.Empty<GameCreditResult>().ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditQueryAsync(
            brandCode: brandCode)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameCredit>();

        Assert.False(actual.Any());
    }

    [Fact]
    public async Task GameCredit_Get_Current()
    {
        var brandCode = "GPI";

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<GameCreditGetCurrentQuery, GameCreditResult?>>();

                var query = new GameCreditGetCurrentQuery(brandCode);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new GameCreditResult(
                        ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                        ResellerCode: "TGS",
                        LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                        LicenseeCode: "TGS",
                        BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                        BrandCode: "GPI",
                        Month: DateOnly.Parse("2023-12-01"),
                        InitialAmount: 2m,
                        CreditAmount: 100m,
                        DebitAmount: 1m,
                        ProfitAmount: -33m,
                        CreditLimit: 101m,
                        CreditUsed: 33m,
                        CreditUsedPer: 32.6733m,
                        CreditAvailable: 68m,
                        CreditAvailablePer: 67.3267m,
                        CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
                        UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00")));

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditGetCurrentAsync(brandCode: brandCode);

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.ResellerId);
        Assert.Equal("TGS", actual.ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.LicenseeId);
        Assert.Equal("TGS", actual.LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.BrandId);
        Assert.Equal("GPI", actual.BrandCode);
        Assert.Equal(DateOnly.Parse("2023-12-01"), actual.Month);
        Assert.Equal(2m, actual.InitialAmount);
        Assert.Equal(100m, actual.CreditAmount);
        Assert.Equal(1, actual.DebitAmount);
        Assert.Equal(-33m, actual.ProfitAmount);
        Assert.Equal(101m, actual.CreditLimit);
        Assert.Equal(33m, actual.CreditUsed);
        Assert.Equal(32.6733m, actual.CreditUsedPer);
        Assert.Equal(68m, actual.CreditAvailable);
        Assert.Equal(67.3267m, actual.CreditAvailablePer);
        Assert.Equal(DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"), actual.CreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"), actual.UpdatedOn);
    }

    [Fact]
    public async Task GameCredit_Get_Current_No_Data()
    {
        var brandCode = "GPI";

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<GameCreditGetCurrentQuery, GameCreditResult?>>();

                var query = new GameCreditGetCurrentQuery(brandCode);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns((GameCreditResult?)null);

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditGetCurrentAsync(brandCode: brandCode);

        Assert.Null(actual);
    }
}
