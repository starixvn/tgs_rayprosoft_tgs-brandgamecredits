using System.Text.Json;
using System.Text.Json.Nodes;
using Tgs.BrandGameCredits.Common.Models;

namespace Tgs.BrandGameCredits.Core.Expansions;

public static class StringExpansion
{
    public static JsonNode ToJsonNode(this string jsonString)
    {
        try
        {
            return JsonNode.Parse(jsonString)!;
        }
        catch
        {
            return JsonNode.Parse("{}")!;
        }
    }

    public static string ToJsonString(this BrandGameCreditSuspSettingNotificationMsg value)
        => JsonSerializer.Serialize(value);

    public static BrandGameCreditSuspSettingNotificationMsg ToGameCreditSuspSettingNotificationMsg(this string jsonString)
    {
        try
        {
            return JsonSerializer.Deserialize<BrandGameCreditSuspSettingNotificationMsg>(jsonString)
                ?? new BrandGameCreditSuspSettingNotificationMsg();
        }
        catch
        {
            return new BrandGameCreditSuspSettingNotificationMsg();
        }
    }
}
