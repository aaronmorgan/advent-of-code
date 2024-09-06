using AdventOfCode.Utilities;

namespace AdventOfCode._2015;

public class Day2
{
    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 58)]
    [InlineData("Day2DevelopmentTesting2.txt", 43)]
    [InlineData("Day2.txt", 1586300)]
    public void Day2_Part1_IWasToldThereWouldBeNoMath(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2015/" + filename).ToArray();

        var sides = input
            .Select(x => x.Split('x').Select(int.Parse).ToArray())
            .Select(x => new Side(x[0], x[1], x[2])).ToList();

        var paperLength = 0;

        foreach (var a in sides)
        {
            var side1 = a.Length * a.Width;
            var side2 = a.Width * a.Height;
            var side3 = a.Height * a.Length;

            paperLength += 2 * (side1 + side2 + side3) + Math.Min(Math.Min(side1, side2), side3);
        }

        Assert.Equal(expectedAnswer, paperLength);
    }

    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 34)]
    [InlineData("Day2DevelopmentTesting2.txt", 14)]
    [InlineData("Day2.txt", 3737498)]
    public void Day2_Part2_IWasToldThereWouldBeNoMath(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2015/" + filename).ToArray();

        var sides = input
            .Select(x => x.Split('x').Select(int.Parse).ToArray())
            .Select(x => new Side(x[0], x[1], x[2])).ToList();

        var ribbonLength = 0;

        foreach (var a in sides)
        {
            var side1 = 2 * (a.Length + a.Width);
            var side2 = 2 * (a.Width + a.Height);
            var side3 = 2 * (a.Height + a.Length);

            ribbonLength += a.Length * a.Width * a.Height + Math.Min(Math.Min(side1, side2), side3);
        }

        Assert.Equal(expectedAnswer, ribbonLength);
    }

    private record Side(int Length, int Width, int Height);
}