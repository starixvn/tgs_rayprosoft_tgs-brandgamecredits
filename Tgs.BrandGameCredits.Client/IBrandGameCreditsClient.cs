using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Client;

public interface IBrandGameCreditsClient
{
    IAsyncEnumerable<GameCredit> GameCreditQueryAsync(string brandCode, CancellationToken cancellationToken = default);

    ValueTask<GameCredit?> GameCreditGetCurrentAsync(string brandCode, CancellationToken cancellationToken = default);

    ValueTask<GameCreditSuspSetting> GameCreditSuspSettingGetAsync(string brandCode, CancellationToken cancellationToken = default);

    ValueTask GameCreditSuspSettingMergeAsync(string brandCode, decimal lowRisk, JsonNode lowNotification, decimal moderateRisk, JsonNode moderateNotification, decimal highRisk, JsonNode highNotification, JsonNode manualNotification, CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameCreditTxn> GameCreditTxnQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, GameCreditTxnTypes? type, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);

    ValueTask<int> GameCreditTxnQueryCountAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, GameCreditTxnTypes? type, CancellationToken cancellationToken = default);

    ValueTask GameCreditTxnAddCreditAsync(string brandCode, decimal money, decimal amount, string remark, JsonNode expansions, Guid createdUserId, string createdUsername, CancellationToken cancellationToken = default);

    ValueTask GameCreditTxnAddDebitAsync(string brandCode, decimal money, decimal amount, string remark, JsonNode expansions, Guid createdUserId, string createdUsername, CancellationToken cancellationToken = default);
}
