using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day17
{
    /// <summary>
    /// Part 1 & 2: Ideally we should use Dijkstra's algorithm to find the 'least expensive' path on the weighted nodes. However this is more BFS
    /// but because we put it into a <see cref="PriorityQueue{TElement,TPriority}"/> and always dequeue the node with the least amount of heatloss
    /// we eventually get to the target node and implicitly the first time we see that node it's also incurred the lowest cost to get there. 
    /// </summary>
    [Theory]
    [InlineData("Day17DevelopmentTesting1.txt", 0, 3, 102)]
    [InlineData("Day17DevelopmentTesting1.txt", 4, 10, 94)]
    [InlineData("Day17.txt", 0, 3, 928)]
    [InlineData("Day17.txt", 4, 10, 1104)]
    public void Day17_Part1_ClumsyCrucible(string filename, int minConsecutiveMoves, int maxConsecutiveMoves, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToArray();

        int[,] arr = new int[input[0].Length, input.Length];

        // Convert the input file into a 2D char array.
        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];

            for (var col = 0; col < line.Length; col++)
            {
                arr[row, col] = line[col] - '0';
            }
        }

        // End location is fixed, it's the bottom right corner of the grid.
        int gridWidth = arr.GetLength(1) - 1, gridHeight = arr.GetLength(0) - 1;
        var endLocation = new Vertex(X: gridWidth, Y: gridHeight);

        HashSet<string> seen = [];
        var queue = new PriorityQueue<State, int>();

        // Seed start location...
        queue.Enqueue(new State(0, new Vertex(0, 0), Direction.Left, 0), priority: 0);

        var result = -1;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            // We've been on this vertex before, from this direction with this number of continuous steps...
            if (!seen.Add(current.Key)) continue;

            if (current.Position.X == endLocation.X && current.Position.Y == endLocation.Y)
            {
                result = current.HeatLoss; // We have our answer?
                break;
            }

            // Look ahead, then turn 'right' and then 'left' and add those vertices. Bounds checking is done later.
            switch (current.Direction)
            {
                // Store the heatloss of the vertex we're looking at so when it's on the sorted priority queue those with
                // the lowest accumulated heatloss will be topmost.
                case Direction.Down:
                {
                    // Straight ahead
                    if (current.ContinuousStepsInDirection < maxConsecutiveMoves && current.Position.Y > 0)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y - 1, current.Position.X];

                        queue.Enqueue(new State(heatLoss,
                            current.Position with { Y = current.Position.Y - 1 }, Direction.Down,
                            current.ContinuousStepsInDirection + 1), priority: heatLoss);
                    }
                    
                    if (current.ContinuousStepsInDirection < minConsecutiveMoves) break;

                    // Turn right
                    if (current.Position.X < gridWidth)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y, current.Position.X + 1];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { X = current.Position.X + 1 }, Direction.Left,
                            1), priority: heatLoss);
                    }

                    // Turn left
                    if (current.Position.X > 0)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y, current.Position.X - 1];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { X = current.Position.X - 1 }, Direction.Right,
                            1), priority: heatLoss);
                    }

                    break;
                }
                case Direction.Right:
                {
                    // Straight ahead
                    if (current.ContinuousStepsInDirection < maxConsecutiveMoves && current.Position.X > 0)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y, current.Position.X - 1];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { X = current.Position.X - 1 }, Direction.Right,
                            current.ContinuousStepsInDirection + 1), priority: heatLoss);
                    }
                    
                    if (current.ContinuousStepsInDirection < minConsecutiveMoves) break;

                    // Turn right
                    if (current.Position.Y > 0)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y - 1, current.Position.X];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { Y = current.Position.Y - 1 }, Direction.Down,
                            1), priority: heatLoss);
                    }

                    // Turn left
                    if (current.Position.Y < gridHeight)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y + 1, current.Position.X];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { Y = current.Position.Y + 1 }, Direction.Up,
                            1), priority: heatLoss);
                    }

                    break;
                }
                case Direction.Left:
                {
                    //if (current.Position.X == gridWidth - 1) break;
                    // Straight ahead
                    if (current.ContinuousStepsInDirection < maxConsecutiveMoves && current.Position.X < gridWidth)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y, current.Position.X + 1];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { X = current.Position.X + 1 }, Direction.Left,
                            current.ContinuousStepsInDirection + 1), priority: heatLoss);
                    }
                    
                    if (current.ContinuousStepsInDirection < minConsecutiveMoves) break;

                    // Turn right
                    if (current.Position.Y < gridHeight)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y + 1, current.Position.X];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { Y = current.Position.Y + 1 }, Direction.Up,
                            1), priority: heatLoss);
                    }

                    // Turn left
                    if (current.Position.Y > 0)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y - 1, current.Position.X];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { Y = current.Position.Y - 1 }, Direction.Down,
                            1), priority: heatLoss);
                    }

                    break;
                }
                case Direction.Up:
                {
                    // Straight ahead
                    if (current.ContinuousStepsInDirection < maxConsecutiveMoves && current.Position.Y < gridHeight)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y + 1, current.Position.X];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { Y = current.Position.Y + 1 }, Direction.Up,
                            current.ContinuousStepsInDirection + 1), priority: heatLoss);
                    }
                    
                    if (current.ContinuousStepsInDirection < minConsecutiveMoves) break;

                    // Turn right
                    if (current.Position.X > 0)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y, current.Position.X - 1];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { X = current.Position.X - 1 }, Direction.Right,
                            1), priority: heatLoss);
                    }

                    // Turn left
                    if (current.Position.X < gridWidth)
                    {
                        var heatLoss = current.HeatLoss + arr[current.Position.Y, current.Position.X + 1];
                        queue.Enqueue(new State(heatLoss,
                            current.Position with { X = current.Position.X + 1 }, Direction.Left,
                            1), priority: heatLoss);
                    }

                    break;
                }
                // Out of bounds?
                default:
                {
                    throw new Exception();
                }
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    private record Vertex(int X, int Y);

    private class State
    {
        public readonly string Key;

        public readonly int HeatLoss;
        public readonly Vertex Position;
        public readonly Direction Direction;
        public readonly int ContinuousStepsInDirection;

        public State(int heatLoss, Vertex position, Direction direction, int continuousStepsInDirection)
        {
            HeatLoss = heatLoss;
            Position = position;
            Direction = direction;
            ContinuousStepsInDirection = continuousStepsInDirection;

            Key = $"{Position.X}_{Position.Y}_{(int)Direction}_{ContinuousStepsInDirection}";
        }
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}