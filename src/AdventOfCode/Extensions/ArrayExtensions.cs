namespace AdventOfCode.Extensions;

public static class ArrayExtensions
{
    /// <summary>
    /// Writes a 2d array to file 'line by line'.
    /// </summary>
    public static void WriteToFile(this char[,] array, string filename)
    {
        using var sw = new StreamWriter(filename);
        for (var i = 0; i < array.GetLength(0); i++)
        {
            var  line = string.Empty;

            for (var j = 0; j < array.GetLength(1); j++)
            {
                line += array[i, j];
            }

            sw.WriteLine(line);
        }
    }
}
