using Microsoft.Extensions.Options;
using Tgs.BrandGameCredits.Core;

namespace Tgs.BrandGameCredits.Core;

internal class GameCreditTimeUtnsil : IGameCreditTimeUtnsil
{
    public DateOnly Get1stOfEveryMonthDateOnly(DateTimeOffset currentTime, int timeSpanMinutes = 480)
    {
        var timeSpan = TimeSpan.FromMinutes(timeSpanMinutes);
        var currentTimeOffset = currentTime.ToOffset(timeSpan);
        return DateOnly.FromDateTime(new DateTimeOffset(currentTimeOffset.Year, currentTimeOffset.Month, 1, 0, 0, 0, timeSpan).Date);
    }

    public DateTimeOffset Get1stOfEveryMonthTimeZone(DateTimeOffset currentTime, int timeSpanMinutes = 480)
    {
        var timeSpan = TimeSpan.FromMinutes(timeSpanMinutes);
        var currentTimeOffset = currentTime.ToOffset(timeSpan);
        return new DateTimeOffset(currentTimeOffset.Year, currentTimeOffset.Month, 1, 0, 0, 0, timeSpan);
    }
}
