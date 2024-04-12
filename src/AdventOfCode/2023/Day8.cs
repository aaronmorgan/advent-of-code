using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day8
{
    [Theory]
    [InlineData("Day8DevelopmentTesting1.txt", "AAA", 2)]
    [InlineData("Day8DevelopmentTesting2.txt", "AAA", 6)]
    [InlineData("Day8.txt", "AAA", 21797)]
    public void Day8_Part1_HauntedWasteland(string filename, string nodeId, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToArray();

        var steps = fileInput[0].ToCharArray();
        Dictionary<string, (string Left, string Right)> nodes = fileInput.Skip(2).ToDictionary(k => k[..3], v => (v[7..10], v[^4..^1]));

        var nodeIndex = 1;
        var stepIndex = 0;

        do
        {
            var node = nodes[nodeId];
            var direction = steps[stepIndex];

            stepIndex = stepIndex == steps.Length - 1 ? 0 : stepIndex + 1;

            nodeId = direction switch
            {
                'L' => node.Item1, 'R' => node.Item2, _ => nodeId
            };

            if (nodeId == "ZZZ")
            {
                break;
            }

            nodeIndex++;
        } while (true);

        Assert.Equal(expectedAnswer, nodeIndex);
    }

    [Theory]
    [InlineData("Day8DevelopmentTesting3.txt", 6)]
    [InlineData("Day8.txt", 23977527174353)]
    public void Day8_Part2_HauntedWasteland(string filename, long expectedAnswer)
    {
        var fileInput = FileLoader.ReadAllLines("2023/" + filename).ToArray();
        var steps = fileInput[0].ToCharArray();

        Dictionary<string, (string Left, string Right)> nodes = fileInput.Skip(2).ToDictionary(k => k[..3], v => (v[7..10], v[^4..^1]));

        List<long> ZNumbers = new();

        foreach (var node in nodes.Where(a => a.Key[2] == 'A'))
        {
            GetSteps(node.Key);
        }

        var result = ZNumbers.Skip(1).Aggregate(ZNumbers[0], (current, number) => lcm(current, number));

        Assert.Equal(expectedAnswer, result);

        return;

        void GetSteps(string node)
        {
            string currentNode = node;

            var found = false;
            var currentZ = 0;

            while (!found)
            {
                foreach (var c in steps)
                {
                    currentNode = c == 'L' ? nodes[currentNode].Item1 : nodes[currentNode].Item2;
                    currentZ++;

                    if (currentNode[2] != 'Z') continue;

                    ZNumbers.Add(currentZ);
                    found = true;
                    break;
                }
            }
        }

        long gcf(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        long lcm(long a, long b)
        {
            return a / gcf(a, b) * b;
        }
    }
}
