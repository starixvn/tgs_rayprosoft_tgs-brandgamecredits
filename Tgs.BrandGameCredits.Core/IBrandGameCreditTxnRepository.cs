using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandGameCreditTxnRepository
{
    public ValueTask AddAsync(GameCreditTxnAddCredit source, CancellationToken cancellationToken = default);
    public ValueTask AddAsync(GameCreditTxnAddDebit source, CancellationToken cancellationToken = default);
    public IAsyncEnumerable<GameCreditTxnInfo> QueryAsync(Guid brandId, BrandGameCreditTxnTypes? type, DateTimeOffset startOn, DateTimeOffset endOn, int offset = 0, int limit = 20, CancellationToken cancellationToken = default);

    public ValueTask<int> QueryCountAsync(Guid brandId, BrandGameCreditTxnTypes? type, DateTimeOffset startOn, DateTimeOffset endOn, CancellationToken cancellationToken = default);
}
