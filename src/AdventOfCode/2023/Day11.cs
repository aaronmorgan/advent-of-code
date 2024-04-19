using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day11
{
    /// <summary>
    /// Part 1 & 2: Can be done together if we pass in an 'empty space multiplier', Simply allow for this when calculating the shortest distance using
    /// the Manhattan distance algorithm. The empty space multipler means we avoid plotting the full size of the galaxy.
    /// </summary>
    [Theory]
    [InlineData("Day11DevelopmentTesting1.txt", 2, 374)]
    [InlineData("Day11DevelopmentTesting1.txt", 10, 1030)]
    [InlineData("Day11DevelopmentTesting1.txt", 100, 8410)]
    [InlineData("Day11.txt", 2, 10289334)] // Part 1
    [InlineData("Day11.txt", 1000000, 649862989626)] // Part 2
    public void Day11_Part1And2_CosmicExpansion(string filename, int distanceMultiplier, long expectedAnswer)
    {
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToArray();

        // Using a jagged array because it's easier to do the replacement of markers for 'empty' rows and columns.
        char[][] arr = new char[fileInput.Length][];

        List<int> clearSpaceRows = [], clearSpaceColumns = [];

        for (var columnIndex = 0; columnIndex < fileInput[0].Length; columnIndex++)
        {
            clearSpaceColumns.Add(columnIndex);
        }

        // Convert the input file into a 2D char array and determine rows and columns with no galaxies present.
        for (var row = 0; row < arr.Length; row++)
        {
            var rowIsClearSpace = true;

            arr[row] = new char[fileInput[row].Length];

            for (var col = 0; col < fileInput[row].Length; col++)
            {
                arr[row][col] = fileInput[row][col];

                if (fileInput[row][col] == '.') continue;

                rowIsClearSpace = false;
                clearSpaceColumns.Remove(col);
            }

            if (rowIsClearSpace) clearSpaceRows.Add(row);
        }

        // Replace all empty rows with fiducial markers.
        var emptyRowMarkers = new char[arr.GetLength(0)];
        Array.Fill(emptyRowMarkers, '+');

        foreach (var y in clearSpaceRows)
        {
            arr[y] = emptyRowMarkers;
        }

        // Replace all empty columns with fiducial markers.
        foreach (var x in clearSpaceColumns)
        {
            foreach (var row in arr)
            {
                row[x] = '+';
            }
        }

        //   arr.WriteToFile("C:/temp/day11.txt");

        // Plot galaxy locations.
        var galaxyIndex = 1;
        var galaxyCoordinates = new Dictionary<int, (int X, int Y)>();

        for (var rowIndex = 0; rowIndex < arr.Length; rowIndex++)
        {
            var row = arr[rowIndex];

            for (var i = 0; i < row.Length; i++)
            {
                if (row[i] is '.' or '+') continue;

                galaxyCoordinates.Add(galaxyIndex, (X: i, Y: rowIndex));
                galaxyIndex++;
            }
        }

        // Identify unique galaxy pairs.
        var galaxyPairs = GetCombinations(galaxyCoordinates.Keys.ToArray());

        // Calculate shortest distance between each pair.
        long result = 0;

        foreach (var pair in galaxyPairs)
        {
            result += PlotNextCursorPosition(galaxyCoordinates[pair[1]],
                (galaxyCoordinates[pair[0]].X, galaxyCoordinates[pair[0]].Y));
        }

        Assert.Equal(expectedAnswer, result);

        return;

        int PlotNextCursorPosition((int X, int Y) galaxyB, (int X, int Y) cursorPos)
        {
            var distance = 0;
            while (true)
            {
                distance += arr[cursorPos.Y][cursorPos.X] == '+' ? distanceMultiplier : 1;

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

                if (cursorPos.X == galaxyB.X && cursorPos.Y == galaxyB.Y) return distance;
            }
        }

        static IEnumerable<List<int>> GetCombinations(int[] array)
        {
            List<List<int>> result = [];
            GeneratePairs(array, 0, new List<int>(), result);
            return result;
        }

        static void GeneratePairs(IReadOnlyList<int> array, int index, IList<int> currentPair,
            ICollection<List<int>> result)
        {
            if (currentPair.Count == 2)
            {
                result.Add([..currentPair]);
                return;
            }

            for (var i = index; i < array.Count; i++)
            {
                currentPair.Add(array[i]);
                GeneratePairs(array, i + 1, currentPair, result);
                currentPair.RemoveAt(currentPair.Count - 1);
            }
        }
    }
}