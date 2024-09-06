using AdventOfCode.Types;
using AdventOfCode.Utilities;

namespace AdventOfCode._2015;

public class Day1
{
    [Theory]
    [InlineData("Day1.txt", 280, Part.One)]
    [InlineData("Day1.txt", 1797, Part.Two)]
    public void Day1_Part1_NotQuiteLisp(string filename, int expectedAnswer, Part problemPart)
    {
        string[] input = FileLoader.ReadAllLines("2015/" + filename).ToArray();
        int floor = 0, index;

        for (index = 0; index < input[0].Length; index++)
        {
            if (problemPart == Part.Two && floor is -1) break;

            var a = input[0][index];

            switch (a)
            {
                case '(':
                    floor++;
                    break;
                default:
                    floor--;
                    break;
            }
        }

        switch (problemPart)
        {
            case Part.One:
                Assert.Equal(expectedAnswer, floor);
                break;
            case Part.Two:
                Assert.Equal(expectedAnswer, index);
                break;
        }
    }
}