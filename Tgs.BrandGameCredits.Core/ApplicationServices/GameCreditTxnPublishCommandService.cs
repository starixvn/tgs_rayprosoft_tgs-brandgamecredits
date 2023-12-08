using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;
using Tgs.BrandGameCredits.Handler.Common.Protos;
using Tgs.ServiceUtility;
using Valhalla.MessageQueue;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

internal class GameCreditTxnPublishCommandService(
    ILogger<GameCreditTxnPublishCommandService> logger,
    ISystemClock systemClock,
    IMessageSender messageSender) : ICommandService<GameCreditTxnPublishCommand>
{
    public ValueTask ExecuteAsync(GameCreditTxnPublishCommand command, CancellationToken cancellationToken = default)
    {
        var currentTime = systemClock.CurrentTime;
        var publish = new BrandGameCreditPublish
        {
            ResellerId = command.ResellerId,
            ResellerCode = command.ResellerCode,
            LicenseeId = command.LicenseeId,
            LicenseeCode = command.LicenseeCode,
            BrandId = command.BrandId,
            BrandCode = command.BrandCode,
            CurrentTime = currentTime.ToTimestamp()
        };

        logger.LogInformation("LogOn:{logOn} Subject:{subject} Message:The content of this publish.({publish})",
            DateTime.Now.ToString("u"),
            command.Subject,
            publish);

        return messageSender.PublishAsync(
            subject: command.Subject,
            data: publish.ToByteArray(),
            cancellationToken: cancellationToken);
    }
}
