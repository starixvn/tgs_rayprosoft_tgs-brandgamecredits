using Microsoft.EntityFrameworkCore;
using Tgs.BrandGameCredits.Core;
using Tgs.BrandGameCredits.Core.Models;
using Tgs.BrandGameCredits.Entities;

namespace Tgs.BrandGameCredits.Repositories;

internal class BrandRepository(TgsContext context) : IBrandRepository
{
	public async ValueTask<BrandInfo> GetAsync(string code, CancellationToken cancellationToken = default)
		=> await context.Brands.AsQueryable()
		.Where(b => b.Code == code)
		.Join(
		   context.Licensees,
		   inner => inner.LicenseeId,
		   outer => outer.Id,
		   (brand, licensee) => new
		   {
			   licensee.ResellerId,
			   LicenseeId = licensee.Id,
			   LicenseeCode = licensee.Code,
			   BrandId = brand.Id,
			   BrandCode = brand.Code,
			   brand.TimeSpanMinutes,
		   })
		.Join(
			context.Resellers,
			inner => inner.ResellerId,
			outer => outer.Id,
			(combined, reseller) => new
			{
				ResellerId = reseller.Id,
				ResellerCode = reseller.Code,
				combined.LicenseeId,
				combined.LicenseeCode,
				combined.BrandId,
				combined.BrandCode,
				combined.TimeSpanMinutes
			})
		.Select(selector: selector => new BrandInfo(
			selector.ResellerId,
			selector.ResellerCode,
			selector.LicenseeId,
			selector.LicenseeCode,
			selector.BrandId,
			selector.BrandCode,
			selector.TimeSpanMinutes.GetValueOrDefault(480)
			))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false)
		 ?? throw new NullReferenceException($"This brand does not exist.{code}");

	public async ValueTask<BrandInfo> GetAsync(Guid id, CancellationToken cancellationToken = default)
		=> await context.Brands.AsQueryable()
		.Where(predicate => predicate.Id == id)
		.Join(
		   context.Licensees,
		   inner => inner.LicenseeId,
		   outer => outer.Id,
		   (brand, licensee) => new
		   {
			   licensee.ResellerId,
			   LicenseeId = licensee.Id,
			   LicenseeCode = licensee.Code,
			   BrandId = brand.Id,
			   BrandCode = brand.Code,
			   brand.TimeSpanMinutes,
		   })
		.Join(
			context.Resellers,
			inner => inner.ResellerId,
			outer => outer.Id,
			(combined, reseller) => new
			{
				ResellerId = reseller.Id,
				ResellerCode = reseller.Code,
				combined.LicenseeId,
				combined.LicenseeCode,
				combined.BrandId,
				combined.BrandCode,
				combined.TimeSpanMinutes
			})
		.Select(selector: selector => new BrandInfo(
			selector.ResellerId,
			selector.ResellerCode,
			selector.LicenseeId,
			selector.LicenseeCode,
			selector.BrandId,
			selector.BrandCode,
			selector.TimeSpanMinutes.GetValueOrDefault(480)
			))
		.FirstOrDefaultAsync(
			cancellationToken: cancellationToken)
		.ConfigureAwait(false)
		 ?? throw new NullReferenceException($"This brand does not exist.{id}");
}
