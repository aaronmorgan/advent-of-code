using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day9
{
    [Theory]
    [InlineData("Day9DevelopmentTesting1.txt", 114)]
    [InlineData("Day9.txt", 2101499000)]
    public void Day9_Part1_MirageMaintenance(string filename, int expectedAnswer)
    {
        long result = 0;
        var fileInput = FileLoader.ReadFile("2023/" + filename).ToArray();

        List<int[]> histories = new();

        foreach (var line in fileInput)
        {
            histories.Add(line.Split(' ').Select(int.Parse).ToArray());

            ProcessRow(histories[0]);

            long x = 0;

            for (var  historyIndex = histories.Count - 1; historyIndex >= 0; historyIndex--)
            {
                x += histories[historyIndex - 1][histories[historyIndex - 1].Length - 1];

                if (historyIndex != 1) continue;

                result += x;
                break;
            }

            histories.Clear();
        }

        Assert.Equal(expectedAnswer, result);

        return;

        void ProcessRow(int[] x)
        {
            int[] y = new int[x.Length - 1];

            for (int i = 0; i < x.Length - 1; i++)
            {
                y[i] = x[i + 1] - x[i];
            }

            histories.Add(y);

            if (AreAllZeros(y)) return;

            ProcessRow(y);
        }

        static bool AreAllZeros(int[] array) => array.All(i => i == 0);
    }
}
