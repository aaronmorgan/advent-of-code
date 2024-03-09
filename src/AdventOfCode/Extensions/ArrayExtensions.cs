﻿namespace AdventOfCode.Extensions;

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
            var line = string.Empty;

            for (var j = 0; j < array.GetLength(1); j++)
            {
                line += array[i, j];
            }

            sw.WriteLine(line);
        }
    }

    /// <summary>
    /// Uniformly pre-fills a 2d array with the specified character, or a space if not defined. 
    /// </summary>
    public static void Prefill(this char[,] array, char character = ' ')
    {
        for (var i = 0; i < array.GetLength(0); i++)
        {
            for (var j = 0; j < array.GetLength(1); j++)
            {
                array[i, j] = character;
            }
        }
    }
}
