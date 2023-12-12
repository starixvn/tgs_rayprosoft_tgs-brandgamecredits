using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Web;
using Tgs.BrandGameCredits.Common;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Client;

internal class BrandGameCreditsClient(HttpClient httpClient) : IBrandGameCreditsClient
{
    private static readonly JsonSerializerOptions _SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public async ValueTask<GameCredit?> GameCreditGetCurrentAsync(string brandCode, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecredit/{brandCode}/current");
        var builder = new UriBuilder(path);
        string url = builder.ToString();

        return await httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<GameCredit?>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }

    public IAsyncEnumerable<GameCredit> GameCreditQueryAsync(string brandCode, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecredit/{brandCode}/query");
        var builder = new UriBuilder(path);
        string url = builder.ToString();

        return httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<IEnumerable<GameCredit>>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable()
                .SelectMany(arr => arr!.ToAsyncEnumerable()))
            .Where(info => info is not null)
            .Select(info => info!)
            .ToEnumerable()
            .ToAsyncEnumerable();
    }

    public async ValueTask<GameCreditSuspSetting> GameCreditSuspSettingGetAsync(string brandCode, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecreditsuspsetting/{brandCode}");
        var builder = new UriBuilder(path);
        string url = builder.ToString();

        return await httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<GameCreditSuspSetting>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new NullReferenceException($"This brand does not exist.{brandCode}");
    }
    public async ValueTask GameCreditSuspSettingMergeAsync(string brandCode, decimal lowRisk, JsonNode lowNotification, decimal moderateRisk, JsonNode moderateNotification, decimal highRisk, JsonNode highNotification, JsonNode manualNotification, CancellationToken cancellationToken = default)
    {
        var add = new GameCreditSuspSettingMerge
        {
            BrandCode = brandCode,
            LowRisk = lowRisk,
            LowNotification = lowNotification,
            ModerateRisk = moderateRisk,
            ManualNotification = manualNotification,
            HighRisk = highRisk,
            HighNotification = highNotification,
            ModerateNotification = moderateNotification
        };
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecreditsuspsetting/merge");
        var builder = new UriBuilder(path);
        string url = builder.ToString();
        var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

        _ = await httpClient.PostAsync(url, httpContent, cancellationToken)
            .ToAsyncEnumerable()
           .Do(response => response.EnsureSuccessStatusCode())
           .FirstAsync(cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public async ValueTask GameCreditTxnAddCreditAsync(string brandCode, decimal money, decimal amount, string remark, JsonNode expansions, Guid createdUserId, string createdUsername, CancellationToken cancellationToken = default)
    {
        var add = new GameCreditTxnAddCredit
        {
            BrandCode = brandCode,
            Money = money,
            Amount = amount,
            Remark = remark,
            Expansions = expansions,
            CreatedUserId = createdUserId,
            CreatedUsername = createdUsername
        };
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecredittxn/add/credit");
        var builder = new UriBuilder(path);
        string url = builder.ToString();
        var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

        _ = await httpClient.PostAsync(url, httpContent, cancellationToken)
            .ToAsyncEnumerable()
           .Do(response => response.EnsureSuccessStatusCode())
           .FirstAsync(cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public async ValueTask GameCreditTxnAddDebitAsync(string brandCode, decimal money, decimal amount, string remark, JsonNode expansions, Guid createdUserId, string createdUsername, CancellationToken cancellationToken = default)
    {
        var add = new GameCreditTxnAddCredit
        {
            BrandCode = brandCode,
            Money = money,
            Amount = amount,
            Remark = remark,
            Expansions = expansions,
            CreatedUserId = createdUserId,
            CreatedUsername = createdUsername
        };
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecredittxn/add/debit");
        var builder = new UriBuilder(path);
        string url = builder.ToString();
        var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

        _ = await httpClient.PostAsync(url, httpContent, cancellationToken)
            .ToAsyncEnumerable()
           .Do(response => response.EnsureSuccessStatusCode())
           .FirstAsync(cancellationToken: cancellationToken)
           .ConfigureAwait(false);
    }

    public IAsyncEnumerable<GameCreditTxn> GameCreditTxnQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, GameCreditTxnTypes? type, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecredittxn/{brandCode}/query");
        var builder = new UriBuilder(path);
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["startOn"] = startOn.ToString("u");
        query["endOn"] = endOn.ToString("u");
        query["pageNumber"] = pageNumber.ToString();
        query["pageSize"] = pageSize.ToString();
        if (type.HasValue)
        {
            query["type"] = type.ToString();
        }
        builder.Query = query.ToString();
        string url = builder.ToString();

        return httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<IEnumerable<GameCreditTxn>>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable()
                .SelectMany(arr => arr!.ToAsyncEnumerable()))
            .Where(info => info is not null)
            .Select(info => info!)
            .ToEnumerable()
            .ToAsyncEnumerable();
    }

    public ValueTask<int> GameCreditTxnQueryCountAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, GameCreditTxnTypes? type, CancellationToken cancellationToken = default)
    {
        var path = new Uri(httpClient.BaseAddress!, $"/api/gamecredittxn/{brandCode}/query/count");
        var builder = new UriBuilder(path);
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["startOn"] = startOn.ToString("u");
        query["endOn"] = endOn.ToString("u");
        if (type.HasValue)
        {
            query["type"] = type.ToString();
        }
        builder.Query = query.ToString();
        string url = builder.ToString();

        return httpClient.GetAsync(url, cancellationToken)
            .ToAsyncEnumerable()
            .Do(response => response.EnsureSuccessStatusCode())
            .SelectMany(response => response.Content
                .ReadFromJsonAsync<int>(_SerializerOptions, cancellationToken)
                .ToAsyncEnumerable())
            .FirstAsync(cancellationToken: cancellationToken);
    }
}
