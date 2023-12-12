using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NSubstitute;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditTxnQueryServiceUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var fakeBrandRepository = Substitute.For<IBrandRepository>();
        var fakeBrandGameCreditRepository = Substitute.For<IBrandGameCreditTxnRepository>();

        var query = new GameCreditTxnQuery(
            BrandCode: "GPI",
            StartOn: new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8)),
            EndOn: new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8)),
            Type: BrandGameCreditTxnTypes.Credit,
            PageNumber: 1,
            PageSize: 20);

        var brandInfo = new BrandInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS",
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            BrandCode: "GPI",
            TimeSpanMinutes: 480);
        _ = fakeBrandRepository.GetAsync(
            Arg.Is(query.BrandCode),
            Arg.Any<CancellationToken>())
            .Returns(brandInfo);

        var brandGameCreditTxnInfo = new GameCreditTxnInfo(
            Id: Guid.NewGuid(),
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            Type: BrandGameCreditTxnTypes.Credit,
            Money: 100m,
            Amount: 1000m,
            Remark: "text",
            Expansions: new JsonObject { ["User"] = "test" }.ToJsonString(),
            CreatedUserId: Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"),
            CreatedUsername: "admin.enabled",
            CreatedOn: DateTimeOffset.Parse("2023-12-06T10:23:22.533+08:00"));
        _ = fakeBrandGameCreditRepository.QueryAsync(
            brandId: Arg.Is(brandInfo.BrandId),
            type: Arg.Is(query.Type),
            startOn: Arg.Is(query.StartOn),
            endOn: Arg.Is(query.EndOn),
            offset: Arg.Is(query.PageNumber - 1),
            limit: Arg.Is(query.PageSize),
            Arg.Any<CancellationToken>())
            .Returns(new GameCreditTxnInfo[] {
                brandGameCreditTxnInfo }.ToAsyncEnumerable());

        var service = new GameCreditTxnQueryService(
            fakeBrandRepository,
            fakeBrandGameCreditRepository);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.First().ResellerId);
        Assert.Equal("TGS", actual.First().ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.First().LicenseeId);
        Assert.Equal("TGS", actual.First().LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.First().BrandId);
        Assert.Equal("GPI", actual.First().BrandCode);
        Assert.Equal(BrandGameCreditTxnTypes.Credit, actual.First().Type);
        Assert.Equal(100m, actual.First().Money);
        Assert.Equal(1000m, actual.First().Amount);
        Assert.Equal("text", actual.First().Remark);
        Assert.Equal(new JsonObject { ["User"] = "test" }.ToJsonString(), actual.First().Expansions.ToJsonString());
        Assert.Equal(Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"), actual.First().CreatedUserId);
        Assert.Equal("admin.enabled", actual.First().CreatedUsername);
        Assert.Equal(DateTimeOffset.Parse("2023-12-06T10:23:22.533+08:00"), actual.First().CreatedOn);
    }
}
