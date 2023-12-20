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
            ?? await brandGameCreditSuspSettingRepository.InitAsync(
                brandInfo.BrandId,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameCreditSuspSettingResult(
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
