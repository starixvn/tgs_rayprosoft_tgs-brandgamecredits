using Tgs.BrandGameCredits.Core;

namespace Tgs.BrandGameCredits.Core;

internal class SystemClock : ISystemClock
{
    public DateTimeOffset CurrentTime => DateTimeOffset.UtcNow;
}
