using System.Text.Json.Nodes;

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
}
