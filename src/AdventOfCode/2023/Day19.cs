using AdventOfCode.Utilities;
using InvalidOperationException = System.InvalidOperationException;

namespace AdventOfCode._2023;

public class Day19
{
    [Theory]
    [InlineData("Day19DevelopmentTesting1.txt", 19114)]
    [InlineData("Day19.txt", 397061)]
    public void Day19_Part1_Aplenty(string filename, int expectedAnswer)
    {
        var input = FileLoader.ReadAllLines("2023/" + filename).ToList();

        Dictionary<string, Workflow> workflowsDict = new();
        List<Rating> ratings = new();

        for (var index = 0; index < input.Count; index++)
        {
            var line = input[index];

            // Break down each workflow into it's steps and store keyed by it's name.
            if (!string.IsNullOrEmpty(line))
            {
                var charIndex = line.IndexOf('{');
                var name = line[..charIndex];
                var steps = line
                    .Substring(charIndex + 1, line.Length - 2 - charIndex)
                    .Split(',');

                var workflow = new Workflow
                {
                    Name = name,
                    WorkflowSteps = new WorkflowStep[steps.Length]
                };

                workflowsDict.Add(workflow.Name, workflow);

                for (int i = 0; i < steps.Length; i++)
                {
                    workflow.WorkflowSteps[i] = Workflow.Create(steps[i]);
                }

                continue;
            }

            // We've hit the line break, the rest of the input are the ratings objects.
            CreateRatings(input.GetRange(index + 1, input.Count - index - 1));

            break;
        }

        int result = 0;

        foreach (var rating in ratings)
        {
            ProcessRatingsWorkflow(rating, workflowsDict["in"]);
        }

        Assert.Equal(expectedAnswer, result);

        return;

        int SumRating(Rating rating) => rating.s + rating.m + rating.a + rating.x;

        void ProcessRatingsWorkflow(Rating rating, Workflow workflow)
        {
            foreach (var workflowStep in workflow.WorkflowSteps)
            {
                if (string.IsNullOrEmpty(workflowStep.RuleBody))
                {
                    if (workflowStep.NextStep == "R") return;
                    if (workflowStep.NextStep == "A")
                    {
                        result += SumRating(rating);
                        return;
                    }

                    ProcessRatingsWorkflow(rating, workflowsDict[workflowStep.NextStep]);
                    return;
                }

                var ratingKey = workflowStep.RuleBody[..1];
                var operation = workflowStep.RuleBody.Substring(1, 1);

                int value = ratingKey switch
                {
                    "x" => rating.x,
                    "m" => rating.m,
                    "a" => rating.a,
                    "s" => rating.s,
                    _ => throw new InvalidOperationException()
                };

                var comparisonValue = int.Parse(workflowStep.RuleBody[2..]);

                switch (operation)
                {
                    case ">" when value > comparisonValue:
                    {
                        if (workflowStep.NextStep == "R") return;
                        if (workflowStep.NextStep == "A")
                        {
                            result += SumRating(rating);
                            return;
                        }

                        ProcessRatingsWorkflow(rating, workflowsDict[workflowStep.NextStep]);
                        return;
                    }

                    case "<":
                    {
                        if (value < comparisonValue)
                        {
                            if (workflowStep.NextStep == "R") return;
                            if (workflowStep.NextStep == "A")
                            {
                                result += SumRating(rating);
                                return;
                            }

                            ProcessRatingsWorkflow(rating, workflowsDict[workflowStep.NextStep]);
                            return;
                        }

                        break;
                    }
                }
            }
        }

        void CreateRatings(List<string> lines)
        {
            foreach (var line in lines)
            {
                var partRatings = line.Trim('}', '{').Split(',');

                var x = int.Parse(partRatings[0][2..]);
                var m = int.Parse(partRatings[1][2..]);
                var a = int.Parse(partRatings[2][2..]);
                var s = int.Parse(partRatings[3][2..]);

                ratings.Add(new Rating(x, m, a, s));
            }
        }
    }

    private record struct Rating(int x, int m, int a, int s);

    private struct Workflow
    {
        public string Name;
        public WorkflowStep[] WorkflowSteps;

        public static WorkflowStep Create(string step)
        {
            var condition = step.Split(':');

            if (condition.Length == 1) return new WorkflowStep { NextStep = condition[0] };

            return new WorkflowStep
            {
                RuleBody = condition[0],
                NextStep = condition[1]
            };
        }
    }

    private class WorkflowStep
    {
        public string RuleBody;
        public string NextStep;
    }
}
