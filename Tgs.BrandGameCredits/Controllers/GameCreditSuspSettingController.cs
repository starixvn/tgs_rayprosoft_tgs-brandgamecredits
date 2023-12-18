using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.ServiceUtility;
using Microsoft.AspNetCore.Http.HttpResults;
using Tgs.BrandGameCredits.Repositories.Entities;

namespace Tgs.BrandGameCredits.Controllers;

[Route("[controller]")]
[ApiController]
public class GameCreditSuspSettingController : ControllerBase
{
    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="brandCode">The brand code.</param>
    /// <returns></returns>
    [HttpGet("{brandCode}")]
    public async ValueTask<Common.Models.BrandGameCreditSuspSetting> GetAsync(
        [FromServices] IGetService<GameCreditSuspSettingGetQuery, GameCreditSuspSettingResult> service,
        string brandCode)
    {
        var result = await service.GetAsync(
            query: new GameCreditSuspSettingGetQuery(
                BrandCode: brandCode),
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new Common.Models.BrandGameCreditSuspSetting(
            ResellerId: result.ResellerId,
            ResellerCode: result.ResellerCode,
            LicenseeId: result.LicenseeId,
            LicenseeCode: result.LicenseeCode,
            BrandId: result.BrandId,
            BrandCode: result.BrandCode,
            LowRisk: result.LowRisk,
            LowNotification: result.LowNotification,
            ModerateRisk: result.ModerateRisk,
            ModerateNotification: result.ModerateNotification,
            HighRisk: result.HighRisk,
            HighNotification: result.HighNotification,
            ManualNotification: result.ManualNotification,
            CreatedOn: result.CreatedOn,
            UpdatedOn: result.UpdatedOn);
    }

    /// <summary>
    /// Adds the credit asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "brandCode": "GPI",
    ///             "lowRisk": 60,
    ///             "lowNotification": {
    ///                 "emailSubject": "[Licensee][Brand] 加值通知 Warning: Balance low",
    ///                 "emailContent": "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you."
    ///             },
    ///             "moderateRisk": 85,
    ///             "moderateNotification": {
    ///                 "emailSubject": "[Licensee][Brand] 緊急加值通知 Warning: Balance too low",
    ///                 "emailContent": "敬愛的夥伴您好，\r\n您的平台目前剩餘額度嚴重不足，所有遊戲暫時不可進入；為避免影響平台正常運營，請立即聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a higher balance usage, and the access to all games will be temporarily suspended. To maintain the stability of your site, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you."
    ///             },
    ///             "highRisk": 95,
    ///             "highNotification": {
    ///                 "emailSubject": "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension",
    ///                 "emailContent": "貴司您好，\r\n由於您的平台剩餘額度已低於限制，平台及所有遊戲暫時不可進入；請立即聯繫貴司對應商務進行加值，並由專人協助您恢復平台運行，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site has been temporarily suspended due to a balance usage that has exceeded the limit. To recover the full access of platform and all games, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you."
    ///             },
    ///             "manualNotification": {
    ///                 "emailSubject": "[Licensee][Brand] 加值通知 Warning: Balance low",
    ///                 "emailContent": "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you."
    ///              }
    ///         }
    /// </remarks>
    [HttpPost("merge")]
    public ValueTask MergeAsync(
        [FromServices] ICommandService<GameCreditSuspSettingMergeCommand> service,
        BrandGameCreditSuspSettingMerge source)
        => service.ExecuteAsync(
            command: new GameCreditSuspSettingMergeCommand(
                BrandCode: source.BrandCode,
                LowRisk: source.LowRisk,
                LowNotification: source.LowNotification,
                ModerateRisk: source.ModerateRisk,
                ModerateNotification: source.ModerateNotification,
                HighRisk: source.HighRisk,
                HighNotification: source.HighNotification,
                ManualNotification: source.ManualNotification),
            cancellationToken: HttpContext.RequestAborted);
}
