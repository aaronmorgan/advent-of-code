using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day2
{
    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 8, 12, 13, 14)]
    [InlineData("Day2.txt", 2006, 12, 13, 14)]
    public void Day2_Part1_CubeConundrum(string filename, int expectedAnswer, int redCubes, int greenCubes, int blueCubes)
    {
        int result = 0;

        foreach (var game in FileLoader.ReadAllLines("2023/" + filename))
        {
            var gamenumber = int.Parse(game[..game.IndexOf(':')].Split(' ')[1]);
            var hands = game[(game.IndexOf(':') + 2)..].Split(';');
            var handSucceeds = true;

            foreach (var hand in hands)
            {
                int redCount = 0, greenCount = 0, blueCount = 0;

                var cubes = hand.Split(',').Select(x => x.Trim());

                foreach (var cube in cubes)
                {
                    switch (cube)
                    {
                        case not null when cube.EndsWith("red"):
                            redCount += DeriveValue(cube);
                            continue;
                        case not null when cube.EndsWith("green"):
                            greenCount += DeriveValue(cube);
                            continue;
                        case not null when cube.EndsWith("blue"):
                            blueCount += DeriveValue(cube);
                            continue;
                    }
                }

                if (redCount > redCubes || greenCount > greenCubes || blueCount > blueCubes)
                {
                    handSucceeds = false;
                    break;
                }
            }

            if (handSucceeds)
            {
                result += gamenumber;
            }
        }

        Assert.Equal(expectedAnswer, result);

        static int DeriveValue(string intput) => int.Parse(intput[..intput.IndexOf(' ')]);
    }

    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 2286)]
    [InlineData("Day2.txt", 84911)]
    public void Day2_Part2_CubeConundrum(string filename, int expectedAnswer)
    {
        int result = 0;

        foreach (var game in FileLoader.ReadAllLines("2023/" + filename))
        {
            var hands = game[(game.IndexOf(':') + 2)..].Split(';');
            int redCount = 0, greenCount = 0, blueCount = 0;

            foreach (var hand in hands)
            {
                var cubes = hand.Split(',').Select(x => x.Trim());

                foreach (var cube in cubes)
                {
                    switch (cube)
                    {
                        case not null when cube.EndsWith("red"):
                            redCount = Math.Max(redCount, DeriveValue(cube));
                            continue;
                        case not null when cube.EndsWith("green"):
                            greenCount = Math.Max(greenCount, DeriveValue(cube));
                            continue;
                        case not null when cube.EndsWith("blue"):
                            blueCount = Math.Max(blueCount, DeriveValue(cube));
                            continue;
                    }
                }
            }

            result += redCount * greenCount * blueCount;
        }

        Assert.Equal(expectedAnswer, result);

        return;

        static int DeriveValue(string intput) => int.Parse(intput[..intput.IndexOf(' ')]);
    }
}
