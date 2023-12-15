using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day14
{
    [Theory]
    [InlineData("Day14DevelopmentTesting1.txt", 136)]
    [InlineData("Day14.txt", 102497)]
    public void Day14_Part1_ParabolicReflectorDish(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToArray();
        char[,] arr = new char[input[0].Length, input.Length];

        // Convert the input file into a 2D char array.
        for (int i = 0; i < input.Length; i++)
        {
            var line = input[i];

            for (int j = 0; j < line.Length; j++)
            {
                arr[i, j] = line[j];
            }
        }

        // Iterate across all columns, and down each column, repeating the column until we no longer sort any rocks.
        for (int j = 0; j < arr.GetLength(1); j++)
        {
            bool columnIsSorted;

            do
            {
                columnIsSorted = true;

                // Look down the column...
                for (int k = 0; k < arr.GetLength(0) - 1; k++)
                {
                    if (arr[k + 1, j] == 'O' && arr[k, j] == '.')
                    {
                        arr[k, j] = 'O';
                        arr[k + 1, j] = '.';
                        columnIsSorted = false;
                    }
                }
            } while (!columnIsSorted);
        }

        // Count the rocks in each row and apply their 'weight' calculation.
        int result = 0, arrayLength = arr.GetLength(0);

        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                if (arr[i, j] != 'O') continue;

                result += arrayLength - i;
            }
        }

        Assert.Equal(expectedAnswer, result);
    }
}
