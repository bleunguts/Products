namespace Products.API.Utils;

public class ShortGuid
{
    public static string NewGuid()
    {
        return GuidToShortGuid(Guid.NewGuid());
    }
    public static string GuidToShortGuid(Guid guid)
    {
        string encoded = Convert.ToBase64String(guid.ToByteArray());
        encoded = encoded.Replace("/", "_").Replace("+", "-");
        return encoded.Substring(0, 22);
    }
}
