using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using NSubstitute;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Expansions;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditSuspSettingMergeCommandServiceUnitTest
{
    [Fact]
    public async void GameCreditSuspSetting_Add()
    {
        var fakeBrandRepository = Substitute.For<IBrandRepository>();
        var fakeBrandGameCreditSuspSettingRepository = Substitute.For<IBrandGameCreditSuspSettingRepository>();

        var command = new GameCreditSuspSettingMergeCommand(
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
            });

        var brandInfo = new BrandInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS",
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            BrandCode: "GPI",
            TimeSpanMinutes: 480);
        _ = fakeBrandRepository.GetAsync(
            code: Arg.Is(command.BrandCode),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(brandInfo);

        _ = fakeBrandGameCreditSuspSettingRepository.GetAsync(
            brandId: Arg.Is(brandInfo.BrandId),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns((GameCreditSuspSettingInfo?)null);

        var service = new GameCreditSuspSettingMergeCommandService(
            fakeBrandRepository,
            fakeBrandGameCreditSuspSettingRepository);

        await service.ExecuteAsync(command);

        _ = fakeBrandGameCreditSuspSettingRepository
            .Received(1)
            .AddAsync(
            source: Arg.Is(new GameCreditSuspSettingAdd(
                BrandId: brandInfo.BrandId,
                LowRisk: command.LowRisk,
                LowNotification: command.LowNotification,
                ModerateRisk: command.ModerateRisk,
                ModerateNotification: command.ModerateNotification,
                HighRisk: command.HighRisk,
                HighNotification: command.HighNotification,
                ManualNotification: command.ManualNotification)),
            Arg.Any<CancellationToken>());

        _ = fakeBrandGameCreditSuspSettingRepository
            .Received(0)
            .UpdateAsync(
            source: Arg.Is(new GameCreditSuspSettingUpdate(
                BrandId: brandInfo.BrandId,
                LowRisk: command.LowRisk,
                LowNotification: command.LowNotification,
                ModerateRisk: command.ModerateRisk,
                ModerateNotification: command.ModerateNotification,
                HighRisk: command.HighRisk,
                HighNotification: command.HighNotification,
                ManualNotification: command.ManualNotification)),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void GameCreditSuspSetting_Update()
    {
        var fakeBrandRepository = Substitute.For<IBrandRepository>();
        var fakeBrandGameCreditSuspSettingRepository = Substitute.For<IBrandGameCreditSuspSettingRepository>();

        var command = new GameCreditSuspSettingMergeCommand(
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
            });

        var brandInfo = new BrandInfo(
            ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
            ResellerCode: "TGS",
            LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
            LicenseeCode: "TGS",
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            BrandCode: "GPI",
            TimeSpanMinutes: 480);
        _ = fakeBrandRepository.GetAsync(
            code: Arg.Is(command.BrandCode),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(brandInfo);

        var info = new GameCreditSuspSettingInfo(
            BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
            LowRisk: 59m,
            LowNotification: new JsonObject
            {
                ["EmailContent"] = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
                ["EmailSubject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            }.ToString(),
            ModerateRisk: 79m,
            ModerateNotification: new JsonObject
            {
                ["EmailContent"] = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度嚴重不足，所有遊戲暫時不可進入；為避免影響平台正常運營，請立即聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a higher balance usage, and the access to all games will be temporarily suspended. To maintain the stability of your site, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you.",
                ["EmailSubject"] = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low"
            }.ToString(),
            HighRisk: 89m,
            HighNotification: new JsonObject
            {
                ["EmailContent"] = "貴司您好，\r\n由於您的平台剩餘額度已低於限制，平台及所有遊戲暫時不可進入；請立即聯繫貴司對應商務進行加值，並由專人協助您恢復平台運行，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site has been temporarily suspended due to a balance usage that has exceeded the limit. To recover the full access of platform and all games, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you.",
                ["EmailSubject"] = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension"
            }.ToString(),
            Credit: 0m,
            CreditNotification: new JsonObject
            {
                ["EmailContent"] = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
                ["EmailSubject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            }.ToString(),
            ManualNotification: new JsonObject
            {
                ["EmailContent"] = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you.",
                ["EmailSubject"] = "[Licensee][Brand] 加值通知 Warning: Balance low"
            }.ToString(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        _ = fakeBrandGameCreditSuspSettingRepository.GetAsync(
            brandId: Arg.Is(brandInfo.BrandId),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(info);

        var service = new GameCreditSuspSettingMergeCommandService(
            fakeBrandRepository,
            fakeBrandGameCreditSuspSettingRepository);

        await service.ExecuteAsync(command);

        _ = fakeBrandGameCreditSuspSettingRepository
            .Received(0)
            .AddAsync(
            source: Arg.Is(new GameCreditSuspSettingAdd(
                BrandId: brandInfo.BrandId,
                LowRisk: command.LowRisk,
                LowNotification: command.LowNotification,
                ModerateRisk: command.ModerateRisk,
                ModerateNotification: command.ModerateNotification,
                HighRisk: command.HighRisk,
                HighNotification: command.HighNotification,
                ManualNotification: command.ManualNotification)),
            Arg.Any<CancellationToken>());

        _ = fakeBrandGameCreditSuspSettingRepository
            .Received(1)
            .UpdateAsync(
            source: Arg.Is(new GameCreditSuspSettingUpdate(
                BrandId: brandInfo.BrandId,
                LowRisk: command.LowRisk,
                LowNotification: command.LowNotification,
                ModerateRisk: command.ModerateRisk,
                ModerateNotification: command.ModerateNotification,
                HighRisk: command.HighRisk,
                HighNotification: command.HighNotification,
                ManualNotification: command.ManualNotification)),
            Arg.Any<CancellationToken>());
    }
}
