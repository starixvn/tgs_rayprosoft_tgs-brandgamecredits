using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NSubstitute;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core.UnitTest;

public class BrandGameCreditRiskUtnsilUnitTest
{
    [Fact]
    public async void Normal_Risk()
    {
        var fakeBrandGameCreditSuspSettingRepository = Substitute.For<IBrandGameCreditSuspSettingRepository>();

        var brandId = Guid.Empty;
        var creditUsedPer = 59.9999m;

        var info = new BrandGameCreditSuspSettingInfo(
            BrandId: Guid.Empty,
            LowRisk: 60,
            LowNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ModerateRisk: 80,
            ModerateNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
            },
            HighRisk: 95,
            HighNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
            },
            Credit: 0,
            CreditNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ManualNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            CreatedOn: DateTimeOffset.UtcNow,
             UpdatedOn: DateTimeOffset.UtcNow);

        _ = fakeBrandGameCreditSuspSettingRepository.GetAsync(
            brandId: brandId,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(info);

        var utnsil = new BrandGameCreditRiskUtnsil(fakeBrandGameCreditSuspSettingRepository);

        var actual = await utnsil.GetRiskTypeAsync(brandId, creditUsedPer);

        Assert.Equal(BrandGameCreditRiskTypes.NormalRisk, actual);
    }

    [Fact]
    public async void Low_Risk()
    {
        var fakeBrandGameCreditSuspSettingRepository = Substitute.For<IBrandGameCreditSuspSettingRepository>();

        var brandId = Guid.Empty;
        var creditUsedPer = 79.9999m;

        var info = new BrandGameCreditSuspSettingInfo(
            BrandId: Guid.Empty,
            LowRisk: 60,
            LowNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ModerateRisk: 80,
            ModerateNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
            },
            HighRisk: 95,
            HighNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
            },
            Credit: 0,
            CreditNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ManualNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            CreatedOn: DateTimeOffset.UtcNow,
             UpdatedOn: DateTimeOffset.UtcNow);

        _ = fakeBrandGameCreditSuspSettingRepository.GetAsync(
            brandId: brandId,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(info);

        var utnsil = new BrandGameCreditRiskUtnsil(fakeBrandGameCreditSuspSettingRepository);

        var actual = await utnsil.GetRiskTypeAsync(brandId, creditUsedPer);

        Assert.Equal(BrandGameCreditRiskTypes.LowRisk, actual);
    }

    [Fact]
    public async void Moderate_Risk()
    {
        var fakeBrandGameCreditSuspSettingRepository = Substitute.For<IBrandGameCreditSuspSettingRepository>();

        var brandId = Guid.Empty;
        var creditUsedPer = 94.9999m;

        var info = new BrandGameCreditSuspSettingInfo(
            BrandId: Guid.Empty,
            LowRisk: 60,
            LowNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ModerateRisk: 80,
            ModerateNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
            },
            HighRisk: 95,
            HighNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
            },
            Credit: 0,
            CreditNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ManualNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            CreatedOn: DateTimeOffset.UtcNow,
             UpdatedOn: DateTimeOffset.UtcNow);

        _ = fakeBrandGameCreditSuspSettingRepository.GetAsync(
            brandId: brandId,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(info);

        var utnsil = new BrandGameCreditRiskUtnsil(fakeBrandGameCreditSuspSettingRepository);

        var actual = await utnsil.GetRiskTypeAsync(brandId, creditUsedPer);

        Assert.Equal(BrandGameCreditRiskTypes.ModerateRisk, actual);
    }

    [Fact]
    public async void High_Risk()
    {
        var fakeBrandGameCreditSuspSettingRepository = Substitute.For<IBrandGameCreditSuspSettingRepository>();

        var brandId = Guid.Empty;
        var creditUsedPer = 95.0001m;

        var info = new BrandGameCreditSuspSettingInfo(
            BrandId: Guid.Empty,
            LowRisk: 60,
            LowNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ModerateRisk: 80,
            ModerateNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
            },
            HighRisk: 95,
            HighNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
            },
            Credit: 0,
            CreditNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ManualNotification: new JsonObject
            {
                ["Subject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            CreatedOn: DateTimeOffset.UtcNow,
             UpdatedOn: DateTimeOffset.UtcNow);

        _ = fakeBrandGameCreditSuspSettingRepository.GetAsync(
            brandId: brandId,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(info);

        var utnsil = new BrandGameCreditRiskUtnsil(fakeBrandGameCreditSuspSettingRepository);

        var actual = await utnsil.GetRiskTypeAsync(brandId, creditUsedPer);

        Assert.Equal(BrandGameCreditRiskTypes.HighRisk, actual);
    }
}
