using AdventOfCode.Algorithms;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day10
{
    /// <summary>
    /// Part 1: Locate the start location of the pipe, move around the pipe until we get back to the start and divide the distance by 2.
    /// This gives us the point 'farthest from the starting position'.
    ///
    /// Part 2: Initially thought ray casting was the best way to solve this, but reusing the 'shoelace' algorithm worked, simply deduct
    /// the pipelength from the total area for the internal area only.
    /// </summary>
    [Theory]
    [InlineData("Day10DevelopmentTesting1.txt", 4, 1)]
    [InlineData("Day10DevelopmentTesting2.txt", 4, 0)]
    [InlineData("Day10DevelopmentTesting3.txt", 8, 0)]
    [InlineData("Day10DevelopmentTesting4.txt", 8, 0)]
    [InlineData("Day10DevelopmentTesting5.txt", 23, 4)]
    [InlineData("Day10DevelopmentTesting6.txt", 22, 4)]
    [InlineData("Day10.txt", 6828, 459)]
    public void Day10_Part1_PipeMaze(string filename, int expectedAnswer, int areaInsideLoop)
    {
        // Read the input data and load it into a 2d array.
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToList();

        var map = new char[fileInput.Count, fileInput[0].Length];
        (int X, int Y) startLocation = (0, 0);

        for (var row = 0; row < fileInput.Count; row++)
        {
            for (var col = 0; col < fileInput[row].Length; col++)
            {
                var c = fileInput[row][col];
                map[row, col] = c;

                if (c == 'S') startLocation = (X: col, Y: row);
            }
        }

        // Look around the S start location and remove anything S cannot be.
        var startTileOptions = new List<char> { '7', '|', 'J', '-', 'L', 'F' };

        switch (startLocation.X)
        {
            case 0:
            case > 0 when map[startLocation.Y, startLocation.X - 1] is '.':
                startTileOptions.RemoveAll(z => new[] { '-', 'J', '7' }.Contains(z));
                break;
            case > 0 when map[startLocation.Y, startLocation.X - 1] is '-':
                startTileOptions.RemoveAll(z => new[] { 'F', '|', 'L' }.Contains(z));
                break;
            case > 0 when map[startLocation.Y, startLocation.X + 1] is '-':
                startTileOptions.RemoveAll(z => new[] { 'J', '|', '7' }.Contains(z));
                break;
            case > 0 when map[startLocation.Y, startLocation.X + 1] is '.': //todo merge with above
                startTileOptions.RemoveAll(z => new[] { 'F', '|', 'L', '-' }.Contains(z));
                break;
        }

        switch (startLocation.Y)
        {
            case 0:
            case > 0 when map[startLocation.Y - 1, startLocation.X] is '.':
                startTileOptions.RemoveAll(z => new[] { '|', 'L', 'J' }.Contains(z));
                break;
            case > 0 when map[startLocation.Y - 1, startLocation.X] is '|':
                startTileOptions.RemoveAll(z => new[] { 'F', '-', '7' }.Contains(z));
                break;
            case > 0 when map[startLocation.Y + 1, startLocation.X] is '|':
                startTileOptions.RemoveAll(z => new[] { 'J', '-', 'L' }.Contains(z));
                break;
            case > 0 when map[startLocation.Y + 1, startLocation.X] is '.':
                startTileOptions.RemoveAll(z => new[] { 'F', '-', '7', '|' }.Contains(z));
                break;
        }

        // Replace the map's S with it's actual pipe symbol.
        map[startLocation.Y, startLocation.X] = startTileOptions[0];

        int x = startLocation.X, y = startLocation.Y, pipeLength = 0;
        var fromDirection = GetInitialStartDirection(map[startLocation.Y, startLocation.X]);

        List<ShoelaceFormula.Point> points = [new ShoelaceFormula.Point(x, y)];

        do
        {
            (x, y, fromDirection) = InspectNextTile(x, y, fromDirection);
            points.Add(new ShoelaceFormula.Point(x, y)); // For part 2 only.

            pipeLength++;

            // The next tile is where we started, break.
            if (x == startLocation.X && y == startLocation.Y) break;
        } while (true);

        Assert.Equal(expectedAnswer, pipeLength / 2);

        // Part 2 assertion.
        if (areaInsideLoop > 0)
        {
            var polygonArea = ShoelaceFormula.CalculatePolygonArea(points, pipeLength);

            Assert.Equal(areaInsideLoop, polygonArea - pipeLength);
        }

        return;

        // Returns the starting direction assuming a clockwise progression.
        Direction GetInitialStartDirection(char c) =>
            c switch
            {
                'F' => Direction.Down,
                '-' => Direction.Left,
                '7' => Direction.Down,
                '|' => Direction.Down,
                'J' => Direction.Left,
                'L' => Direction.Up,
                _ => throw new InvalidOperationException()
            };

        // Returns the next x y tile location plus the direction we're coming from.
        (int x, int y, Direction direction) InspectNextTile(int x, int y, Direction direction)
        {
            return map[y, x] switch
            {
                'F' when direction is Direction.Down => new(x + 1, y, Direction.Left),
                'F' when direction is Direction.Right => new(x, y + 1, Direction.Up),
                '-' when direction is Direction.Left => new(x + 1, y, Direction.Left),
                '-' when direction is Direction.Right => new(x - 1, y, Direction.Right),
                '7' when direction is Direction.Left => new(x, y + 1, Direction.Up),
                '7' when direction is Direction.Down => new(x - 1, y, Direction.Right),
                '|' when direction is Direction.Up => new(x, y + 1, Direction.Up),
                '|' when direction is Direction.Down => new(x, y - 1, Direction.Down),
                'J' when direction is Direction.Up => new(x - 1, y, Direction.Right),
                'J' when direction is Direction.Left => new(x, y - 1, Direction.Down),
                'L' when direction is Direction.Up => new(x + 1, y, Direction.Left),
                'L' when direction is Direction.Right => new(x, y - 1, Direction.Down),
                _ => throw new InvalidOperationException()
            };
        }
    }

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}