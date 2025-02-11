
public static class FileIO
{
    public static string ReadFromFile(string path)
    {
        return File.ReadAllText(path);
    }

    public static void WriteToFile(string path, string content)
    {

        File.WriteAllText(path, content);
        
    }
}