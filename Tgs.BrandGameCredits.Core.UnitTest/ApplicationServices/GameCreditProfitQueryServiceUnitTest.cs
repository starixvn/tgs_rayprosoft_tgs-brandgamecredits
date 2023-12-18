using NSubstitute;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditProfitQueryServiceUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var fakeBrandRepository = Substitute.For<IBrandRepository>();
        var fakeBrandGameCreditProfitRepository = Substitute.For<IBrandGameCreditProfitRepository>();

        var query = new GameCreditProfitQuery(
            BrandCode: "GPI",
            Month: DateOnly.Parse("2023-12-01"));

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

        var brandGameCreditProfitInfo = new BrandGameCreditProfitInfo(
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
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
            ProfitAmount: 0.3m);
        _ = fakeBrandGameCreditProfitRepository.QueryAsync(
            Arg.Is(brandInfo.BrandId),
            Arg.Is(query.Month),
            Arg.Any<CancellationToken>())
            .Returns(new BrandGameCreditProfitInfo[] {
                brandGameCreditProfitInfo }.ToAsyncEnumerable());

        var service = new GameCreditProfitQueryService(
            fakeBrandRepository,
            fakeBrandGameCreditProfitRepository);

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
}
