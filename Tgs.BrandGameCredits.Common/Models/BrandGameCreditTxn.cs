using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Common.Models;

public record BrandGameCreditTxn(
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
