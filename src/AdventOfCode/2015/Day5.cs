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

    [Theory]
    [InlineData("qjhvhtzxzqqjkmpb", 1)]
    [InlineData("xxyxx", 1)]
    [InlineData("uurcxstgmygtbstg", 0)]
    [InlineData("ieodomkazucvgmuy", 0)]
    [InlineData("Day5.txt", 55)]
    public void Day5_Part2_DoesntHeHaveInternElvesForThis(string filename, int expectedAnswer)
    {
        var input = filename.StartsWith("Day5")
            ? FileLoader.ReadAllLines("2015/" + filename).ToArray()
            : [filename];

        var naughtyStrings = 0;

        foreach (var line in input)
        {
            if (!ContainsPairsOfStrings(line))
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

        // A 'nice' string contains a pair of any two letters that appears at least twice in the string
        // without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
        bool ContainsPairsOfStrings(string str)
        {
            List<string> stringPairs = [];

            // Build a list of string pairs.
            for (var index = 0; index < str.Length; index++)
            {
                if (index == str.Length - 1) continue;

                var chars = new[] { str[index], str[index + 1] };
                var stringPair = new string(chars);

                if (stringPairs.Contains(stringPair) && stringPairs.IndexOf(stringPair) < index - 1)
                {
                    return true;
                }

                stringPairs.Add(stringPair);
            }

            return false;
        }

        // True if the string contains at least one letter which repeats with exactly one letter between
        // them, like xyx, abcdefeghi (efe), or even aaa.
        bool CheckForDoubleChars(string str)
        {
            char a = str[0];
            char b = str[1];

            for (var index = 2; index < str.Length; index++)
            {
                var c = str[index];

                if (c == a) return true;

                a = b;
                b = c;
            }

            return false;
        }
    }
}