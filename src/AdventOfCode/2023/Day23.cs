using AdventOfCode.Types;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day23
{
    /// <summary>
    /// Part 1 & 2: The map can be reduced down to a directed graph but we cannot use Dijkstra's algorithm because that's for shortest path. We need to use a
    /// modified DFS search algorithm to find the longest path.
    ///
    /// Part 1 could be done without brute forcing because it's actually a directed acyclic graph (DAG) and topographical sorting of the graph within a loop
    /// can net us the longest path, see 'Longest path on DAG' https://www.youtube.com/watch?v=TXkDpqjDMHA.
    /// </summary>
    [Theory]
    [InlineData("Day23DevelopmentTesting1.txt", 9, 94, Part.One)]
    [InlineData("Day23.txt", 36, 2106, Part.One)]
    [InlineData("Day23DevelopmentTesting1.txt", 9, 154, Part.Two)]
    [InlineData("Day23.txt", 36, 6350, Part.Two)]
    public void Day23_Part1_ALongWalk(string filename, int expectedVertices, int expectedAnswer, Part problemPart)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToArray();
        char[,] grid = new char[input.Length, input[0].Length];

        // Convert the input file into a 2D char array.
        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];

            for (var col = 0; col < line.Length; col++)
            {
                grid[row, col] = line[col];
            }
        }

        (int Row, int Col) startPoint = (0, input[0].IndexOf('.'));
        (int Row, int Col) endPoint = (input.Length - 1, input[^1].IndexOf('.'));

        List<(int Row, int Col)> points = [startPoint, endPoint];

        // Iterate over the grid and identify all points that have at least 3 neighbours. These 
        // are the vertices of our graph, having removed the other unnecessary noise.
        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                if (grid[row, col] == '#') continue;

                int neighbours = 0;

                foreach (var (nr, nc) in new[]
                         {
                             (row - 1, col),
                             (row + 1, col),
                             (row, col - 1),
                             (row, col + 1)
                         })
                {
                    // Skip this point if we're out of bounds or in the 'forest'.
                    if (nr >= 0 && nr < grid.GetLength(0) &&
                        nc >= 0 && nc < grid.GetLength(1) &&
                        grid[nr, nc] != '#')
                    {
                        neighbours += 1;
                    }

                    // Identify if the grid point has 3 or more neighbours and if it does add it to the Points list. 
                    // We later use this list of points to recreate the graph using only those points.
                    if (neighbours >= 3)
                    {
                        if (!points.Contains((row, col)))
                        {
                            points.Add((row, col));
                        }
                    }
                }
            }
        }

        Assert.Equal(expectedVertices, points.Count);

        // This dictionary lets us store each neighbour for a point and far away that neighbouring point is.
        // It forms an adjacency list; https://www.geeksforgeeks.org/adjacency-list-meaning-definition-in-dsa/.
        var graph = new Dictionary<(int Row, int Col), Dictionary<(int Row, int Col), int>>();

        foreach (var pt in points)
        {
            graph[(pt.Row, pt.Col)] = new Dictionary<(int Row, int Col), int>();
        }

        var directions = new Dictionary<char, List<(int Row, int Col)>>
        {
            { '^', [(-1, 0)] },
            { 'v', [(1, 0)] },
            { '<', [(0, -1)] },
            { '>', [(0, 1)] },
            { '.', [(-1, 0), (1, 0), (0, -1), (0, 1)] }
        };

        foreach (var (sr, sc) in points)
        {
            // Use a stack, we're going to perform a DFS,
            var stack = new Stack<(int seenCount, int seenRow, int seenCol)>();
            stack.Push((0, sr, sc));

            var seen = new HashSet<(int Row, int Col)> { (sr, sc) };

            while (stack.Count > 0)
            {
                var (n, r, c) = stack.Pop();

                // If not the starting position, n=0, and our starting row (sr) and starting column (sc) have
                // a connection to the new row/col we've found, add it to the collection of graph vertices.
                if (n != 0 && points.Any(p => p.Row == r && p.Col == c))
                {
                    graph[(sr, sc)][(r, c)] = n;
                    continue;
                }

                if (problemPart == Part.One)
                {
                    foreach (var (dr, dc) in directions[grid[r, c]])
                    {
                        var nr = r + dr;
                        var nc = c + dc;

                        if (nr >= 0 && nr < grid.GetLength(0) &&
                            nc >= 0 && nc < grid.GetLength(1) &&
                            grid[nr, nc] != '#' &&
                            !seen.Contains((nr, nc)))
                        {
                            // Push the new row/col to the stack, incrementing the counter by 1.
                            stack.Push((n + 1, nr, nc));

                            seen.Add((nr, nc));
                        }
                    }
                }
                // For part 2 we treat all tiles as . and ignore slopes. This means we don't need the
                // 'directions' collection from part 1.
                else if (problemPart == Part.Two)
                {
                    foreach (var (dr, dc) in new[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                    {
                        var nr = r + dr;
                        var nc = c + dc;

                        if (nr >= 0 && nr < grid.GetLength(0) &&
                            nc >= 0 && nc < grid.GetLength(1) &&
                            grid[nr, nc] != '#' &&
                            !seen.Contains((nr, nc)))
                        {
                            // Push the new row/col to the stack, incrementing the counter by 1.
                            stack.Push((n + 1, nr, nc));

                            seen.Add((nr, nc));
                        }
                    }
                }
            }
        }

        HashSet<(int y, int x)> dfsProcessedPoints = [];
        var result = GetDfsMaxPointDistance(startPoint);

        Assert.Equal(expectedAnswer, result);
        return;

        // Modified DFS search function. 
        // We iterate over all the adjacent points for each node in the graph collection and
        // recursively look at each of it's adjacent nodes to determine the furthest distance.
        int GetDfsMaxPointDistance((int y, int x) point)
        {
            if (point == endPoint) return 0;

            int max = int.MinValue;

            // This is really only required for Part 2 as Part 1 doesn't have any cycles and so doesn't
            // loop back over already seen points. The removal of slopes changes this behaviour.
            dfsProcessedPoints.Add(point);

            foreach (KeyValuePair<(int y, int x), int> nx in graph[(point.y, point.x)])
            {
                // Ensure we haven't seen this point before, to avoid crossing the same path over and over.
                if (dfsProcessedPoints.Contains(nx.Key)) continue;

                // Add the nx.Key distance to the result of recursively calling GetDfsMaxPointDistance and test against 'max'
                // for the furthest distance.
                max = Math.Max(max,
                    GetDfsMaxPointDistance(nx.Key) + graph[(point.y, point.x)][(nx.Key.y, nx.Key.x)]);
            }

            dfsProcessedPoints.Remove(point);

            return max;
        }
    }
}