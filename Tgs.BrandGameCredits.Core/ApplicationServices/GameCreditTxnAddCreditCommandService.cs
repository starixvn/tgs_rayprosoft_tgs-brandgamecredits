using NATS.Client.JetStream;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Handler.Common;
using Tgs.SerialNumberGenerator;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditTxnAddCreditCommandService(
    ISerialNumberGenerator serialNumberGenerator,
    IBrandRepository brandRepository,
    IBrandGameCreditTxnRepository brandGameCreditTxnRepository,
    ICommandService<GameCreditTxnPublishCommand> service) : ICommandService<GameCreditTxnAddCreditCommand>
{
    public async ValueTask ExecuteAsync(GameCreditTxnAddCreditCommand command, CancellationToken cancellationToken = default)
    {
        var brandInfo = await brandRepository.GetAsync(
            code: command.BrandCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var id = await serialNumberGenerator.GenerateAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var addCredit = new GameCreditTxnAddCredit(
            Id: id,
            BrandId: brandInfo.BrandId,
            Money: command.Money,
            Amount: command.Amount,
            Remark: command.Remark,
            Expansions: command.Expansions,
            CreatedUserId: command.CreatedUserId,
            CreatedUsername: command.CreatedUsername);

        await brandGameCreditTxnRepository.AddAsync(
            source: addCredit,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var publishCommand = new GameCreditTxnPublishCommand(
            Subject: BrandGameCreditsHandlerNatsConst.Credit,
            ResellerId: brandInfo.ResellerId,
            ResellerCode: brandInfo.ResellerCode,
            LicenseeId: brandInfo.LicenseeId,
            LicenseeCode: brandInfo.LicenseeCode,
            BrandId: brandInfo.BrandId,
            BrandCode: brandInfo.BrandCode);

        await service.ExecuteAsync(
            command: publishCommand,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
