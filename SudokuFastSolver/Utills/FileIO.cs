
public static class FileIO
{
    public static string ReadFromFile(string? path)
    {
        if (path == null)
        {
            throw new ArgumentNullException("path");
        }
        return File.ReadAllText(path);
    }

    public static void WriteToFile(string path, string content)
    {

        File.WriteAllText(path, content);
        
    }
}