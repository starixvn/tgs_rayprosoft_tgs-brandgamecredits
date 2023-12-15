using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgs.BrandGameCredits.Common;

namespace Tgs.BrandGameCredits.Core.ApplicationServices;

public record GameCreditResult(
    Guid ResellerId,
    string ResellerCode,
    Guid LicenseeId,
    string LicenseeCode,
    Guid BrandId,
    string BrandCode,
    DateOnly Month,
    decimal InitialAmount,
    decimal CreditAmount,
    decimal DebitAmount,
    decimal ProfitAmount,
    decimal CreditLimit,
    decimal CreditUsed,
    decimal CreditUsedPer,
    decimal CreditAvailable,
    decimal CreditAvailablePer,
    BrandGameCreditRiskTypes CreditRiskType,
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);