using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IComputingUtnsil
{
    CreditCalculationInfo CreditCalculation(decimal initialAmount, decimal creditAmount, decimal debitAmount, decimal profitAmount);
}
