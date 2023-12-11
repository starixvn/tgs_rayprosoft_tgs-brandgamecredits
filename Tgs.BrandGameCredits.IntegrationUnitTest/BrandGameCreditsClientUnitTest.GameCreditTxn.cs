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
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core;
using Tgs.ServiceUtility;
using System.Text.Json.Nodes;
using NATS.Client.JetStream;

namespace Tgs.BrandGameCredits.IntegrationUnitTest;

public partial class BrandGameCreditsClientUnitTest
{
    [Fact]
    public async Task GameCreditTxn_Query()
    {
        var brandCode = "GPI";
        var startOn = new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8));
        var endOn = new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8));
        var type = GameCreditTxnTypes.Credit;
        var pageNumber = 1;
        var pageSize = 20;

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditTxnQuery, GameCreditTxnResult>>();

                var query = new GameCreditTxnQuery(
                    BrandCode: brandCode,
                    StartOn: startOn,
                    EndOn: endOn,
                    Type: (BrandGameCreditTxnTypes?)type,
                    PageNumber: pageNumber,
                    PageSize: pageSize);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new GameCreditTxnResult[] {
                    new(
                        Id: Guid.NewGuid(),
                        ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                        ResellerCode: "TGS",
                        LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                        LicenseeCode: "TGS",
                        BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                        BrandCode: "GPI",
                        Type: BrandGameCreditTxnTypes.Credit,
                        Money: 100m,
                        Amount: 1000m,
                        Remark: "text",
                        Expansions: new JsonObject { ["User"] = "test" },
                        CreatedUserId: Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"),
                        CreatedUsername: "admin.enabled",
                        CreatedOn: DateTimeOffset.Parse("2023-12-06T10:23:22.533+08:00"))}.ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditTxnQueryAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            type: type,
            pageNumber: pageNumber,
            pageSize: pageSize)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameCreditTxn>();

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.First().ResellerId);
        Assert.Equal("TGS", actual.First().ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.First().LicenseeId);
        Assert.Equal("TGS", actual.First().LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.First().BrandId);
        Assert.Equal("GPI", actual.First().BrandCode);
        Assert.Equal(GameCreditTxnTypes.Credit, actual.First().Type);
        Assert.Equal(100m, actual.First().Money);
        Assert.Equal(1000m, actual.First().Amount);
        Assert.Equal("text", actual.First().Remark);
        Assert.Equal(new JsonObject { ["User"] = "test" }.ToJsonString(), actual.First().Expansions.ToJsonString());
        Assert.Equal(Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"), actual.First().CreatedUserId);
        Assert.Equal("admin.enabled", actual.First().CreatedUsername);
        Assert.Equal(DateTimeOffset.Parse("2023-12-06T10:23:22.533+08:00"), actual.First().CreatedOn);
    }

    [Fact]
    public async Task GameCreditTxn_Query_Type_Is_Null()
    {
        var brandCode = "GPI";
        var startOn = new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8));
        var endOn = new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8));
        var type = (GameCreditTxnTypes?)null;
        var pageNumber = 1;
        var pageSize = 20;

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditTxnQuery, GameCreditTxnResult>>();

                var query = new GameCreditTxnQuery(
                    BrandCode: brandCode,
                    StartOn: startOn,
                    EndOn: endOn,
                    Type: (BrandGameCreditTxnTypes?)type,
                    PageNumber: pageNumber,
                    PageSize: pageSize);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(new GameCreditTxnResult[] {
                    new(
                        Id: Guid.NewGuid(),
                        ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
                        ResellerCode: "TGS",
                        LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
                        LicenseeCode: "TGS",
                        BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
                        BrandCode: "GPI",
                        Type: BrandGameCreditTxnTypes.Credit,
                        Money: 100m,
                        Amount: 1000m,
                        Remark: "text",
                        Expansions: new JsonObject { ["User"] = "test" },
                        CreatedUserId: Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"),
                        CreatedUsername: "admin.enabled",
                        CreatedOn: DateTimeOffset.Parse("2023-12-06T10:23:22.533+08:00"))}.ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditTxnQueryAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            type: type,
            pageNumber: pageNumber,
            pageSize: pageSize)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameCreditTxn>();

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.First().ResellerId);
        Assert.Equal("TGS", actual.First().ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.First().LicenseeId);
        Assert.Equal("TGS", actual.First().LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.First().BrandId);
        Assert.Equal("GPI", actual.First().BrandCode);
        Assert.Equal(GameCreditTxnTypes.Credit, actual.First().Type);
        Assert.Equal(100m, actual.First().Money);
        Assert.Equal(1000m, actual.First().Amount);
        Assert.Equal("text", actual.First().Remark);
        Assert.Equal(new JsonObject { ["User"] = "test" }.ToJsonString(), actual.First().Expansions.ToJsonString());
        Assert.Equal(Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"), actual.First().CreatedUserId);
        Assert.Equal("admin.enabled", actual.First().CreatedUsername);
        Assert.Equal(DateTimeOffset.Parse("2023-12-06T10:23:22.533+08:00"), actual.First().CreatedOn);
    }

    [Fact]
    public async Task GameCreditTxn_Query_No_Data()
    {
        var brandCode = "GPI";
        var startOn = new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8));
        var endOn = new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8));
        var type = GameCreditTxnTypes.Credit;
        var pageNumber = 1;
        var pageSize = 20;

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IQueryService<GameCreditTxnQuery, GameCreditTxnResult>>();

                var query = new GameCreditTxnQuery(
                    BrandCode: brandCode,
                    StartOn: startOn,
                    EndOn: endOn,
                    Type: (BrandGameCreditTxnTypes?)type,
                    PageNumber: pageNumber,
                    PageSize: pageSize);

                _ = fakeService.QueryAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(Enumerable.Empty<GameCreditTxnResult>().ToAsyncEnumerable());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditTxnQueryAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            type: type,
            pageNumber: pageNumber,
            pageSize: pageSize)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameCreditTxn>();

        Assert.False(actual.Any());
    }

    [Fact]
    public async Task GameCreditTxn_Query_Count()
    {
        var brandCode = "GPI";
        var startOn = new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8));
        var endOn = new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8));
        var type = GameCreditTxnTypes.Credit;

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<GameCreditTxnQueryCountQuery, int>>();

                var query = new GameCreditTxnQueryCountQuery(
                    BrandCode: brandCode,
                    StartOn: startOn,
                    EndOn: endOn,
                    Type: (BrandGameCreditTxnTypes?)type);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(1);

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditTxnQueryCountAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            type: type);

        Assert.Equal(1, actual);
    }

    [Fact]
    public async Task GameCreditTxn_Query_Coun_Is_Nullt()
    {
        var brandCode = "GPI";
        var startOn = new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8));
        var endOn = new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8));
        var type = (GameCreditTxnTypes?)null;

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<GameCreditTxnQueryCountQuery, int>>();

                var query = new GameCreditTxnQueryCountQuery(
                    BrandCode: brandCode,
                    StartOn: startOn,
                    EndOn: endOn,
                    Type: (BrandGameCreditTxnTypes?)type);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(1);

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditTxnQueryCountAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            type: type);

        Assert.Equal(1, actual);
    }

    [Fact]
    public async Task GameCreditTxn_Query_Count_No_Data()
    {
        var brandCode = "GPI";
        var startOn = new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8));
        var endOn = new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8));
        var type = GameCreditTxnTypes.Credit;

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<GameCreditTxnQueryCountQuery, int>>();

                var query = new GameCreditTxnQueryCountQuery(
                    BrandCode: brandCode,
                    StartOn: startOn,
                    EndOn: endOn,
                    Type: (BrandGameCreditTxnTypes?)type);

                _ = fakeService.GetAsync(
                    query: Arg.Is(query),
                    cancellationToken: Arg.Any<CancellationToken>())
                .Returns(0);

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditTxnQueryCountAsync(
            brandCode: brandCode,
            startOn: startOn,
            endOn: endOn,
            type: type);

        Assert.Equal(0, actual);
    }

    [Fact]
    public async Task GameCreditTxn_Add_Credit()
    {
        var brandCode = "GPI";
        var money = 100m;
        var amount = 1000m;
        var remark = "test";
        var expansions = new JsonObject
        {
            ["Name"] = "admin.enabled"
        };
        var createdUserId = Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5");
        var createdUsername = "admin.enabled";

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(async services =>
            {
                var fakeService = Substitute.For<ICommandService<GameCreditTxnAddCreditCommand>>();

                var command = new GameCreditTxnAddCreditCommand(
                    BrandCode: brandCode,
                    Money: money,
                    Amount: amount,
                    Remark: remark,
                    Expansions: expansions,
                    CreatedUserId: createdUserId,
                    CreatedUsername: createdUsername);

                await fakeService
                .ExecuteAsync(
                    command: Arg.Is(command),
                    cancellationToken: Arg.Any<CancellationToken>());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        await client.GameCreditTxnAddCreditAsync(
            brandCode: brandCode,
            money: money,
            amount: amount,
            remark: remark,
            expansions: expansions,
            createdUserId: createdUserId,
            createdUsername: createdUsername);
    }

    [Fact]
    public async Task GameCreditTxn_Add_Debit()
    {
        var brandCode = "GPI";
        var money = 100m;
        var amount = 1000m;
        var remark = "test";
        var expansions = new JsonObject
        {
            ["Name"] = "admin.enabled"
        };
        var createdUserId = Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5");
        var createdUsername = "admin.enabled";

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(async services =>
            {
                var fakeService = Substitute.For<ICommandService<GameCreditTxnAddDebitCommand>>();

                var command = new GameCreditTxnAddDebitCommand(
                    BrandCode: brandCode,
                    Money: money,
                    Amount: amount,
                    Remark: remark,
                    Expansions: expansions,
                    CreatedUserId: createdUserId,
                    CreatedUsername: createdUsername);

                await fakeService
                .ExecuteAsync(
                    command: Arg.Is(command),
                    cancellationToken: Arg.Any<CancellationToken>());

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        await client.GameCreditTxnAddDebitAsync(
            brandCode: brandCode,
            money: money,
            amount: amount,
            remark: remark,
            expansions: expansions,
            createdUserId: createdUserId,
            createdUsername: createdUsername);
    }
}
