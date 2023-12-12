using Tgs.BrandGameCredits.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditSuspSettingMergeCommandService(
    IBrandRepository brandRepository,
    IBrandGameCreditSuspSettingRepository brandGameCreditSuspSettingRepository) : ICommandService<GameCreditSuspSettingMergeCommand>
{
    public async ValueTask ExecuteAsync(GameCreditSuspSettingMergeCommand command, CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: command.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (await brandGameCreditSuspSettingRepository.GetAsync(
            brandInfo.BrandId,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false) is null)
        {

            var add = new BrandGameCreditSuspSettingAdd(
                BrandId: brandInfo.BrandId,
                LowRisk: command.LowRisk,
                LowNotification: command.LowNotification,
                ModerateRisk: command.ModerateRisk,
                ModerateNotification: command.ModerateNotification,
                HighRisk: command.HighRisk,
                HighNotification: command.HighNotification,
                ManualNotification: command.ManualNotification);

            await brandGameCreditSuspSettingRepository.AddAsync(
                source: add,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
        else
        {
            var update = new BrandGameCreditSuspSettingUpdate(
                BrandId: brandInfo.BrandId,
                LowRisk: command.LowRisk,
                LowNotification: command.LowNotification,
                ModerateRisk: command.ModerateRisk,
                ModerateNotification: command.ModerateNotification,
                HighRisk: command.HighRisk,
                HighNotification: command.HighNotification,
                ManualNotification: command.ManualNotification);

            await brandGameCreditSuspSettingRepository.UpdateAsync(
                source: update,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
