using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day13
{
    [Theory]
    [InlineData("Day13DevelopmentTesting1.txt", 405)]
    [InlineData("Day13.txt", 33000)] // 33000 too low.
    public void Day13_Part1_PointOfIncidence(string filename, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadFile("2023/" + filename).ToList();

        int aaa = 0, overallResult = 0;
        List<string> columns;
        bool isRow;

        for (int i = 0; i <= fileInput.Count; i++)
        {
            isRow = false;
            
            if (i == fileInput.Count || fileInput[i] == string.Empty)
            {
                var a = fileInput.GetRange(aaa, i - aaa);
                ProcessPattern(a);
                i++;
                aaa = i;
            }
        }

        Assert.Equal(expectedAnswer, overallResult);
        
        return;

        void ProcessPattern(List<string> inputPattern)
        {
            int result = 0;

            // Look for mirror rows...
            for (int i = 0; i < inputPattern.Count -1; i++)
            {
                if (ExpandSearchOutwards(i, i + 1, inputPattern))
                {
                    result += (i + 1)  * 100;
                    isRow = true;
                    break;
                }
            }

            // Look for mirror columns...
            if (!isRow)
            {
                columns = new(inputPattern.Count);

                for (int rowIndex = 0; rowIndex < inputPattern[0].Length; rowIndex++)
                {
                    var tmpStr = string.Empty;
                    
                    foreach (var row in inputPattern)
                    {
                        tmpStr += row[rowIndex].ToString();
                    }

                    columns.Add(tmpStr);
                }

                aaa = -1;

                for (int i = 0; i < columns.Count - 1; i++)
                {
                    if (ExpandSearchOutwards(i, i + 1, columns))
                    {
                        result += i + 1;
                        break;
                    }
                }
            }

            overallResult += result;
        }

        bool ExpandSearchOutwards(int x, int y, IReadOnlyList<string> data)
        {
            if (data[x] != data[y]) return false;
            
            // We found a perfect series of reflections up to the pattern boundary.
            if (x == 0 || y == data.Count - 1) return true;

            return ExpandSearchOutwards(x - 1, y + 1, data);
        }
    }
}
