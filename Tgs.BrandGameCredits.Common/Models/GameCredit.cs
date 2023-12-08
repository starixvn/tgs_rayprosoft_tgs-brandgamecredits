using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tgs.BrandGameCredits.Common.Models;

public record GameCredit(
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
    DateTimeOffset CreatedOn,
    DateTimeOffset UpdatedOn);