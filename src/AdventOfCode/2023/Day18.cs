using AdventOfCode.Algorithms;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day18
{
    [Theory]
    [InlineData("Day18DevelopmentTesting1.txt", 62)]
    [InlineData("Day18.txt", 34329)]
    public void Day18_Part1_LavaductLagoon(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToList();

        int x = 0, y = 0, boundaryLength = 0;

        var points = new ShoelaceFormula.Point[input.Count + 1];
        points[0] = new ShoelaceFormula.Point(0, 0);

        for (var rowIndex = 0; rowIndex < input.Count; rowIndex++)
        {
            var (direction, distance) = input[rowIndex].Split(' ') switch { var str => (str[0], int.Parse(str[1])) };

            boundaryLength += distance;

            switch (direction)
            {
                case "R": { x += distance; break; }
                case "L": { x -= distance; break; }
                case "D": { y += distance; break; }
                case "U": { y -= distance; break; }
            }

            points[rowIndex + 1] = new ShoelaceFormula.Point(x, y);
        }

        points[^1] = new ShoelaceFormula.Point(points[0].X, points[0].Y);

        var polygonArea = ShoelaceFormula.CalculatePolygonArea(points, boundaryLength);

        Assert.Equal(polygonArea, expectedAnswer);
    }

    [Theory]
    [InlineData("Day18DevelopmentTesting1.txt", 952408144115)]
    [InlineData("Day18.txt", 42617947302920)]
    public void Day18_Part2_LavaductLagoon(string filename, long expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToList();

        int x = 0, y = 0, boundaryLength = 0;

        var points = new ShoelaceFormula.Point[input.Count + 1];
        points[0] = new ShoelaceFormula.Point(0, 0);

        for (var rowIndex = 0; rowIndex < input.Count; rowIndex++)
        {
            var instructions = input[rowIndex].Split('#')[1].Trim('#', ')');
            var direction = instructions[^1];
            var distance = Convert.ToInt32(instructions[..5], 16);

            boundaryLength += distance;

            switch (direction)
            {
                case '0': { x += distance; break; }
                case '1': { y += distance; break; }
                case '2': { x -= distance; break; }
                case '3': { y -= distance; break; }
            }

            points[rowIndex + 1] = new ShoelaceFormula.Point(x, y);
        }

        points[^1] = new ShoelaceFormula.Point(points[0].X, points[0].Y);

        var polygonArea = ShoelaceFormula.CalculatePolygonArea(points, boundaryLength);

        Assert.Equal(polygonArea, expectedAnswer);
    }
}
