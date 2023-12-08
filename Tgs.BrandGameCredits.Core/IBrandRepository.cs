using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandRepository
{
    ValueTask<BrandInfo> GetAsync(string code, CancellationToken cancellationToken = default);
    ValueTask<BrandInfo> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
