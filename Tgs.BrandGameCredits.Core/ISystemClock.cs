namespace Tgs.BrandGameCredits.Core;

public interface ISystemClock
{
    DateTimeOffset CurrentTime { get; }
}
