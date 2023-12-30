using AdventOfCode.Extensions;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day18
{
    [Theory]
    [InlineData("Day18DevelopmentTesting1.txt", 62)]
    [InlineData("Day18.txt", 34329)] // 31057 31058 too low. 36552 too high   34330
    public void Day19_Part1_LavaductLagoon(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToList();

        int x = 0, y = 0, maxX = 0, maxY = 0, startX = 0, startY = 0;

        // Determine the maximum Right and Down steps so we know the size of the ground map.
        foreach (var row in input)
        {
            var instructions = row.Split(' ');
            var direction = instructions[0];
            var distance = int.Parse(instructions[1]);

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

            if (x < 0) startX = Math.Max(startX, Math.Abs(x));
            if (y < 0) startY = Math.Max(startY, Math.Abs(y));

            maxX = Math.Max(maxX, x);
            maxY = Math.Max(maxY, y);
        }

        var groundMap = new char[startY + maxY + 1, startX + maxX + 1];

        // Pre fill the ground map with dirt.
        for (var i = 0; i < groundMap.GetLength(0); i++)
        {
            for (var j = 0; j < groundMap.GetLength(1); j++)
            {
                groundMap[i, j] = '.';
            }
        }

        // Dig the trench...
        x = startX;
        y = startY;

        foreach (var row in input)
        {
            var instructions = row.Split(' ');
            var direction = instructions[0];
            var distance = int.Parse(instructions[1]);


            switch (direction)
            {
                case "R":
                {
                    for (var i = 0; i < distance; i++)
                    {
                        x++;
                        groundMap[y, x] = '#';
                    }

                    break;
                }
                case "L":
                {
                    for (var i = 0; i < distance; i++)
                    {
                        x--;
                        groundMap[y, x] = '#';
                    }

                    break;
                }
                case "D":
                {
                    for (var i = 0; i < distance; i++)
                    {
                        y++;
                        groundMap[y, x] = '#';
                    }

                    break;
                }
                case "U":
                {
                    for (var i = 0; i < distance; i++)
                    {
                        y--;
                        groundMap[y, x] = '#';
                    }

                    break;
                }
            }
        }

        // Dig out interior...
        for (var i = 0; i < groundMap.GetLength(0); i++)
        {
            var insideTrench = false;
            char lastTile = '.';

            for (var j = 0; j < groundMap.GetLength(1); j++)
            {
                var currentTile = groundMap[i, j];
                var nextTile = j == groundMap.GetLength(1) - 1 ? '#' : groundMap[i, j + 1];

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
                        if (i > 0 && groundMap[i - 1, j + 1] == 'O')
                        {
                            insideTrench = true;
                        }
                        else insideTrench = false;

                        break;

                    case '.' when insideTrench:
                        // Dig...
                        groundMap[i, j] = 'O';
                        break;
                }

                lastTile = currentTile;
            }
        }

        // groundMap.WriteToFile("C:/temp/Day18_complete.txt"); // Debug

        // Count the size of the interior...
        int result = 0;

        for (var i = 0; i < groundMap.GetLength(0); i++)
        {
            for (var j = 0; j < groundMap.GetLength(1); j++)
            {
                if (groundMap[i, j] == '#' || groundMap[i, j] == 'O') result++;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}
