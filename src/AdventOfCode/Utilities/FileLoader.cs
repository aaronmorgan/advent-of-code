namespace AdventOfCode.Utilities;

public static class FileLoader
{
    [Obsolete("Use ReadAllLines instead")]
    public static IEnumerable<string> ReadFile(string filename) => File.ReadAllLines($"./TestData/{filename}");

    public static IEnumerable<string> ReadAllLines(string filename) => File.ReadAllLines($"./TestData/{filename}");

    public static string ReadAllText(string filename) => File.ReadAllText($"./TestData/{filename}");
}
