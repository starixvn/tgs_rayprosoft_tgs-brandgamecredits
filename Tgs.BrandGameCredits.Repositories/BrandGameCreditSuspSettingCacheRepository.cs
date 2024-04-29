using System.Text.Json;
using StackExchange.Redis;
using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandGameCreditSuspSettingCacheRepository(
	IDatabase database,
	IBrandGameCreditSuspSettingRepository repository,
	TimeSpan Expiry) : IBrandGameCreditSuspSettingRepository
{
	public async ValueTask AddAsync(GameCreditSuspSettingAdd source, CancellationToken cancellationToken = default)
	{
		await repository.AddAsync(
			source: source,
			cancellationToken: cancellationToken)
			 .ConfigureAwait(false);

		_ = await DeleteAsync(
			source.BrandId)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameCreditSuspSettingInfo?> GetAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var info = await GetAsync(brandId)
			.ConfigureAwait(false);

		if (info is null)
		{
			info = await repository.GetAsync(
				brandId: brandId,
				cancellationToken: cancellationToken)
				.ConfigureAwait(false);

			if (info is not null)
			{
				_ = await SetAsync(
					source: info)
					.ConfigureAwait(false);
			}
		}

		return info;
	}

	public async ValueTask UpdateAsync(GameCreditSuspSettingUpdate source, CancellationToken cancellationToken = default)
	{
		await repository.UpdateAsync(
			source: source,
			cancellationToken: cancellationToken)
			 .ConfigureAwait(false);

		_ = await DeleteAsync(
			source.BrandId)
			.ConfigureAwait(false);
	}

	public async ValueTask<GameCreditSuspSettingInfo> InitAsync(Guid brandId, CancellationToken cancellationToken = default)
	{
		var source = new GameCreditSuspSettingAdd(
			BrandId: brandId,
			LowRisk: 60,
			LowNotification: new BrandGameCreditSuspSettingNotificationMsg
			{
				EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low",
				EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you."
			},
			ModerateRisk: 80,
			ModerateNotification: new BrandGameCreditSuspSettingNotificationMsg
			{
				EmailSubject = "[Licensee][Brand] 緊急加值通知 Warning: Balance too low",
				EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度嚴重不足，所有遊戲暫時不可進入；為避免影響平台正常運營，請立即聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a higher balance usage, and the access to all games will be temporarily suspended. To maintain the stability of your site, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you."
			},
			HighRisk: 95,
			HighNotification: new BrandGameCreditSuspSettingNotificationMsg
			{
				EmailSubject = "[Licensee][Brand] 平台已停用，請立即加值 Notice of Suspension",
				EmailContent = "貴司您好，\r\n由於您的平台剩餘額度已低於限制，平台及所有遊戲暫時不可進入；請立即聯繫貴司對應商務進行加值，並由專人協助您恢復平台運行，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site has been temporarily suspended due to a balance usage that has exceeded the limit. To recover the full access of platform and all games, we kindly recommend reaching out to your designated business contact for a top-up immediately. Thank you."
			},
			ManualNotification: new BrandGameCreditSuspSettingNotificationMsg
			{
				EmailSubject = "[Licensee][Brand] 加值通知 Warning: Balance low",
				EmailContent = "敬愛的夥伴您好，\r\n您的平台目前剩餘額度不足，為避免影響會員遊戲體驗，請盡速聯繫貴司對應商務進行加值，謝謝您。\r\nDear Partner,\r\nWe wanted to inform you that your site currently has reached a high balance usage. To maintain the stability and quality of your gaming experience, we kindly recommend reaching out to your designated business contact for a top-up. Thank you."
			});

		await AddAsync(
			source,
			cancellationToken)
			.ConfigureAwait(false);

		return await GetAsync(
			brandId,
			cancellationToken)
			.ConfigureAwait(false)
			?? throw new Exception("Failed to initialize data for this brand.({brandId})");
	}

	private static string GetKey(Guid brandId)
		=> string.Concat("brandgamecredit:suspsetting:", brandId);

	private async ValueTask<GameCreditSuspSettingInfo?> GetAsync(Guid brandId)
	{
		var key = GetKey(brandId);
		if (await database.KeyExistsAsync(key).ConfigureAwait(false))
		{
			var value = await database.StringGetAsync(
				key: key)
				.ConfigureAwait(false);

			if (!value.IsNullOrEmpty)
			{
				return JsonSerializer.Deserialize<GameCreditSuspSettingInfo>(value.ToString());
			}
		}

		return null;
	}

	private async ValueTask<bool> SetAsync(GameCreditSuspSettingInfo source)
		=> await database.StringSetAsync(
			key: GetKey(source.BrandId),
			value: JsonSerializer.Serialize(source),
			expiry: Expiry)
		.ConfigureAwait(false);

	private async ValueTask<bool> DeleteAsync(Guid brandId)
		=> await database.KeyDeleteAsync(
			key: GetKey(brandId),
			flags: CommandFlags.FireAndForget)
		.ConfigureAwait(false);
}
