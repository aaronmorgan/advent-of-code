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

        foreach (var i in input)
        {
            int tmpResult = 0;
            var a = i.ToCharArray();

            for (int j = 0; j < a.Length; j++)
            {
                // Subtract zero from the char to simply convert it to an integer.
                int b = a[j] - 0;

                tmpResult = (tmpResult + b) * 17;
                tmpResult %= 256;
            }

            result += tmpResult;
        }

        Assert.Equal(expectedAnswer, result);
    }

    [Theory]
    [InlineData("Day15DevelopmentTesting2.txt", 145)]
    [InlineData("Day15.txt", 259356)]
    public void Day15_Part2_LensLibrary(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllText("2023/" + filename).Split(',');
        var boxes = new Box[256];
        
        for(var i = 0; i<boxes.Length; i++)
        {
            boxes[i] = new Box(i, []);
        }

        foreach (var i in input)
        {
            int boxIndex = 0;
            var a = i.ToCharArray();

            for (int j = 0; j < a.Length; j++)
            {
                if (a[j] == '=' || a[j] == '-')
                {
                    var label = new string(a, 0, j);
                    var existingLens = boxes[boxIndex].LensArray.Find(lens => lens.Label == label);

                    if (a[j] == '-')
                    {
                        if (existingLens != null)
                        {
                            boxes[boxIndex].LensArray.Remove(existingLens);
                        }
                    }
                    else
                    {
                        _ = int.TryParse(new string(a[(j + 1)..]), out var focalLength);

                        var lens = new Lens(label, focalLength);

                        // Replace the old lens with the new one...
                        if (existingLens != null)
                        {
                            var existingLensIndex = boxes[boxIndex].LensArray.IndexOf(existingLens);

                            boxes[boxIndex].LensArray[existingLensIndex] = lens;
                        }
                        // Add the lens to the box...
                        else
                        {
                            boxes[boxIndex].LensArray.Add(lens);
                        }
                    }

                    break;
                }

                // Subtract zero from the char to simply convert it to an integer.
                int charAsInt = a[j] - 0;

                boxIndex = (boxIndex + charAsInt) * 17;
                boxIndex %= 256;
            }
        }
    
        var focusingPower = boxes.Sum(SumFocusingPower);

        Assert.Equal(expectedAnswer, focusingPower);
        
        return;
        
        int SumFocusingPower(Box box)
        {
            if (box.LensArray.Count == 0) return 0;
        
            var subTotal = 0;

            for (var index = 0; index < box.LensArray.Count; index++)
            {
                var z = (1 + box.Index) * (index + 1) * box.LensArray[index].FocalLength;
                
                subTotal += z;
            }

            return subTotal;
        }
    }

    private record Lens(string Label, int FocalLength);

    private record Box(int Index, List<Lens> LensArray);
}
