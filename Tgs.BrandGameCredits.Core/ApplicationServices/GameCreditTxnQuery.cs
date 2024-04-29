using Tgs.BrandGameCredits.Common;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditTxnQuery(
	 string BrandCode,
	 DateTimeOffset StartOn,
	 DateTimeOffset EndOn,
	 BrandGameCreditTxnTypes? Type,
	 int PageNumber = 1,
	 int PageSize = 20);
