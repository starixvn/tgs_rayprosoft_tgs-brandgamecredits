using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Tgs.BrandGameCredits.Client;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Expansions;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.IntegrationUnitTest;

public partial class BrandGameCreditsClientUnitTest
{
    [Fact]
    public async Task GameCreditSuspSetting_Get()
    {
        var brandCode = "GPI";

        var result = new GameCreditSuspSettingResult(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS",
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            BrandCode: "GPI",
            LowRisk: 60m,
            LowNotification: new BrandGameCreditSuspSettingNotificationMsg
            {
                EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
                EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            ModerateRisk: 85m,
            ModerateNotification: new BrandGameCreditSuspSettingNotificationMsg
            {
                EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度嚴重不足，所有遊戲暫時不可進入；為避免影響平台正常運營，請立即聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a higher balance usage, and the access to all games will be temporarily suspended. To maintain the stability of your site, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you.",
                EmailSubject = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
            },
            HighRisk: 95m,
            HighNotification: new BrandGameCreditSuspSettingNotificationMsg
            {
                EmailContent = "貴司您好，\r\n由於您的平台剩餘額度已低於限制，平台及所有遊戲暫時不可進入；請立即聯繫貴司對應商務進行加值，並由專人協助您恢復平台運行，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site has been temporarily suspended due to a balance usage that has exceeded the limit. To recover the full access of platform and all games, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you.",
                EmailSubject = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
            },
            ManualNotification: new BrandGameCreditSuspSettingNotificationMsg
            {
                EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
                EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low"
            },
            CreatedOn: DateTimeOffset.Now,
            UpdatedOn: DateTimeOffset.Now);
        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(services =>
            {
                var fakeService = Substitute.For<IGetService<GameCreditSuspSettingGetQuery, GameCreditSuspSettingResult>>();

                var query = new GameCreditSuspSettingGetQuery(
                    BrandCode: brandCode);

                _ = fakeService
             .GetAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
             .Returns(result);

                _ = services.AddTransient(_ => fakeService);
            });
        });

        var fakeLoggingHandler = new LoggingHandler(web.Services.GetRequiredService<ILogger<LoggingHandler>>());

        var httpClient = web.CreateDefaultClient(fakeLoggingHandler);

        var client = new BrandGameCreditsClient(httpClient);

        var actual = await client.GameCreditSuspSettingGetAsync(brandCode: brandCode);

        Assert.NotNull(actual);
        Assert.Equal(Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"), actual.ResellerId);
        Assert.Equal("TGS", actual.ResellerCode);
        Assert.Equal(Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"), actual.LicenseeId);
        Assert.Equal("TGS", actual.LicenseeCode);
        Assert.Equal(Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"), actual.BrandId);
        Assert.Equal("GPI", actual.BrandCode);
        Assert.Equal(60m, actual.LowRisk);
        Assert.Equal(result.LowNotification.ToJsonString(), actual.LowNotification.ToJsonString());
        Assert.Equal(85m, actual.ModerateRisk);
        Assert.Equal(result.ModerateNotification.ToJsonString(), actual.ModerateNotification.ToJsonString());
        Assert.Equal(95m, actual.HighRisk);
        Assert.Equal(result.HighNotification.ToJsonString(), actual.HighNotification.ToJsonString());
        Assert.Equal(result.ManualNotification.ToJsonString(), actual.ManualNotification.ToJsonString());
    }

    [Fact]
    public async Task GameCreditSuspSetting_Merge()
    {
        var brandCode = "GPI";
        var lowRisk = 60m;
        var lowNotification = new BrandGameCreditSuspSettingNotificationMsg
        {
            EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
            EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low"
        };
        var moderateRisk = 85m;
        var moderateNotification = new BrandGameCreditSuspSettingNotificationMsg
        {
            EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度嚴重不足，所有遊戲暫時不可進入；為避免影響平台正常運營，請立即聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a higher balance usage, and the access to all games will be temporarily suspended. To maintain the stability of your site, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you.",
            EmailSubject = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
        };
        var highRisk = 95m;
        var highNotification = new BrandGameCreditSuspSettingNotificationMsg
        {
            EmailContent = "貴司您好，\r\n由於您的平台剩餘額度已低於限制，平台及所有遊戲暫時不可進入；請立即聯繫貴司對應商務進行加值，並由專人協助您恢復平台運行，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site has been temporarily suspended due to a balance usage that has exceeded the limit. To recover the full access of platform and all games, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you.",
            EmailSubject = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
        };
        var manualNotification = new BrandGameCreditSuspSettingNotificationMsg
        {
            EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
            EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low"
        };

        var web = new TestBrandGameCreditsApplication(builder =>
        {
            _ = builder.ConfigureServices(async services =>
            {
                var fakeService = Substitute.For<ICommandService<GameCreditSuspSettingMergeCommand>>();

                var command = new GameCreditSuspSettingMergeCommand(
                    BrandCode: brandCode,
                    LowRisk: lowRisk,
                    LowNotification: lowNotification,
                    ModerateRisk: moderateRisk,
                    ModerateNotification: moderateNotification,
                    HighRisk: highRisk,
                    HighNotification: highNotification,
                    ManualNotification: manualNotification);

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

        await client.GameCreditSuspSettingMergeAsync(
            brandCode: brandCode,
            lowRisk: lowRisk,
            lowNotification: lowNotification,
            moderateRisk: moderateRisk,
            moderateNotification: moderateNotification,
            highRisk: highRisk,
            highNotification: highNotification,
            manualNotification: manualNotification);
    }
}
