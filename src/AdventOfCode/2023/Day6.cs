using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day6
{
    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 288)]
    [InlineData("Day6.txt", 2612736)]
    public void Day6_Part1_WaitForIt(string filename, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToArray();

        var times = Array.ConvertAll(fileInput[0][(fileInput[0].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
        var distances = Array.ConvertAll(fileInput[1][(fileInput[1].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
        var recordBeatingAlternatives = new int[times.Length];

        for (var i = 0; i < times.Length; i++)
        {
            for (var secondsButtonPressed = 1; secondsButtonPressed < times[i]; secondsButtonPressed++)
            {
                if (secondsButtonPressed * (times[i] - secondsButtonPressed) > distances[i])
                {
                    recordBeatingAlternatives[i] += 1;
                }
            }
        }

        var result = recordBeatingAlternatives.Aggregate(1, (current, a) => current * a);

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day6DevelopmentTesting1.txt", 71503)]
    [InlineData("Day6.txt", 29891250)]
    public void Day6_Part2_WaitForIt(string filename, long expectedAnswer)
    {
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToArray();

        var time = int.Parse(string.Concat(fileInput[0][(fileInput[0].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
        var distance = long.Parse(string.Concat(fileInput[1][(fileInput[1].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries)));

        var result = 0;

        for (long secondsButtonPressed = 1; secondsButtonPressed < time; secondsButtonPressed++)
        {
            if (secondsButtonPressed * (time - secondsButtonPressed) > distance) result += 1;
        }

        Assert.Equal(expectedAnswer, result);
    }
}
