using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Utilities;

namespace AdventOfCode._2015;

public class Day5
{
    [Theory]
    [InlineData("ugknbfddgicrmopn", 1)]
    [InlineData("aaa", 1)]
    [InlineData("jchzalrnumimnmhp", 0)]
    [InlineData("haegwjzuvuyypxyu", 0)]
    [InlineData("dvszwmarrgswjxmb", 0)]
    [InlineData("Day5.txt", 255)]
    public void Day5_Part1_DoesntHeHaveInternElvesForThis(string filename, int expectedAnswer)
    {
        var input = filename.StartsWith("Day5")
            ? FileLoader.ReadAllLines("2015/" + filename).ToArray()
            : [filename];

        var naughtyStrings = 0;

        foreach (var line in input)
        {
            if (ContainsStrings(line))
            {
                naughtyStrings += 1;
                continue;
            }

            if (!CountVowels(line))
            {
                naughtyStrings += 1;
                continue;
            }

            if (!CheckForDoubleChars(line))
            {
                naughtyStrings += 1;
            }
        }

        Assert.Equal(expectedAnswer, input.Length - naughtyStrings);

        return;

        // A 'nice' string does not contain the strings ab, cd, pq, or xy,
        // even if they are part of one of the other requirements.
        bool ContainsStrings(string str)
        {
            var matchFound = str.Contains("ab") ||
                             str.Contains("cd") ||
                             str.Contains("pq") ||
                             str.Contains("xy");

            return matchFound;
        }

        // Returns true if the input string contains 3 or more vowels.
        bool CountVowels(string str)
        {
            int count = 0;

            foreach (char c in str)
            {
                switch (c)
                {
                    case 'a': count += 1; continue;
                    case 'e': count += 1; continue;
                    case 'i': count += 1; continue;
                    case 'o': count += 1; continue;
                    case 'u': count += 1; break;
                }
            }

            return count > 2;
        }

        bool CheckForDoubleChars(string str)
        {
            char a = str[0];

            for (var index = 1; index < str.Length; index++)
            {
                var c = str[index];

                if (c == a) return true;

                a = c;
            }

            return false;
        }
    }
}