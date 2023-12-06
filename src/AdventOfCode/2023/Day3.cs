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
        string[] lines = FileLoader.ReadFile("2023/" + filename).Select(x => x + ".").ToArray();

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

                    bool isEnginePart = false;

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
    [InlineData("Day3.txt", 550064)]
    public void Day3_Part2_GearRatios(string filename, int expectedAnswer)
    {
        int result = 0;
        int lastNumber = 0;
        int lastSymbol = 0;
        int currentSymbol = 0;

        string[] lines = FileLoader.ReadFile("2023/" + filename).Select(x => x + ".").ToArray();

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
                    char partSymbol = '\0';

                    if (currentNumber.Count == 0)
                    {
                        continue;
                    }

                    bool isEnginePart = false;

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

                    if (preceedingChar == '*' || trailingChar == '*') currentSymbol = '*';

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
                                // if (c == '*' ) partSymbol = '*';
                                currentSymbol = c;
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
                                // if (c == '*' ) partSymbol = '*';
                                currentSymbol = c;

                                break;
                            }
                        }
                    }

                    if (isEnginePart)
                    {
                        if (lastSymbol == currentSymbol && currentSymbol == '*')
                        {
                            result += (lastNumber * int.Parse(numberBeingTested)) - lastNumber;
                        }
                        else
                        {
                            result += int.Parse(numberBeingTested);
                        }
                    }

                    //         else
                    //         {
                    //    lastNumber = '\0';
                    // }
                    lastNumber = int.Parse(numberBeingTested);
                    lastSymbol = currentSymbol;
                }
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}
