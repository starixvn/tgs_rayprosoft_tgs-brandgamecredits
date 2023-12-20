using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core.Expansions;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditSuspSettingGetService(
    IBrandRepository brandRepository,
    IBrandGameCreditSuspSettingRepository brandGameCreditSuspSettingRepository) : IGetService<GameCreditSuspSettingGetQuery, GameCreditSuspSettingResult>
{
    public async ValueTask<GameCreditSuspSettingResult> GetAsync(GameCreditSuspSettingGetQuery query, CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: query.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var info = await brandGameCreditSuspSettingRepository.GetAsync(
            brandInfo.BrandId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false)
            ?? await brandGameCreditSuspSettingRepository.GetAsync(
                Guid.Empty,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return info is null
            ? new GameCreditSuspSettingResult(
                ResellerId: brandInfo.ResellerId,
                ResellerCode: brandInfo.ResellerCode,
                LicenseeId: brandInfo.LicenseeId,
                LicenseeCode: brandInfo.LicenseeCode,
                BrandId: brandInfo.BrandId,
                BrandCode: brandInfo.BrandCode,
                LowRisk: 60,
                LowNotification: new BrandGameCreditSuspSettingNotificationMsg
                {
                    EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low",
                    EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you."
                },
                ModerateRisk: 80,
                ModerateNotification: new BrandGameCreditSuspSettingNotificationMsg
                {
                    EmailSubject = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low",
                    EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度嚴重不足，所有遊戲暫時不可進入；為避免影響平台正常運營，請立即聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a higher balance usage, and the access to all games will be temporarily suspended. To maintain the stability of your site, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you."
                },
                HighRisk: 95,
                HighNotification: new BrandGameCreditSuspSettingNotificationMsg
                {
                    EmailSubject = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension",
                    EmailContent = "貴司您好，\r\n由於您的平台剩餘額度已低於限制，平台及所有遊戲暫時不可進入；請立即聯繫貴司對應商務進行加值，並由專人協助您恢復平台運行，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site has been temporarily suspended due to a balance usage that has exceeded the limit. To recover the full access of platform and all games, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you."
                },
                ManualNotification: new BrandGameCreditSuspSettingNotificationMsg
                {
                    EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low",
                    EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you."
                },
                CreatedOn: DateTimeOffset.UtcNow,
                UpdatedOn: DateTimeOffset.UtcNow)
            : new GameCreditSuspSettingResult(
                ResellerId: brandInfo.ResellerId,
                ResellerCode: brandInfo.ResellerCode,
                LicenseeId: brandInfo.LicenseeId,
                LicenseeCode: brandInfo.LicenseeCode,
                BrandId: brandInfo.BrandId,
                BrandCode: brandInfo.BrandCode,
                LowRisk: info.LowRisk,
                LowNotification: info.LowNotification.ToGameCreditSuspSettingNotificationMsg(),
                ModerateRisk: info.ModerateRisk,
                ModerateNotification: info.ModerateNotification.ToGameCreditSuspSettingNotificationMsg(),
                HighRisk: info.HighRisk,
                HighNotification: info.HighNotification.ToGameCreditSuspSettingNotificationMsg(),
                ManualNotification: info.ManualNotification.ToGameCreditSuspSettingNotificationMsg(),
                CreatedOn: info.CreatedOn,
                UpdatedOn: info.UpdatedOn);
    }
}
