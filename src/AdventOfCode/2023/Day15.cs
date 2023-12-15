using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day15
{
    [Theory]
    [InlineData("Day15DevelopmentTesting1.txt", 52)]
    [InlineData("Day15DevelopmentTesting2.txt", 1320)]
    [InlineData("Day15.txt", 497373)]
    public void Day15_Part1_LensLibrary(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllText("2023/" + filename).Split(',');
        
        int result = 0;
        
        foreach(var i in input)
        {
            int tmpResult = 0;
            var a = i.ToCharArray();
            
            for (int j = 0; j < a.Length; j++)
            {
                int  b = a[j] - 0;
                
                tmpResult = (tmpResult + b) * 17;
                
                tmpResult %= 256;
            }
            
            result += tmpResult;
        }
        
        Assert.Equal(expectedAnswer, result);
    }
}
