namespace AdventOfCode.Utilities;

public static class FileLoader
{
    public static IEnumerable<string> ReadAllLines(string filename) => File.ReadAllLines($"./TestData/{filename}");

    public static string ReadAllText(string filename) => File.ReadAllText($"./TestData/{filename}");
}
