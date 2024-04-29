using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Handler.Common;
using Tgs.SerialNumberGenerator;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditTxnAddDebitCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeSerialNumberGenerator = Substitute.For<ISerialNumberGenerator>();
		var fakeBrandRepository = Substitute.For<IBrandRepository>();
		var fakeBrandGameCreditTxnRepository = Substitute.For<IBrandGameCreditTxnRepository>();
		var fakeService = Substitute.For<ICommandService<GameCreditTxnPublishCommand>>();

		var command = new GameCreditTxnAddDebitCommand(
			BrandCode: "GPI",
			100m,
			1000m,
			"text",
			new JsonObject
			{
				["User"] = "test"
			},
			Guid.Parse("50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5"),
			"admin.enabled");

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

		var id = Guid.NewGuid();
		_ = fakeSerialNumberGenerator.GenerateAsync(
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(id);

		var service = new GameCreditTxnAddDebitCommandService(
			fakeSerialNumberGenerator,
			fakeBrandRepository,
			fakeBrandGameCreditTxnRepository,
			fakeService);

		await service.ExecuteAsync(command);

		var addCredit = new GameCreditTxnAddDebit(
			Id: id,
			BrandId: brandInfo.BrandId,
			Money: command.Money,
			Amount: command.Amount,
			Remark: command.Remark,
			Expansions: command.Expansions,
			CreatedUserId: command.CreatedUserId,
			CreatedUsername: command.CreatedUsername);
		_ = fakeBrandGameCreditTxnRepository
			.Received(1)
			.AddAsync(
			source: Arg.Is(addCredit),
			Arg.Any<CancellationToken>());

		var publishCommand = new GameCreditTxnPublishCommand(
			Subject: BrandGameCreditsHandlerNatsConst.Debit,
			ResellerId: brandInfo.ResellerId,
			ResellerCode: brandInfo.ResellerCode,
			LicenseeId: brandInfo.LicenseeId,
			LicenseeCode: brandInfo.LicenseeCode,
			BrandId: brandInfo.BrandId,
			BrandCode: brandInfo.BrandCode);
		_ = fakeService
			 .Received(1)
			.ExecuteAsync(
			command: Arg.Is(publishCommand),
			Arg.Any<CancellationToken>());
	}
}
