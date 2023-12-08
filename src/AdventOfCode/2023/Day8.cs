using System.Collections.Concurrent;
using System.Diagnostics;
using System.Timers;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day8
{
    [Theory]
    [InlineData("Day8DevelopmentTesting1.txt", "AAA", 2)]
    [InlineData("Day8DevelopmentTesting2.txt", "AAA", 6)]
    [InlineData("Day8.txt", "AAA", 21797)]
    public void Day8_Part1_CamelCards(string filename, string nodeId, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadFile("2023/" + filename).ToArray();
        Dictionary<string, Tuple<string, string>> nodes = new();

        for (var i = 2; i < fileInput.Length; i++)
        {
            var nodeDetails = fileInput[i].Split(' ').Select(x => x.Trim('(', ')', ',')).ToArray();

            nodes.Add(nodeDetails[0], new Tuple<string, string>(nodeDetails[2], nodeDetails[3]));
        }

        var stepInstructions = fileInput[0].ToCharArray();
        var nodeIndex = 1;
        var stepIndex = 0;

        do
        {
            var node = nodes[nodeId];
            var direction = stepInstructions[stepIndex];

            stepIndex = stepIndex == stepInstructions.Length - 1 ? 0 : stepIndex + 1;

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
    [InlineData("Day8.txt", 7)]
    public void Day8_Part2_CamelCards(string filename, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadFile("2023/" + filename).ToArray();
        Dictionary<string, Tuple<string, string>> nodes = new();
        List<string> startingNodes = new();

        for (var i = 2; i < fileInput.Length; i++)
        {
            var nodeDetails = fileInput[i].Split(' ').Select(x => x.Trim('(', ')', ',')).ToArray();

            if (nodeDetails[0].EndsWith('A'))
            {
                startingNodes.Add(nodeDetails[0]);
            }

            nodes.Add(nodeDetails[0], new Tuple<string, string>(nodeDetails[2], nodeDetails[3]));
        }

        var stepInstructions = fileInput[0].ToCharArray();
        var nodeIndex = 1;
        var stepIndex = 0;
        ConcurrentDictionary<int, bool> withz = new();
        object _lock = new object();

var endsWithZ = false;

        {
            do
            {
                var direction = stepInstructions[stepIndex];

         //   Parallel.ForEach(startingNodes, new ParallelOptions { MaxDegreeOfParallelism = Math.Min(6, startingNodes.Count) }, (nodeId, state) =>
         Parallel.For(0, startingNodes.Count, index =>
        //        for (var index = 0; index < startingNodes.Count; index ++)
                {
                    var nodeId = startingNodes[index];
                    var n = nodes[nodeId];


                    nodeId = direction switch
                    {
                        'L' => n.Item1, 'R' => n.Item2, _ => throw new NotImplementedException()
                    };

                    endsWithZ = nodeId.EndsWith("Z");
                    withz.AddOrUpdate(index, endsWithZ, (k, v) => endsWithZ);
                    startingNodes[index] = nodeId;
                });

                stepIndex = stepIndex == stepInstructions.Length - 1 ? 0 : stepIndex + 1;

                if (endsWithZ && areSame()) break;
                nodeIndex++;

                for (int i = 0; i < startingNodes.Count; i++)
                {
                    Console.SetCursorPosition(2, i);
                    Console.Write(withz[i]);
                }
            } while (true);
        }

        Assert.Equal(expectedAnswer, nodeIndex);

        return;

        bool areSame()
        {
            //    if (withz.Count < startingNodes.Count) return false;

            foreach (var i in withz)
            {
                if (i.Value == false) return false;
            }

            return true;
        }
    }
}
