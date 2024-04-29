using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Handler.Common;
using Tgs.BrandGameCredits.Handler.Common.Protos;
using Valhalla.MessageQueue;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditTxnPublishCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeLogger = NullLogger<GameCreditTxnPublishCommandService>.Instance;
		var fakeTimeProvider = Substitute.For<TimeProvider>();
		var fakeMessageSender = Substitute.For<IMessageSender>();

		var command = new GameCreditTxnPublishCommand(
			Subject: BrandGameCreditsHandlerNatsConst.Debit,
			ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
			ResellerCode: "TGS",
			LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
			LicenseeCode: "TGS",
			BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
			BrandCode: "GPI");

		var currentTime = new DateTimeOffset(2023, 12, 08, 13, 28, 36, TimeSpan.FromHours(+8));
		_ = fakeTimeProvider.GetUtcNow()
			.Returns(currentTime);

		var service = new GameCreditTxnPublishCommandService(
			fakeLogger,
			fakeTimeProvider,
			fakeMessageSender);

		await service.ExecuteAsync(command);

		_ = fakeMessageSender
			.Received(1)
			.PublishAsync(
			subject: Arg.Is(command.Subject),
			data: Arg.Any<BrandGameCreditPublish>(),
			Arg.Any<CancellationToken>());
	}
}
