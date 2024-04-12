using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day16
{
    [Theory]
    [InlineData("Day16DevelopmentTesting1.txt", 46)]
    [InlineData("Day16.txt", 6795)]
    public void Day16_Part1_TheFloorWillBeLava(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToArray();
        char[,] arr = new char[input.Length, input[0].Length];

        // Convert the input file into a 2D char array.
        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];

            for (var col = 0; col < line.Length; col++)
            {
                arr[row, col] = line[col];
            }
        }

        var energisedTilesCount = ProcessRoute(arr, new Move(0, 0, Direction.Left));

        Assert.Equal(expectedAnswer, energisedTilesCount);
    }

    [Theory]
    [InlineData("Day16DevelopmentTesting1.txt", 51)]
    // [InlineData("Day16.txt", 7154)] // Takes 90 seconds to complete.
    public void Day16_Part2_TheFloorWillBeLava(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToArray();
        char[,] arr = new char[input.Length, input[0].Length];

        int maxEnergisedTiles = 0;

        // Convert the input file into a 2D char array.
        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];

            for (var col = 0; col < line.Length; col++)
            {
                arr[row, col] = line[col];
            }
        }

        int arrayWidth = arr.GetLength(1) - 1, arrayHeight = arr.GetLength(0) - 1;

        // Process across the top side.
        for (var x = 0; x < arrayWidth; x++)
        {
            var energisedTilesCount = ProcessRoute(arr, new Move(x, 0, Direction.Above));

            if (energisedTilesCount > maxEnergisedTiles)
            {
                maxEnergisedTiles = energisedTilesCount;
            }
        }

        // Process down the left hand side.
        for (var y = 0; y < arrayHeight; y++)
        {
            var energisedTilesCount = ProcessRoute(arr, new Move(0, y, Direction.Left));

            if (energisedTilesCount > maxEnergisedTiles)
            {
                maxEnergisedTiles = energisedTilesCount;
            }
        }

        // Process down the right hand side.
        for (var y = 0; y < arrayHeight; y++)
        {
            var energisedTilesCount = ProcessRoute(arr, new Move(input[0].Length - 1, y, Direction.Right));

            if (energisedTilesCount > maxEnergisedTiles)
            {
                maxEnergisedTiles = energisedTilesCount;
            }
        }

        // Process across the bottom side.
        for (var x = 0; x < arrayWidth; x++)
        {
            var energisedTilesCount = ProcessRoute(arr, new Move(x, arrayHeight, Direction.Below));

            if (energisedTilesCount > maxEnergisedTiles)
            {
                maxEnergisedTiles = energisedTilesCount;
            }
        }

        Assert.Equal(expectedAnswer, maxEnergisedTiles);
    }

    /// <summary>
    /// Ideally would be a BFS search algo following the mirrors, but because it's using a Queue<T> and only one, it's really DFS.
    /// It adds to the queue path, branches, then backtracks (unwinds the queue) when progression halts.
    /// </summary>
    /// <remarks>Might be faster with a Stack<T>...</remarks>
    private static int ProcessRoute(char[,] arr, Move startingMove)
    {
        List<string> seen = [], energisedTiles = [];
        int arrayWidth = arr.GetLength(1) - 1, arrayHeight = arr.GetLength(0) - 1;

        var moves = new Queue<Move>();
        moves.Enqueue(startingMove);

        while (moves.Count > 0)
        {
            var move = moves.Dequeue();

            // Navigate array starting from 0,0 and moving to the right.
            if (move.X < 0 || move.Y < 0 || move.X > arrayWidth || move.Y > arrayHeight) continue;

            var energisedKey = move.X + "_" + move.Y;
            var seenKey = energisedKey + "_" + move.FromDirection;

            if (seen.IndexOf(seenKey) > 0) continue;

            if (!energisedTiles.Contains(energisedKey))
            {
                energisedTiles.Add(energisedKey);
            }

            seen.Add(seenKey);

            switch (arr[move.Y, move.X])
            {
                case '.':
                    switch (move.FromDirection)
                    {
                        case Direction.Above:
                            moves.Enqueue(new Move(move.X, move.Y + 1, Direction.Above));
                            break;
                        case Direction.Right:
                            moves.Enqueue(new Move(move.X - 1, move.Y, Direction.Right));
                            break;
                        case Direction.Below:
                            moves.Enqueue(new Move(move.X, move.Y - 1, Direction.Below));
                            break;
                        case Direction.Left:
                            moves.Enqueue(new Move(move.X + 1, move.Y, Direction.Left));
                            break;
                    }

                    break;
                case '\\':
                    switch (move.FromDirection)
                    {
                        case Direction.Above:
                            moves.Enqueue(new Move(move.X + 1, move.Y, Direction.Left));
                            break;
                        case Direction.Right:
                            moves.Enqueue(new Move(move.X, move.Y - 1, Direction.Below));
                            break;
                        case Direction.Below:
                            moves.Enqueue(new Move(move.X - 1, move.Y, Direction.Right));
                            break;
                        case Direction.Left:
                            moves.Enqueue(new Move(move.X, move.Y + 1, Direction.Above));
                            break;
                    }

                    break;
                case '/':
                    switch (move.FromDirection)
                    {
                        case Direction.Above:
                            moves.Enqueue(new Move(move.X - 1, move.Y, Direction.Right));
                            break;
                        case Direction.Right:
                            moves.Enqueue(new Move(move.X, move.Y + 1, Direction.Above));
                            break;
                        case Direction.Below:
                            moves.Enqueue(new Move(move.X + 1, move.Y, Direction.Left));
                            break;
                        case Direction.Left:
                            moves.Enqueue(new Move(move.X, move.Y - 1, Direction.Below));
                            break;
                    }

                    break;
                case '|':
                    switch (move.FromDirection)
                    {
                        case Direction.Above:
                            moves.Enqueue(new Move(move.X, move.Y + 1, Direction.Above));
                            break;
                        case Direction.Below:
                            moves.Enqueue(new Move(move.X, move.Y - 1, Direction.Below));
                            break;
                        case Direction.Right:
                        case Direction.Left:
                            moves.Enqueue(new Move(move.X, move.Y - 1, Direction.Below));
                            moves.Enqueue(new Move(move.X, move.Y + 1, Direction.Above));
                            break;
                    }

                    break;
                case '-':
                    switch (move.FromDirection)
                    {
                        case Direction.Left:
                            moves.Enqueue(new Move(move.X + 1, move.Y, Direction.Left));
                            break;
                        case Direction.Right:
                            moves.Enqueue(new Move(move.X - 1, move.Y, Direction.Right));
                            break;
                        case Direction.Below:
                        case Direction.Above:
                            moves.Enqueue(new Move(move.X - 1, move.Y, Direction.Right));
                            moves.Enqueue(new Move(move.X + 1, move.Y, Direction.Left));
                            break;
                    }

                    break;
            }
        }

        return energisedTiles.Count;
    }

    private record struct Move(int X, int Y, Direction FromDirection);

    private enum Direction
    {
        Above,
        Below,
        Left,
        Right
    }
}
