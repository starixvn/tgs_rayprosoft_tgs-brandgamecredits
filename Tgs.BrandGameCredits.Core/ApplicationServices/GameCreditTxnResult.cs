using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditTxnResult(
	Guid Id,
	Guid ResellerId,
	string ResellerCode,
	Guid LicenseeId,
	string LicenseeCode,
	Guid BrandId,
	string BrandCode,
	BrandGameCreditTxnTypes Type,
	decimal Money,
	decimal Amount,
	string Remark,
	JsonNode Expansions,
	Guid CreatedUserId,
	string CreatedUsername,
	DateTimeOffset CreatedOn);
