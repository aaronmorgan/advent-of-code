using System.Text;
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
                case "R":
                {
                    x += distance;
                    break;
                }
                case "L":
                {
                    x -= distance;
                    break;
                }
                case "D":
                {
                    y += distance;
                    break;
                }
                case "U":
                {
                    y -= distance;
                    break;
                }
            }

            points[rowIndex + 1] = new ShoelaceFormula.Point(x, y);
        }

        points[^1] = new ShoelaceFormula.Point(points[0].X, points[0].Y);

        var polygonArea = ShoelaceFormula.CalculatePolygonArea(points, boundaryLength);

        Assert.Equal(polygonArea, expectedAnswer);
    }

    [Theory(Skip = "")]
    [InlineData("Day18DevelopmentTesting1.txt", 952408144115)]
    //   [InlineData("Day18.txt", 34329)]
    public void Day18_Part2_LavaductLagoon(string filename, long expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToList();

        int x = 0, y = 0, maxX = 0, maxY = 0, startX = 0, startY = 0;

        // Determine the maximum Right and Down steps so we know the size of the ground map.
        foreach (var row in input)
        {
            var instructions = row.Split('#')[1].Trim('#', ')');
            var direction = instructions[^1];
            var distance = Convert.ToInt32(instructions[..5], 16);

            switch (direction)
            {
                case '0': // Right
                {
                    x += distance;
                    break;
                }
                case '1': // Down
                {
                    y += distance;
                    break;
                }
                case '2': // Left
                {
                    x -= distance;
                    break;
                }

                case '3': // Up
                {
                    y -= distance;
                    break;
                }
            }

            if (x < 0) startX = Math.Max(startX, Math.Abs(x));
            if (y < 0) startY = Math.Max(startY, Math.Abs(y));

            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);
        }

        // Pre fill the ground map with dirt.
        int yLineCount = startY + maxY + 1;
        string xLine = new string('.', startX + maxX + 1);
        List<StringBuilder> groundMap = new(yLineCount);

        for (int i = 0; i < yLineCount; i++)
        {
            groundMap.Add(new StringBuilder(xLine));
        }

        // Dig the trench...
        x = startX;
        y = startY;

        foreach (var row in input)
        {
            var instructions = row.Split('#')[1].Trim('#', ')');
            var direction = instructions[^1];
            var distance = Convert.ToInt32(instructions[..5], 16);

            switch (direction)
            {
                case '0': // Right
                {
                    for (var i = 0; i < distance; i++)
                    {
                        x++;
                        groundMap[y][4] = '#';
                    }

                    break;
                }
                case '1': // Down
                {
                    for (var i = 0; i < distance; i++)
                    {
                        y++;
                        groundMap[y][x] = '#';
                    }

                    break;
                }
                case '2': // Left
                {
                    for (var i = 0; i < distance; i++)
                    {
                        x--;
                        groundMap[y][x] = '#';
                    }

                    break;
                }
                case '3': // Up
                {
                    for (var i = 0; i < distance; i++)
                    {
                        y--;
                        groundMap[y][x] = '#';
                    }

                    break;
                }
            }
        }

        // Dig out interior...
        for (var i = 0; i < groundMap.Count; i++)
        {
            var insideTrench = false;
            char lastTile = '.';

            for (var j = 0; j < groundMap[i].Length; j++)
            {
                var currentTile = groundMap[i][j];
                var nextTile = j == groundMap[i].Length - 1 ? '#' : groundMap[i][j + 1];

                switch (currentTile)
                {
                    // Encounter the trench edge afer not digging, start digging.
                    case '#' when lastTile == '.' && nextTile == '.' && !insideTrench:
                        insideTrench = true;
                        break;

                    case '#' when lastTile == '.' && nextTile == '.' && insideTrench:
                        insideTrench = false;
                        break;
                    case '#' when lastTile == '#' && nextTile == '.':
                        // Look above us to see if we at at the ┘ corner of the trench boundry and therefore the next tile 
                        // is actually within the trench boundy.
                        if (i > 0 && groundMap[i - 1][j + 1] == 'O')
                        {
                            insideTrench = true;
                        }
                        else insideTrench = false;

                        break;

                    case '.' when insideTrench:
                        // Dig...
                        groundMap[i][j] = 'O';
                        break;
                }

                lastTile = currentTile;
            }
        }

        // Count the size of the interior...
        int result = 0;

        for (var i = 0; i < groundMap.Count; i++)
        {
            for (var j = 0; j < groundMap[i].Length; j++)
            {
                if (groundMap[i][j] == '#' || groundMap[i][j] == 'O') result++;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}
