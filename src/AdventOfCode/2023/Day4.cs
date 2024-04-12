using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day4
{
    [Theory]
    [InlineData("Day4DevelopmentTesting1.txt", 13)]
    [InlineData("Day4.txt", 18653)]
    public void Day4_Part1_Scratchcards(string filename, int expectedAnswer)
    {
        int result = 0;

        foreach (var gameCard in FileLoader.ReadAllLines("2023/" + filename))
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
    // [InlineData("Day4.txt", 5921508)] // TODO: Takes 50 seconds to complete, can it be reduced?
    public void Day4_Part2_Scratchcards(string filename, int expectedAnswer)
    {
        var games = FileLoader.ReadAllLines("2023/" + filename).ToArray();
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
