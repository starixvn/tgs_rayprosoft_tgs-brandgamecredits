using Tgs.BrandGameCredits.Core.Models;

namespace Tgs.BrandGameCredits.Core;

public interface IBrandGameCreditSuspSettingRepository
{
	public ValueTask<GameCreditSuspSettingInfo?> GetAsync(Guid brandId, CancellationToken cancellationToken = default);

	public ValueTask<GameCreditSuspSettingInfo> InitAsync(Guid brandId, CancellationToken cancellationToken = default);

	public ValueTask AddAsync(GameCreditSuspSettingAdd source, CancellationToken cancellationToken = default);

	public ValueTask UpdateAsync(GameCreditSuspSettingUpdate source, CancellationToken cancellationToken = default);
}
