using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day3
{
    [Theory]
    [InlineData("Day3DevelopmentTesting1.txt", 4361)]
    [InlineData("Day3.txt", 550064)]
    public void Day3_Part1_GearRatios(string filename, int expectedAnswer)
    {
        int result = 0;
        string[] lines = FileLoader.ReadAllLines("2023/" + filename).Select(x => x + ".").ToArray();

        for (var i = 0; i < lines.Length; i++)
        {
            List<char> currentNumber = new();

            for (var j = 0; j < lines[i].Length; j++)
            {
                if (char.IsDigit(lines[i][j]))
                {
                    currentNumber.Add(lines[i][j]);
                }
                else
                {
                    if (currentNumber.Count == 0)
                    {
                        continue;
                    }

                    var isEnginePart = false;

                    var numberBeingTested = new string(currentNumber.ToArray());
                    currentNumber.Clear();

                    var numberLength = numberBeingTested.Length;

                    var preceedingChar = lines[i][Math.Max(0, j - numberLength - 1)];

                    if (!char.IsDigit(preceedingChar) && preceedingChar != '.')
                    {
                        isEnginePart = true;
                    }

                    var trailingChar = lines[i][Math.Min(lines[i].Length, j)];

                    if (!char.IsDigit(trailingChar) && trailingChar != '.')
                    {
                        isEnginePart = true;
                    }

                    // Check line below.
                    if (!isEnginePart && i + 1 < lines.Length)
                    {
                        var startIndex = Math.Max(0, j - numberLength - 1);
                        var lineBelow = lines[i + 1].Substring(startIndex, numberLength + (startIndex == 0 && j == 0 ? 1 : 2));

                        foreach (var c in lineBelow)
                        {
                            if (char.IsDigit(c) || c == '.')
                            {
                                isEnginePart = false;
                            }
                            else
                            {
                                isEnginePart = true;
                                break;
                            }
                        }
                    }

                    // Check preceeding line.
                    if (!isEnginePart && i - 1 > 0)
                    {
                        var startIndex = Math.Max(0, j - numberLength - 1);
                        var lineBefore = lines[i - 1].Substring(startIndex, numberLength + (startIndex == 0 ? 1 : 2));

                        foreach (var c in lineBefore)
                        {
                            if (char.IsDigit(c) || c == '.')
                            {
                                isEnginePart = false;
                            }
                            else
                            {
                                isEnginePart = true;
                                break;
                            }
                        }
                    }

                    if (!isEnginePart) continue;

                    result += int.Parse(numberBeingTested);
                }
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day3DevelopmentTesting1.txt", 467835)]
    [InlineData("Day3.txt", 85010461)]
    public void Day3_Part2_GearRatios(string filename, int expectedAnswer)
    {
        var result = 0;

        string[] lines = FileLoader.ReadAllLines("2023/" + filename).Select(x => x + ".").ToArray();

        for (var row = 0; row < lines.Length; row++)
        {
            var line = lines[row];

            if (!line.Contains('*')) continue;

            for (var col = 0; col < line.Length; col++)
            {
                if (lines[row][col] != '*') continue;

                // We've found a gear, check if it's adjacent to two engine parts.
                var foundDigits = new List<Point>();

                // The following is a bit rough and doesn't do correct bounds checking (extreme left and right)
                // when checking the preceeding and following rows. 

                // Check preceeding char...
                if (char.IsDigit(lines[row][Math.Max(0, col - 1)]))
                {
                    foundDigits.Add(new Point(row, col - 1));
                }

                // Check trailing char...
                if (char.IsDigit(lines[row][Math.Min(lines[row].Length, col + 1)]))
                {
                    foundDigits.Add(new Point(row, col + 1));
                }

                // Check above...
                if (row > 0)
                {
                    // Directly
                    if (char.IsDigit(lines[row - 1][col]))
                    {
                        foundDigits.Add(new Point(row - 1, col));
                    }
                    else
                    {
                        // Up & Left
                        if (char.IsDigit(lines[row - 1][col - 1]))
                        {
                            foundDigits.Add(new Point(row - 1, col - 1));
                        }

                        // Up & Right
                        if (char.IsDigit(lines[row - 1][col + 1]))
                        {
                            foundDigits.Add(new Point(row - 1, col + 1));
                        }
                    }
                }

                // Check below...
                if (row < lines.Length)
                {
                    // Directly
                    if (char.IsDigit(lines[row + 1][col]))
                    {
                        foundDigits.Add(new Point(row + 1, col));
                    }
                    else
                    {
                        // Down & Left
                        if (char.IsDigit(lines[row + 1][col - 1]))
                        {
                            // Down & Left
                            foundDigits.Add(new Point(row + 1, col - 1));
                        }

                        // Down & Right
                        if (char.IsDigit(lines[row + 1][col + 1]))
                        {
                            foundDigits.Add(new Point(row + 1, col + 1));
                        }
                    }
                }

                if (foundDigits.Count != 2) continue;

                // Calculate the first full integer for the two adjacent engine parts.
                var point = foundDigits[0];
                var lineTmp = lines[point.Row];

                // Look back for the start of this integer.
                var digitsStartIndex = -1;

                for (var i = point.Column; i >= 0; i--)
                {
                    if (char.IsDigit(lineTmp[i])) continue;

                    digitsStartIndex = i + 1;
                    break;
                }

                if (digitsStartIndex < 0)
                {
                    digitsStartIndex = 0;
                }

                // Look forward for the end of this integer.
                var digitsEndIndex = -1;

                for (var i = point.Column; i <= lineTmp.Length; i++)
                {
                    if (char.IsDigit(lineTmp[i])) continue;

                    digitsEndIndex = i;

                    break;
                }

                var num = int.Parse(lineTmp.Substring(digitsStartIndex, digitsEndIndex - digitsStartIndex));

                // Determine the second digit.
                point = foundDigits[1];
                lineTmp = lines[point.Row];

                digitsStartIndex = -1;

                for (var i = point.Column; i >= 0; i--)
                {
                    if (char.IsDigit(lineTmp[i])) continue;

                    digitsStartIndex = i + 1;

                    break;
                }

                digitsEndIndex = lineTmp.IndexOf('.', point.Column) - digitsStartIndex;

                if (digitsStartIndex < 0)
                {
                    digitsStartIndex = 0;
                    digitsEndIndex--;
                }

                var num2 = int.Parse(lineTmp.Substring(digitsStartIndex, digitsEndIndex));

                result += num * num2;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }

    private record struct Point(int Row, int Column);
}
