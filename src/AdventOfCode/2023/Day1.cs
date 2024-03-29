using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day1
{
    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 142)]
    [InlineData("Day1.txt", 55172)]
    public void Day1_Part1_Trebuchet(string filename, int expectedAnswer)
    {
        int result = 0;

        foreach (var line in FileLoader.ReadAllLines("2023/" + filename))
        {
            if (string.IsNullOrEmpty(line)) continue;

            var digits = new char[2];

            foreach (var t in line.Where(char.IsDigit))
            {
                if (digits[0] == '\0')
                {
                    digits[0] = t;
                }

                digits[1] = t;
            }

            result += int.Parse(new string(digits));
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day1DevelopmentTesting2.txt", 281)]
    [InlineData("Day1.txt", 54925)]
    public void Day1_Part2_Trebuchet(string filename, int expectedAnswer)
    {
        var result = 0;

        string? firstDigit, secondDigit;

        foreach (var line in FileLoader.ReadAllLines("2023/" + filename))
        {
            if (string.IsNullOrEmpty(line)) continue;

            firstDigit = string.Empty;
            secondDigit = string.Empty;

            for (var i = 0; i < line.Length; i++)
            {
                switch (line[i..])
                {
                    case { } s when int.TryParse(s, out var x):
                        SetDigit(x);
                        continue;
                    case { } s when s.StartsWith("one"):
                        SetDigit(1);
                        i += 2;
                        continue;
                    case { } s when s.StartsWith("two"):
                        SetDigit(2);
                        i += 2;
                        continue;
                    case { } s when s.StartsWith("three"):
                        SetDigit(3);
                        i += 4;
                        continue;
                    case { } s when s.StartsWith("four"):
                        SetDigit(4);
                        i += 3;
                        continue;
                    case { } s when s.StartsWith("five"):
                        SetDigit(5);
                        i += 3;
                        continue;
                    case { } s when s.StartsWith("six"):
                        SetDigit(6);
                        i += 2;
                        continue;
                    case { } s when s.StartsWith("seven"):
                        SetDigit(7);
                        i += 4;
                        continue;
                    case { } s when s.StartsWith("eight"):
                        SetDigit(8);
                        i += 4;
                        continue;
                    case { } s when s.StartsWith("nine"):
                        SetDigit(9);
                        i += 3;
                        continue;

                    default:
                        return;
                }
            }

            result += int.Parse(firstDigit + secondDigit);
        }

        Assert.Equal(expectedAnswer, result);

        return;

        void SetDigit(int number)
        {
            if (string.IsNullOrEmpty(firstDigit))
            {
                firstDigit = number.ToString();
            }

            secondDigit = number.ToString();
        }
    }
}
