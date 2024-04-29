using NSubstitute;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core.UnitTest.ApplicationServices;

public class GameCreditTxnQueryCountGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeBrandRepository = Substitute.For<IBrandRepository>();
		var fakeBrandGameCreditRepository = Substitute.For<IBrandGameCreditTxnRepository>();

		var query = new GameCreditTxnQueryCountQuery(
			BrandCode: "GPI",
			StartOn: new DateTimeOffset(2023, 12, 8, 00, 00, 00, TimeSpan.FromHours(8)),
			EndOn: new DateTimeOffset(2023, 12, 9, 00, 00, 00, TimeSpan.FromHours(8)),
			Type: BrandGameCreditTxnTypes.Credit);

		var brandInfo = new BrandInfo(
			ResellerId: Guid.Parse("b5a2c708-ab70-4316-9f4d-5d2ad0e77460"),
			ResellerCode: "TGS",
			LicenseeId: Guid.Parse("66c5fef5-217b-4929-9a08-72c2479a8f4e"),
			LicenseeCode: "TGS",
			BrandId: Guid.Parse("4e86719f-243f-46f6-b010-5c758b7285af"),
			BrandCode: "GPI",
			TimeSpanMinutes: 480);
		_ = fakeBrandRepository.GetAsync(
			Arg.Is(query.BrandCode),
			Arg.Any<CancellationToken>())
			.Returns(brandInfo);

		var count = 1;
		_ = fakeBrandGameCreditRepository.QueryCountAsync(
			brandId: Arg.Is(brandInfo.BrandId),
			type: Arg.Is(query.Type),
			startOn: Arg.Is(query.StartOn),
			endOn: Arg.Is(query.EndOn),
			Arg.Any<CancellationToken>())
			.Returns(count);

		var service = new GameCreditTxnQueryCountGetService(
			fakeBrandRepository,
			fakeBrandGameCreditRepository);

		var actual = await service.GetAsync(query: query);

		Assert.Equal(1, actual);
	}
}
