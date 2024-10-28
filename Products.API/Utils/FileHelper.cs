namespace Products.API.Utils;

public static class FileHelper
{
    public static void ForceDelete(string jsonFilename)
    {
        if (File.Exists(jsonFilename))
        {
            try
            {
                File.Delete(jsonFilename);
            }
            finally
            {
            }
        }
    }
}
