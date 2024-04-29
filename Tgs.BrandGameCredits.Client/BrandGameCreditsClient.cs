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

	public async ValueTask<BrandGameCredit?> GameCreditGetCurrentAsync(string brandCode, CancellationToken cancellationToken = default)
	{
		var path = new Uri(httpClient.BaseAddress!, $"/gamecredit/{brandCode}/current");
		var builder = new UriBuilder(path);
		string url = builder.ToString();

		return await httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<BrandGameCredit?>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.FirstOrDefaultAsync(cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public IAsyncEnumerable<BrandGameCredit> GameCreditHistoriesAsync(string brandCode, CancellationToken cancellationToken = default)
	{
		var path = new Uri(httpClient.BaseAddress!, $"/gamecredit/{brandCode}/histories");
		var builder = new UriBuilder(path);
		string url = builder.ToString();

		return httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandGameCredit>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}

	public IAsyncEnumerable<BrandGameCreditProfit> GameCreditProfitQueryAsync(string brandCode, DateOnly month, CancellationToken cancellationToken = default)
	{
		var path = new Uri(httpClient.BaseAddress!, $"/gamecreditprofit/{brandCode}/query");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["month"] = month.ToShortDateString();
		builder.Query = query.ToString();
		string url = builder.ToString();

		return httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<IEnumerable<BrandGameCreditProfit>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}

	public async ValueTask<BrandGameCreditSuspSetting> GameCreditSuspSettingGetAsync(string brandCode, CancellationToken cancellationToken = default)
	{
		var path = new Uri(httpClient.BaseAddress!, $"/gamecreditsuspsetting/{brandCode}");
		var builder = new UriBuilder(path);
		string url = builder.ToString();

		return await httpClient.GetAsync(url, cancellationToken)
			.ToAsyncEnumerable()
			.Do(response => response.EnsureSuccessStatusCode())
			.Where(response => response.StatusCode == System.Net.HttpStatusCode.OK)
			.SelectMany(response => response.Content
				.ReadFromJsonAsync<BrandGameCreditSuspSetting>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable())
			.FirstOrDefaultAsync(cancellationToken: cancellationToken).ConfigureAwait(false)
			?? throw new NullReferenceException($"This brand does not exist.{brandCode}");
	}

	public async ValueTask GameCreditSuspSettingMergeAsync(string brandCode, decimal lowRisk, BrandGameCreditSuspSettingNotificationMsg lowNotification, decimal moderateRisk, BrandGameCreditSuspSettingNotificationMsg moderateNotification, decimal highRisk, BrandGameCreditSuspSettingNotificationMsg highNotification, BrandGameCreditSuspSettingNotificationMsg manualNotification, CancellationToken cancellationToken = default)
	{
		var add = new BrandGameCreditSuspSettingMerge
		{
			BrandCode = brandCode,
			LowRisk = lowRisk,
			LowNotification = lowNotification,
			ModerateRisk = moderateRisk,
			ModerateNotification = moderateNotification,
			HighRisk = highRisk,
			HighNotification = highNotification,
			ManualNotification = manualNotification
		};
		var path = new Uri(httpClient.BaseAddress!, $"/gamecreditsuspsetting/merge");
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
		var add = new BrandGameCreditTxnAddCredit
		{
			BrandCode = brandCode,
			Money = money,
			Amount = amount,
			Remark = remark,
			Expansions = expansions,
			CreatedUserId = createdUserId,
			CreatedUsername = createdUsername
		};
		var path = new Uri(httpClient.BaseAddress!, $"/gamecredittxn/add/credit");
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
		var add = new BrandGameCreditTxnAddCredit
		{
			BrandCode = brandCode,
			Money = money,
			Amount = amount,
			Remark = remark,
			Expansions = expansions,
			CreatedUserId = createdUserId,
			CreatedUsername = createdUsername
		};
		var path = new Uri(httpClient.BaseAddress!, $"/gamecredittxn/add/debit");
		var builder = new UriBuilder(path);
		string url = builder.ToString();
		var httpContent = new StringContent(JsonSerializer.Serialize(add, _SerializerOptions), Encoding.UTF8, MediaTypeNames.Application.Json);

		_ = await httpClient.PostAsync(url, httpContent, cancellationToken)
			.ToAsyncEnumerable()
		   .Do(response => response.EnsureSuccessStatusCode())
		   .FirstAsync(cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}

	public IAsyncEnumerable<BrandGameCreditTxn> GameCreditTxnQueryAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandGameCreditTxnTypes? type, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
	{
		var path = new Uri(httpClient.BaseAddress!, $"/gamecredittxn/{brandCode}/query");
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
				.ReadFromJsonAsync<IEnumerable<BrandGameCreditTxn>>(_SerializerOptions, cancellationToken)
				.ToAsyncEnumerable()
				.SelectMany(arr => arr!.ToAsyncEnumerable()))
			.Where(info => info is not null)
			.Select(info => info!)
			.ToEnumerable()
			.ToAsyncEnumerable();
	}

	public ValueTask<int> GameCreditTxnQueryCountAsync(string brandCode, DateTimeOffset startOn, DateTimeOffset endOn, BrandGameCreditTxnTypes? type, CancellationToken cancellationToken = default)
	{
		var path = new Uri(httpClient.BaseAddress!, $"/gamecredittxn/{brandCode}/query/count");
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
