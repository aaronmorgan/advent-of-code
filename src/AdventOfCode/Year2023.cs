using System.Collections;
using System.Text;
using Microsoft.VisualBasic;
using Xunit.Abstractions;

namespace AdventOfCode;

public class Year2023
{
    private readonly ITestOutputHelper output;

    public Year2023(ITestOutputHelper testOutputHelper)
    {
            output = testOutputHelper;
    }
    
    private static IEnumerable<string> ReadFile(string filename) => File.ReadAllLines($"./TestData/2023/{filename}");

    [Theory]
    [InlineData("Day1DevelopmentTesting1.txt", 142)]
    [InlineData("Day1.txt", 55172)]
    public void Day1_Part1_Trebuchet(string filename, int expectedAnswer)
    {
        int result = 0;

        foreach (var line in ReadFile(filename))
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

        foreach (var line in ReadFile(filename))
        {
            if (string.IsNullOrEmpty(line)) continue;

            firstDigit = string.Empty;
            secondDigit = string.Empty;

            for (var i = 0; i < line.Length; i++)
            {
                switch (line[i..])
                {
                    case { } s when int.TryParse(s, out var x): SetDigit(x); continue;
                    case { } s when s.StartsWith("one"): SetDigit(1); i += 2; continue;
                    case { } s when s.StartsWith("two"): SetDigit(2); i += 2; continue;
                    case { } s when s.StartsWith("three"): SetDigit(3); i += 4; continue;
                    case { } s when s.StartsWith("four"): SetDigit(4); i += 3; continue;
                    case { } s when s.StartsWith("five"): SetDigit(5); i += 3; continue;
                    case { } s when s.StartsWith("six"): SetDigit(6); i += 2; continue;
                    case { } s when s.StartsWith("seven"): SetDigit(7); i += 4; continue;
                    case { } s when s.StartsWith("eight"): SetDigit(8); i += 4; continue;
                    case { } s when s.StartsWith("nine"): SetDigit(9); i += 3; continue;

                    default:
                        return;
                }
            }

            result += int.Parse(firstDigit + secondDigit);
        }

        Assert.Equal(expectedAnswer, result);

        void SetDigit(int number)
        {
            if (string.IsNullOrEmpty(firstDigit))
            {
                firstDigit = number.ToString();
            }

            secondDigit = number.ToString();
        }
    }

    [Theory]
    [InlineData("Day2DevelopmentTesting1.txt", 8, 12, 13, 14)]
    [InlineData("Day2.txt", 2006, 12, 13, 14)]
    public void Day2_Part1_CubeConundrum(string filename, int expectedAnswer, int redCubes, int greenCubes, int blueCubes)
    {
        int result = 0;

        foreach (var game in ReadFile(filename))
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
                        case not null when cube.EndsWith("red"): redCount += DeriveValue(cube); continue;
                        case not null when cube.EndsWith("green"): greenCount += DeriveValue(cube); continue;
                        case not null when cube.EndsWith("blue"): blueCount += DeriveValue(cube); continue;
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

        foreach (var game in ReadFile(filename))
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
                        case not null when cube.EndsWith("red"): redCount = Math.Max(redCount, DeriveValue(cube)); continue;
                        case not null when cube.EndsWith("green"): greenCount = Math.Max(greenCount, DeriveValue(cube)); continue;
                        case not null when cube.EndsWith("blue"): blueCount = Math.Max(blueCount, DeriveValue(cube)); continue;
                    }
                }
            }

            result += redCount * greenCount * blueCount;
        }

        Assert.Equal(expectedAnswer, result);
        
        static int DeriveValue(string intput) => int.Parse(intput[..intput.IndexOf(' ')]);
    }

    [Theory]
    [InlineData("Day3DevelopmentTesting1.txt", 4361)]
    [InlineData("Day3.txt", 550064)]
    public void Day3_Part1_GearRatios(string filename, int expectedAnswer)
    {
        int result = 0;
        string[] lines = ReadFile(filename).Select(x => x += ".").ToArray();

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

        string[] lines = ReadFile(filename).Select(x => x += ".").ToArray();

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

    [Theory]
    [InlineData("Day4DevelopmentTesting1.txt", 13)] 
    [InlineData("Day4.txt", 18653)]
    public void Day4_Part1_Scratchcards(string filename, int expectedAnswer)
    {
        int result = 0;

        foreach (var gameCard in ReadFile(filename))
        {
            var tmpStr = gameCard[(gameCard.IndexOf(':') + 1)..].Split('|');

            var winningNumbers = tmpStr[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            var gameNumbers = tmpStr[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            int scoreMultiplier = 1, gameScore = 0;
                
            foreach (var x in winningNumbers)
            {
                foreach (var y in gameNumbers)
                {
                    if (y != x) continue;
                    
                    gameScore = scoreMultiplier;
                    scoreMultiplier += scoreMultiplier;
                }
            }
            
            result += gameScore;
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day4DevelopmentTesting2.txt", 30)]
    [InlineData("Day4.txt", 5921508)]
    public void Day4_Part2_Scratchcards(string filename, int expectedAnswer)
    {
        var games = ReadFile(filename).ToArray();
        var result = games.Length;

        for (var i = 0; i < games.Length; i++)
        {
            PlayGameCard(i);
        }

        Assert.Equal(expectedAnswer, result);

        return;

        void PlayGameCard(int index)
        {
            var gameCard = games[index];

            var tmpStr = gameCard[(gameCard.IndexOf(':') + 1)..].Split('|');

            var winningNumbers = tmpStr[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            var gameNumbers = tmpStr[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            var bonusGames = winningNumbers.Sum(x => gameNumbers.Count(y => y == x));

            result += bonusGames;

            for (var i = index + 1; i <= index + bonusGames; i++)
            {
                PlayGameCard(i);
            }
        }
    }
}