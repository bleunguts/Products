using System.Globalization;

namespace Products.API.Utils;

public class Guids
{
    public static string NewGuidString() => Guid.NewGuid().ToString("N", CultureInfo.InvariantCulture);    
}
