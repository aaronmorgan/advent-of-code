using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day7
{
    [Theory]
    [InlineData("Day7DevelopmentTesting1.txt", 6440)]
    [InlineData("Day7.txt", 247961593)]
    public void Day7_Part1_CamelCards(string filename, int expectedAnswer)
    {
        var result = 0;
        var fileInput = FileLoader.ReadFile("2023/" + filename).ToArray();

        SortedDictionary<string, int> HighCardHands = new();
        SortedDictionary<string, int> OnePairHands = new();
        SortedDictionary<string, int> TwoPairHands = new();
        SortedDictionary<string, int> FullHouseHands = new();
        SortedDictionary<string, int> ThreeOfAKindHands = new();
        SortedDictionary<string, int> FourOfAKindHands = new();
        SortedDictionary<string, int> FiveOfAKindHands = new();

        foreach (var game in fileInput)
        {
            var gameData = game.Split(' ');
            gameData[0] = string.Concat(RemapGameCards(gameData[0]));

            var groupedByCount = gameData[0]
                .GroupBy(c => c)
                .GroupBy(g => g.Count(), g => g.Key)
                .OrderByDescending(g => g.Key)
                .ToArray();

            if (groupedByCount is [{ Key: 5 }]) { AddGameToDictionary(FiveOfAKindHands, gameData); continue; }
            if (groupedByCount is [{ Key: 4 }, _]) { AddGameToDictionary(FourOfAKindHands, gameData); continue; }
            if (groupedByCount is [{ Key: 3 }, { Key: 2 }]) { AddGameToDictionary(FullHouseHands, gameData); continue; }
            if (groupedByCount is [{ Key: 3 }, _]) { AddGameToDictionary(ThreeOfAKindHands, gameData); continue; }
            if (groupedByCount is [{ Key: 2 }, _] && groupedByCount[0].Count() == 2) { AddGameToDictionary(TwoPairHands, gameData); continue; }
            if (groupedByCount is [{ Key: 2 }, { Key: 1 }]) { AddGameToDictionary(OnePairHands, gameData); continue; }
            if (groupedByCount[0].Key == 1 && groupedByCount[0].Count() == 5) { AddGameToDictionary(HighCardHands, gameData); }
        }

        var i = 0;

        CalculateHandWinnings(HighCardHands);
        CalculateHandWinnings(OnePairHands);
        CalculateHandWinnings(TwoPairHands);
        CalculateHandWinnings(ThreeOfAKindHands);
        CalculateHandWinnings(FullHouseHands);
        CalculateHandWinnings(FourOfAKindHands);
        CalculateHandWinnings(FiveOfAKindHands);

        Assert.Equal(expectedAnswer, result);

        return;

        // Remap the game cards into characters that naturally sort.
        // Map A=>E before doing T=A or we map A from T=>A=>E.
        string RemapGameCards(string hand) => hand.Replace('A', 'E').Replace('T', 'A').Replace('J', 'B').Replace('Q', 'C').Replace('K', 'D');

        void AddGameToDictionary(IDictionary<string, int> dictionary, string[] gameData)
        {
            dictionary.Add(gameData[0], int.Parse(gameData[1]));
        }

        // Order the dictionary by 'bet' size then iterate each entry and multiple the bid by the winning hand's rank.
        void CalculateHandWinnings(SortedDictionary<string, int> dictionary)
        {
            foreach (var winningHand in dictionary)
            {
                i++;
                result += winningHand.Value * i;
            }
        }
    }
}
