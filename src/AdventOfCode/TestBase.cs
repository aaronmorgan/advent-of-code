using Xunit.Abstractions;

namespace AdventOfCode;

public class TestBase
{
    protected readonly ITestOutputHelper output;

    public TestBase(ITestOutputHelper testOutputHelper)
    {
        output = testOutputHelper;
    }
}
