﻿using Xunit.Abstractions;

namespace AdventOfCode._2023;

public class Day5 : TestBase
{
    /// <inheritdoc />
    public Day5(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory]
    [InlineData("Day5DevelopmentTesting1.txt", 35)]
    [InlineData("Day5.txt", 88151870)]
    public void Day5_Part1_IfYouGiveASeedAFertilizer(string filename, int expectedAnswer)
    {
        var seedAlmanac = ReadFile(filename).ToArray();
        List<Map> mappingDictionaries = new();

        for (var i = 1; i <= seedAlmanac.Length; i++)
        {
            if (!seedAlmanac[i].EndsWith(':')) continue;

            List<string> lines = new();
            i++;

            while (i < seedAlmanac.Length && !string.IsNullOrEmpty(seedAlmanac[i]))
            {
                lines.Add(seedAlmanac[i]);
                i++;
            }

            mappingDictionaries.Add(CreateMappings(lines));
        }

        var seeds = seedAlmanac[0].Split(' ')[1..].Select(x => long.Parse(x.Trim())).ToArray();

        var lowestLocation = seeds
            .Select(seed => SearchDictionaries(0, seed))
            .Prepend(long.MaxValue).Min();

        Assert.Equal(expectedAnswer, lowestLocation);

        return;

        long SearchDictionaries(int dictionaryIndex, long seedId)
        {
            if (dictionaryIndex >= mappingDictionaries.Count) return seedId;

            foreach (var a in mappingDictionaries[dictionaryIndex].SoureRangeMappings)
            {
                if (seedId < a.Key || seedId > a.Key + a.Value -1) continue;

                var nextSeedId = mappingDictionaries[dictionaryIndex].DestinationRangeMappings[a.Key] + (seedId - a.Key);

                return SearchDictionaries(dictionaryIndex + 1, nextSeedId);
            }

            return SearchDictionaries(dictionaryIndex + 1, seedId);
        }
    }

    [Theory]
    [InlineData("Day5DevelopmentTesting1.txt", 46)]
    [InlineData("Day5.txt", 2008786)] // 13286163, 2008786 too high
    public void Day5_Part2_IfYouGiveASeedAFertilizer(string filename, int expectedAnswer)
    {
        var seedAlmanac = ReadFile(filename).ToArray();
        List<Map> mappingDictionaries = new();

        for (var i = 1; i <= seedAlmanac.Length; i++)
        {
            if (!seedAlmanac[i].EndsWith(':')) continue;

            List<string> lines = new();
            i++;

            while (i < seedAlmanac.Length && !string.IsNullOrEmpty(seedAlmanac[i]))
            {
                lines.Add(seedAlmanac[i]);
                i++;
            }

            mappingDictionaries.Add(CreateMappings(lines));
        }

        var seeds = seedAlmanac[0].Split(' ')[1..].Select(x => long.Parse(x.Trim())).ToArray();
        var lowestLocation = long.MaxValue;

        for (long i = 0; i < seeds.Length; i += 2)
        {
            Console.WriteLine($"Processing seed range: {i}, {seeds[i]}");

            for (var j = seeds[i]; j < seeds[i] + seeds[i + 1]; j++)
            {
                lowestLocation = Math.Min(lowestLocation, SearchDictionaries(0, j));
            }
        }

        Assert.Equal(expectedAnswer, lowestLocation);

        return;

        long SearchDictionaries(int dictionaryIndex, long seedId)
        {
            if (dictionaryIndex >= mappingDictionaries.Count) return seedId;

            foreach (var a in mappingDictionaries[dictionaryIndex].SoureRangeMappings)
            {
                if (seedId < a.Key || seedId > a.Key + a.Value - 1) continue;

                var nextSeedId = mappingDictionaries[dictionaryIndex].DestinationRangeMappings[a.Key] + (seedId - a.Key);

                return SearchDictionaries(dictionaryIndex + 1, nextSeedId);
            }

            return SearchDictionaries(dictionaryIndex + 1, seedId);
        }
    }

    private static Map CreateMappings(List<string> lines)
    {
        var mappingDictionary = new Map
        {
            SoureRangeMappings = new(),
            DestinationRangeMappings = new()
        };

        foreach (var line in lines)
        {
            var inputs = line.Split(' ').Select(x => long.Parse(x.Trim())).ToArray();

            var sourceRangeStart = inputs[1];
            var destinationRangeStart = inputs[0];
            var rangeLength = inputs[2];

            mappingDictionary.SoureRangeMappings.Add(sourceRangeStart, rangeLength);
            mappingDictionary.DestinationRangeMappings.Add(sourceRangeStart, destinationRangeStart);
        }

        return mappingDictionary;
    }

    private class Map
    {
        public Dictionary<long, long> SoureRangeMappings;
        public Dictionary<long, long> DestinationRangeMappings;
    }
}
