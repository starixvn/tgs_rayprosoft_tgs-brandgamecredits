using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Tgs.BrandGameCredits.Common.Models;

public record BrandGameCreditTxnAddDebit
{
    /// <summary>
    /// The brand code
    /// </summary>
    [Required]
    public string BrandCode { get; init; } = default!;

    /// <summary>
    /// The money
    /// </summary>
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Money { get; init; }

    /// <summary>
    /// The amount
    /// </summary>
    [Required]
    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Amount { get; init; }

    /// <summary>
    /// The remark
    /// </summary>
    public string Remark { get; init; } = default!;

    /// <summary>
    /// The expansions
    /// </summary>
    public JsonNode Expansions { get; init; } = default!;

    /// <summary>
    /// The created user identifier
    /// </summary>
    [Required]
    public Guid CreatedUserId { get; init; } = default!;

    /// <summary>
    /// The created username
    /// </summary>
    [Required]
    public string CreatedUsername { get; init; } = default!;
}
