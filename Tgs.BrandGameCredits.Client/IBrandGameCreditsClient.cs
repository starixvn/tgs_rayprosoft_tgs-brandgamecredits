using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Client;

public interface IBrandGameCreditsClient
{
    IAsyncEnumerable<BrandGameCredit> GameCreditQueryAsync(string brandCode, CancellationToken cancellationToken = default);

    ValueTask<BrandGameCredit?> GameCreditGetCurrentAsync(string brandCode, CancellationToken cancellationToken = default);

    ValueTask<BrandGameCreditSuspSetting> GameCreditSuspSettingGetAsync(string brandCode, CancellationToken cancellationToken = default);

    ValueTask GameCreditSuspSettingMergeAsync(string brandCode, decimal lowRisk, BrandGameCreditSuspSettingNotificationMsg lowNotification, decimal moderateRisk, BrandGameCreditSuspSettingNotificationMsg moderateNotification, decimal highRisk, BrandGameCreditSuspSettingNotificationMsg highNotification, BrandGameCreditSuspSettingNotificationMsg manualNotification, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BrandGameCreditTxn> GameCreditTxnQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandGameCreditTxnTypes? type, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default);

    ValueTask<int> GameCreditTxnQueryCountAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandGameCreditTxnTypes? type, CancellationToken cancellationToken = default);

    ValueTask GameCreditTxnAddCreditAsync(string brandCode, decimal money, decimal amount, string remark, JsonNode expansions, Guid createdUserId, string createdUsername, CancellationToken cancellationToken = default);

    ValueTask GameCreditTxnAddDebitAsync(string brandCode, decimal money, decimal amount, string remark, JsonNode expansions, Guid createdUserId, string createdUsername, CancellationToken cancellationToken = default);
}
