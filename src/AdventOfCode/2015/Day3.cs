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
                case '>': x += 1; break;
                case 'v': y += 1; break;
                case '<': x -= 1; break;
                case '^': y -= 1; break;
            }

            TryAddPoint(points, x, y);
        }

        Assert.Equal(expectedAnswer, points.Count);
    }

    [Theory]
    [InlineData("^v", 3)]
    [InlineData("^>v<", 3)]
    [InlineData("^v^v^v^v^v", 11)]
    [InlineData("Day3.txt", 2631)]
    public void Day3_Part2_PerfectlySphericalHousesInAVacuum(string input, int expectedAnswer)
    {
        var directions = input.StartsWith("Day3")
            ? FileLoader.ReadAllText("2015/" + input).ToCharArray()
            : input.ToCharArray();

        int santaX = 0, santaY = 0, roboSantaX = 0, roboSantaY = 0;
        var points = new Dictionary<int, int> { { new Point(santaX, santaY).GetHashCode(), 1 } };

        var santasTurn = true;

        foreach (var c in directions)
        {
            int dx = 0, dy = 0;

            switch (c)
            {
                case '>': dx = 1; break;
                case 'v': dy = 1; break;
                case '<': dx = -1; break;
                case '^': dy = -1; break;
            }

            if (santasTurn)
            {
                santaX += dx;
                santaY += dy;
                TryAddPoint(points, santaX, santaY);
            }
            else
            {
                roboSantaX += dx;
                roboSantaY += dy;
                TryAddPoint(points, roboSantaX, roboSantaY);
            }

            santasTurn = !santasTurn;
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