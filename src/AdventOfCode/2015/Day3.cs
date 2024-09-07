using AdventOfCode.Types;
using AdventOfCode.Utilities;

namespace AdventOfCode._2015;

public class Day3
{
    [Theory]
    [InlineData(">", 2)]
    [InlineData("^>v<", 4)]
    [InlineData("^v^v^v^v^v", 2)]
    [InlineData("Day3.txt", 2572)]
    public void Day3_Part1_PerfectlySphericalHousesInAVacuum(string input, int expectedAnswer)
    {
        var directions = input.StartsWith("Day3")
            ? FileLoader.ReadAllText("2015/" + input).ToCharArray()
            : input.ToCharArray();

        int x = 0, y = 0;
        var points = new Dictionary<int, int> { { new Point(x, y).GetHashCode(), 1 } };

        foreach (var c in directions)
        {
            switch (c)
            {
                case '>':
                {
                    TryAddPoint(points, x += 1, y);

                    break;
                }
                case 'v':
                {
                    TryAddPoint(points, x, y += 1);

                    break;
                }
                case '<':
                {
                    TryAddPoint(points, x -= 1, y);

                    break;
                }
                case '^':
                {
                    TryAddPoint(points, x, y -= 1);

                    break;
                }
                default: throw new Exception();
            }
        }

        Assert.Equal(expectedAnswer, points.Count);
    }

    private static void TryAddPoint(Dictionary<int, int> points, int x, int y)
    {
        var p = new Point(x, y).GetHashCode();
        if (!points.TryAdd(p, 1))
        {
            points[p] += 1;
        }
    }
}