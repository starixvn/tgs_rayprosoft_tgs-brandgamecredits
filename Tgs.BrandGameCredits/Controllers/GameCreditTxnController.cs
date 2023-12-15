using Microsoft.AspNetCore.Mvc;
using Tgs.BrandGameCredits.Common;

using Tgs.BrandGameCredits.Common.Models;
using Tgs.BrandGameCredits.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.BrandGameCredits.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameCreditTxnController : ControllerBase
{
    /// <summary>
    /// Queries the asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="brandCode">The brand code.</param>
    /// <param name="startOn">The start on.</param>
    /// <param name="endOn">The end on.</param>
    /// <param name="type">The type.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns></returns>
    [HttpGet("{brandCode}/query")]
    public async IAsyncEnumerable<BrandGameCreditTxn> QueryAsync(
        [FromServices] IQueryService<GameCreditTxnQuery, GameCreditTxnResult> service,
        string brandCode,
        [FromQuery] DateTimeOffset startOn,
        [FromQuery] DateTimeOffset endOn,
        [FromQuery] BrandGameCreditTxnTypes? type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20)
    {
        var results = await service.QueryAsync(
            query: new GameCreditTxnQuery(
                BrandCode: brandCode,
                StartOn: startOn,
                EndOn: endOn,
                Type: type,
                PageNumber: pageNumber,
                PageSize: pageSize),
             cancellationToken: HttpContext.RequestAborted)
             .ToArrayAsync(
            cancellationToken: HttpContext.RequestAborted)
             .ConfigureAwait(false)
             ?? Enumerable.Empty<GameCreditTxnResult>();

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new BrandGameCreditTxn(
                result.Id,
                result.ResellerId,
                result.ResellerCode,
                result.LicenseeId,
                result.LicenseeCode,
                result.BrandId,
                result.BrandCode,
                result.Type,
                result.Money,
                result.Amount,
                result.Remark,
                result.Expansions,
                result.CreatedUserId,
                result.CreatedUsername,
                result.CreatedOn);
        }
    }

    /// <summary>
    /// Queries the count asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="brandCode">The brand code.</param>
    /// <param name="startOn">The start on.</param>
    /// <param name="endOn">The end on.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    [HttpGet("{brandCode}/query/count")]
    public ValueTask<int> QueryCountAsync(
        [FromServices] IGetService<GameCreditTxnQueryCountQuery, int> service,
        string brandCode,
        [FromQuery] DateTimeOffset startOn,
        [FromQuery] DateTimeOffset endOn,
        [FromQuery] BrandGameCreditTxnTypes? type)
        => service.GetAsync(
            query: new GameCreditTxnQueryCountQuery(
                BrandCode: brandCode,
                StartOn: startOn,
                EndOn: endOn,
                Type: type),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Adds the credit asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "brandCode": "GPI",
    ///             "money": 10,
    ///             "amount": 1000,
    ///             "remark": "test",
    ///             "expansions": {
    ///                 "Name": "admin.enabled"
    ///             },
    ///             "createdUserId": "50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5",
    ///             "createdUsername": "admin.enabled"
    ///         }
    /// </remarks>
    [HttpPost("add/credit")]
    public ValueTask AddCreditAsync(
        [FromServices] ICommandService<GameCreditTxnAddCreditCommand> service,
        BrandGameCreditTxnAddCredit source)
            => service.ExecuteAsync(
                command: new GameCreditTxnAddCreditCommand(
                    BrandCode: source.BrandCode,
                    Money: source.Money,
                    Amount: source.Amount,
                    Remark: source.Remark,
                    Expansions: source.Expansions,
                    CreatedUserId: source.CreatedUserId,
                    CreatedUsername: source.CreatedUsername),
                cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Adds the debit asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///         {
    ///             "brandCode": "GPI",
    ///             "money": 10,
    ///             "amount": 1000,
    ///             "remark": "test",
    ///             "expansions": {
    ///                 "Name": "admin.enabled"
    ///             },
    ///             "createdUserId": "50ce61eb-8c9d-4f84-3cf6-08d9262ce1a5",
    ///             "createdUsername": "admin.enabled"
    ///         }
    /// </remarks>
    [HttpPost("add/debit")]
    public ValueTask AddDebitAsync(
        [FromServices] ICommandService<GameCreditTxnAddDebitCommand> service,
        BrandGameCreditTxnAddDebit source)
        => service.ExecuteAsync(
            command: new GameCreditTxnAddDebitCommand(
                BrandCode: source.BrandCode,
                Money: source.Money,
                Amount: source.Amount,
                Remark: source.Remark,
                Expansions: source.Expansions,
                CreatedUserId: source.CreatedUserId,
                CreatedUsername: source.CreatedUsername),
            cancellationToken: HttpContext.RequestAborted);
}
