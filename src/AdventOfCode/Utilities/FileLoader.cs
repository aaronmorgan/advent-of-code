namespace AdventOfCode.Utilities;

public static class FileLoader
{
    public static IEnumerable<string> ReadFile(string filename) => File.ReadAllLines($"./TestData/{filename}");
}
