using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day10
{
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    /// <summary>
    /// Locate the start location of the pipe, move around the pipe until we get back to the start and divide the distance by 2.
    /// This gives us the point 'farthest from the starting position'.
    /// </summary>
    [Theory]
    [InlineData("Day10DevelopmentTesting1.txt", 4)]
    [InlineData("Day10DevelopmentTesting2.txt", 4)]
    [InlineData("Day10DevelopmentTesting3.txt", 8)]
    [InlineData("Day10DevelopmentTesting4.txt", 8)]
    [InlineData("Day10.txt", 6828)]
    public void Day10_Part1_PipeMaze(string filename, int expectedAnswer)
    {
        // Read the input data and load it into a 2d array.
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToList();

        var map = new char[fileInput[0].Length, fileInput.Count];
        (int X, int Y) startLocation = (0, 0);

        for (var i = 0; i < fileInput.Count; i++)
        {
            for (var j = 0; j < fileInput[i].Length; j++)
            {
                var c = fileInput[i][j];
                map[i, j] = c;

                if (c == 'S') startLocation = (X: j, Y: i);
            }
        }

        // Determine exit nodes from the S starting point.
        var x = startLocation.X;
        var y = startLocation.Y;

        // Look around the S start location and remove anything S cannot be.
        var startTileOptions = new List<char> { '7', '|', 'J', '-', 'L', 'F' };

        switch (x)
        {
            case 0:
            case > 0 when map[y, x - 1] is '.':
                startTileOptions.RemoveAll(z => new[] { '-', 'J', '7' }.Contains(z));
                break;
            case > 0 when map[y, x - 1] is '-':
                startTileOptions.RemoveAll(z => new[] { 'F', '|', 'L' }.Contains(z));
                break;
            case > 0 when map[y, x + 1] is '-':
                startTileOptions.RemoveAll(z => new[] { 'J', '|', '7' }.Contains(z));
                break;
            case > 0 when map[y, x + 1] is '.': //todo merge with above
                startTileOptions.RemoveAll(z => new[] { 'F', '|', 'L', '-' }.Contains(z));
                break;
        }

        switch (y)
        {
            case 0:
            case > 0 when map[y - 1, x] is '.':
                startTileOptions.RemoveAll(z => new[] { '|', 'L', 'J' }.Contains(z));
                break;
            case > 0 when map[y - 1, x] is '|':
                startTileOptions.RemoveAll(z => new[] { 'F', '-', '7' }.Contains(z));
                break;
            case > 0 when map[y + 1, x] is '|':
                startTileOptions.RemoveAll(z => new[] { 'J', '-', 'L' }.Contains(z));
                break;
            case > 0 when map[y + 1, x] is '.':
                startTileOptions.RemoveAll(z => new[] { 'F', '-', '7', '|' }.Contains(z));
                break;
        }

        // Replace the map's S with it's actual pipe symbol.
        map[startLocation.Y, startLocation.X] = startTileOptions[0];

        int x1 = startLocation.X, y1 = startLocation.Y, pipeLength = 0;
        var fromDirection = GetInitialStartDirection(map[startLocation.Y, startLocation.X]);

        do
        {
            (x1, y1, fromDirection) = InspectNextTile(x1, y1, fromDirection);

            pipeLength++;

            // The next tile is where we started, break.
            if (x1 == startLocation.X && y1 == startLocation.Y) break;
        } while (true);

        Assert.Equal(expectedAnswer, pipeLength / 2);

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
        (int x, int y, Direction direction) InspectNextTile(int x, int y, Direction fromDirection)
        {
            return map[y, x] switch
            {
                'F' when fromDirection is Direction.Down => new(x + 1, y, Direction.Left),
                'F' when fromDirection is Direction.Right => new(x, y + 1, Direction.Up),
                '-' when fromDirection is Direction.Left => new(x + 1, y, Direction.Left),
                '-' when fromDirection is Direction.Right => new(x - 1, y, Direction.Right),
                '7' when fromDirection is Direction.Left => new(x, y + 1, Direction.Up),
                '7' when fromDirection is Direction.Down => new(x - 1, y, Direction.Right),
                '|' when fromDirection is Direction.Up => new(x, y + 1, Direction.Up),
                '|' when fromDirection is Direction.Down => new(x, y - 1, Direction.Down),
                'J' when fromDirection is Direction.Up => new(x - 1, y, Direction.Right),
                'J' when fromDirection is Direction.Left => new(x, y - 1, Direction.Down),
                'L' when fromDirection is Direction.Up => new(x + 1, y, Direction.Left),
                'L' when fromDirection is Direction.Right => new(x, y - 1, Direction.Down),
                _ => throw new InvalidOperationException()
            };
        }
    }
}