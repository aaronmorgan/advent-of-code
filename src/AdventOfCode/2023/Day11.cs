using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day11
{
    [Theory]
    [InlineData("Day11DevelopmentTesting1.txt", 374)]
    [InlineData("Day11.txt", 10289334)]
    public void Day11_Part1_CosmicExpansion(string filename, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToList();

        List<int> clearSpaceRows = [], clearSpaceColumns = [];

        for (var columnIndex = 0; columnIndex < fileInput[0].Length; columnIndex++)
        {
            clearSpaceColumns.Add(columnIndex);
        }

        // Calculate rows and columns with no galaxies present.
        for (var rowIndex = 0; rowIndex < fileInput.Count; rowIndex++)
        {
            var row = fileInput[rowIndex].ToCharArray();
            bool rowIsClearSpace = true;

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == '.') continue;

                rowIsClearSpace = false;
                clearSpaceColumns.Remove(i);
            }

            if (rowIsClearSpace) clearSpaceRows.Add(rowIndex);
        }

        // Expand clear space rows and columns (in reverse).
        for (int i = clearSpaceRows.Count - 1; i >= 0; i--)
        {
            fileInput.Insert(clearSpaceRows[i], new string('.', fileInput[clearSpaceRows[i]].Length));
        }

        for (int i = clearSpaceColumns.Count - 1; i >= 0; i--)
        {
            for (var index = 0; index < fileInput.Count; index++)
            {
                fileInput[index] = fileInput[index].Insert(clearSpaceColumns[i], ".");
            }
        }

        // Plot galaxy locations.
        int galaxyIndex = 1;
        var galaxyCoordinates = new Dictionary<int, (int X, int Y)>();

        for (var rowIndex = 0; rowIndex < fileInput.Count; rowIndex++)
        {
            var row = fileInput[rowIndex].ToCharArray();

            for (int i = 0; i < row.Length; i++)
            {
                if (row[i] == '.') continue;

                galaxyCoordinates.Add(galaxyIndex, (X: i, Y: rowIndex));
                galaxyIndex++;
            }
        }

        // Identify unique galaxy pairs.
        var galaxyPairs = GetCombinations(galaxyCoordinates.Keys.ToArray());

        // Calculate shortest distance between each pair.
        var result = 0;
        var hops = 0;

        foreach (var pair in galaxyPairs)
        {
            PlotNextCursorPosition(galaxyCoordinates[pair[1]], (galaxyCoordinates[pair[0]].X, galaxyCoordinates[pair[0]].Y));

            result += hops;
            hops = 0;
        }

        Assert.Equal(expectedAnswer, result);

        return;

        void PlotNextCursorPosition((int X, int Y) galaxyB, (int X, int Y) cursorPos)
        {
            hops += 1;

            if (galaxyB.Y < cursorPos.Y)
            {
                cursorPos.Y--;
            }
            else if (galaxyB.X < cursorPos.X)
            {
                cursorPos.X--;
            }
            else if (galaxyB.Y > cursorPos.Y)
            {
                cursorPos.Y++;
            }
            else if (galaxyB.X > cursorPos.X)
            {
                cursorPos.X++;
            }

            if (cursorPos.X == galaxyB.X && cursorPos.Y == galaxyB.Y) return;

            PlotNextCursorPosition(galaxyB, cursorPos);
        }

        static IEnumerable<List<int>> GetCombinations(int[] array)
        {
            List<List<int>> result = [];
            GeneratePairs(array, 0, new List<int>(), result);
            return result;
        }

        static void GeneratePairs(IReadOnlyList<int> array, int index, IList<int> currentPair, ICollection<List<int>> result)
        {
            if (currentPair.Count == 2)
            {
                result.Add([..currentPair]);
                return;
            }

            for (int i = index; i < array.Count; i++)
            {
                currentPair.Add(array[i]);
                GeneratePairs(array, i + 1, currentPair, result);
                currentPair.RemoveAt(currentPair.Count - 1);
            }
        }
    }
}
