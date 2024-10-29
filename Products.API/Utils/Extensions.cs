namespace Products.API.Utils;

public static class Extensions
{  
    public static string ToGuidString(this Guid guid) => guid.ToString("N");
}