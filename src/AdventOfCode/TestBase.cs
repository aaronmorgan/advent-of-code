using Xunit.Abstractions;

namespace AdventOfCode;

public class TestBase
{
    protected static IEnumerable<string> ReadFile(string filename) => File.ReadAllLines($"./TestData/2023/{filename}");
    
    protected readonly ITestOutputHelper output;

    public TestBase(ITestOutputHelper testOutputHelper)
    {
        output = testOutputHelper;
    }
}
