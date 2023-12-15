using NSubstitute;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Handler.Common;
using Tgs.BrandGameCredits.Handler.Common.Models;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditGetCurrentGetServiceUnitTest
{
    [Fact]
    public async void Normal_Process()
    {
        var fakeBrandRepository = Substitute.For<IBrandRepository>();
        var fakeBrandGameCreditRepository = Substitute.For<IBrandGameCreditRepository>();
        var fakeComputingUtnsil = Substitute.For<IBrandGameCreditComputingUtnsil>();
        var fakeRiskUtnsil = Substitute.For<IBrandGameCreditRiskUtnsil>();

        var query = new GameCreditGetCurrentQuery(
            BrandCode: "GPI");

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

        var gameCreditInfo = new GameCreditInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            Month: DateOnly.Parse("2023-12-01"),
            InitialAmount: 2,
            CreditAmount: 100,
            DebitAmount: 1,
            ProfitAmount: -33m,
            CreatedOn: DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"),
            UpdatedOn: DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"));
        _ = fakeBrandGameCreditRepository.GetCurrentAsync(
            Arg.Is(brandInfo.BrandId),
            Arg.Any<CancellationToken>())
            .Returns(gameCreditInfo);

        var creditCalculationInfo = new CreditCalculationInfo(
            CreditLimit: 101m,
            CreditUsed: 33m,
            CreditUsedPer: 32.6733m,
            CreditAvailable: 68m,
            CreditAvailablePer: 67.3267m);
        _ = fakeComputingUtnsil.CreditCalculation(
            initialAmount: Arg.Is(gameCreditInfo.InitialAmount),
            creditAmount: Arg.Is(gameCreditInfo.CreditAmount),
            debitAmount: Arg.Is(gameCreditInfo.DebitAmount),
            profitAmount: Arg.Is(gameCreditInfo.ProfitAmount))
            .Returns(creditCalculationInfo);

        var riskType = BrandGameCreditRiskTypes.NormalRisk;
        _ = fakeRiskUtnsil.GetRiskTypeAsync(
            brandId: Arg.Is(brandInfo.BrandId),
            creditUsedPer: Arg.Is(32.6733m),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(riskType);

        var service = new GameCreditGetCurrentGetService(
            fakeBrandRepository,
            fakeBrandGameCreditRepository,
            fakeComputingUtnsil,
            fakeRiskUtnsil);

        var actual = await service.GetAsync(query);

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
        Assert.Equal(BrandGameCreditRiskTypes.NormalRisk, actual.CreditRiskType);
        Assert.Equal(DateTimeOffset.Parse("2023-12-06T09:19:50.010516+00:00"), actual.CreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-12-07T10:38:10.350982+00:00"), actual.UpdatedOn);
    }
}
