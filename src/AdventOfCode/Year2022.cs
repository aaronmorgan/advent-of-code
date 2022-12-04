namespace AdventOfCode
{
    public class Year2022
    {
        [Theory]
        [InlineData("Day1DevelopmentTesting.txt", 24000)]
        [InlineData("Day1.txt", 71924)]
        public void Day1_Part1_Calorie_Counting(string filename, int expectedAnswer)
        {
            int maxCaloriesCarried = 0;
            int currentCalories = 0;

            foreach (var line in File.ReadLines($"./TestData/{filename}"))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    currentCalories += int.Parse(line);

                    if (currentCalories > maxCaloriesCarried) maxCaloriesCarried = currentCalories;

                    continue;
                }

                currentCalories = 0;
            }

            Assert.Equal(expectedAnswer, maxCaloriesCarried);
        }

        [Theory]
        [InlineData("Day1DevelopmentTesting.txt", 45000)]
        [InlineData("Day1.txt", 210406)]
        public void Day1_Part2_Calorie_Counting(string filename, int expectedAnswer)
        {
            List<int> elfCalories = new();
            int currentCalories = 0;

            foreach (var line in File.ReadLines($"./TestData/{filename}"))
            {
                if (string.IsNullOrEmpty(line))
                {
                    elfCalories.Add(currentCalories);
                    currentCalories = 0;
                    continue;
                }

                currentCalories += int.Parse(line);
            }

            elfCalories.Add(currentCalories);
            elfCalories.Sort();

            Assert.Equal(expectedAnswer, elfCalories.TakeLast(3).Sum());
        }

        [Theory]
        [InlineData("Day2DevelopmentTesting.txt", 15)]
        [InlineData("Day2.txt", 13268)]
        public void Day2_Part1_Rock_Paper_Sissors(string filename, int expectedScore)
        {
            var draws = new[] { "A X", "B Y", "C Z" };
            var wins = new[] { "A Y", "B Z", "C X" };

            int score = 0;

            foreach (var round in File.ReadAllLines($"./TestData/{filename}"))
            {
                var cheatAnswer = round[^1..];

                if (draws.Contains(round))
                {
                    score += 3;
                }
                else if (wins.Contains(round))
                {
                    score += 6;
                }

                score += cheatAnswer switch
                {
                    "X" => 1,
                    "Y" => 2,
                    "Z" => 3
                };
            }

            Assert.Equal(expectedScore, score);
        }

        [Theory]
        [InlineData("Day2DevelopmentTesting.txt", 12)]
        [InlineData("Day2.txt", 15508)]
        public void Day2_Part2_Rock_Paper_Sissors(string filename, int expectedScore)
        {
            var draws = new[] { "A Y", "B Y", "C Y" };
            var wins = new[] { "A Z", "B Z", "C Z" };

            int score = 0;

            foreach (var round in File.ReadAllLines($"./TestData/{filename}"))
            {
                var opponentChoice = round[..1];

                if (draws.Contains(round))
                {
                    score += 3;
                    score += opponentChoice switch
                    {
                        "A" => 1,
                        "B" => 2,
                        "C" => 3
                    };
                }
                else if (wins.Contains(round))
                {
                    score += 6;
                    score += opponentChoice switch
                    {
                        "A" => 2,
                        "B" => 3,
                        "C" => 1
                    };
                }
                else
                {
                    score += opponentChoice switch
                    {
                        "A" => 3,
                        "B" => 1,
                        "C" => 2
                    };
                }
            }

            Assert.Equal(expectedScore, score);
        }

        [Theory]
        [InlineData("Day3DevelopmentTesting.txt", 157)]
        [InlineData("Day3.txt", 7716)]
        public void Day3_Part1_Rucksack_Reorganization(string filename, int expectedPrioritySum)
        {
            int prioritySum = 0;

            foreach (var line in File.ReadLines($"./TestData/{filename}"))
            {
                var compartmentA = line[..(line.Length / 2)].ToArray();
                var compartmentB = line[^(line.Length / 2)..];

                int rucksackMaxValueItem = 0;

                foreach (var c in compartmentA)
                {
                    if (compartmentB.Contains(c))
                    {
                        var ascii = (int)c;
                        var value = (ascii >= 65 && ascii <= 90) ? ascii - 38 : ascii - 96;

                        if (value > rucksackMaxValueItem) { rucksackMaxValueItem = value; }
                    }
                }

                prioritySum += rucksackMaxValueItem;
            }

            Assert.Equal(expectedPrioritySum, prioritySum);
        }

        [Theory]
        [InlineData("Day3DevelopmentTesting.txt", 70)]
        [InlineData("Day3.txt", 2973)]
        public void Day3_Part2_Rucksack_Reorganization(string filename, int expectedPrioritySum)
        {
            int prioritySum = 0;

            var fileContents = File.ReadAllLines($"./TestData/{filename}");

            for (int i = 0; i < fileContents.Length - 2; i += 3)
            {
                var line1 = fileContents[i + 0].ToArray();
                var line2 = fileContents[i + 1];
                var line3 = fileContents[i + 2];

                int rucksackMaxValueItem = 0;

                foreach (var c in line1)
                {
                    if (line2.Contains(c) && line3.Contains(c))
                    {
                        var ascii = (int)c;
                        var value = (ascii >= 65 && ascii <= 90) ? ascii - 38 : ascii - 96;

                        if (value > rucksackMaxValueItem) { rucksackMaxValueItem = value; }
                    }
                }

                prioritySum += rucksackMaxValueItem;
            }

            Assert.Equal(expectedPrioritySum, prioritySum);
        }

        [Theory]
        [InlineData("Day4DevelopmentTesting.txt", 2)]
        [InlineData("Day4.txt", 487)]
        public void Day4_Part1_Camp_Cleanup(string filename, int expectedSumMatchingPairs)
        {
            int sumMatchingPairs = 0;

            foreach (var line in File.ReadLines($"./TestData/{filename}"))
            {
                var elfZones = line.Split(',');

                var elf1ZoneIdsAsString = ConvertShortRangeToLong(elfZones[0]);
                var elf2ZoneIdsAsString = ConvertShortRangeToLong(elfZones[1]);

                if (elf1ZoneIdsAsString.Contains(elf2ZoneIdsAsString)) { sumMatchingPairs++; }
                else if (elf2ZoneIdsAsString.Contains(elf1ZoneIdsAsString)) { sumMatchingPairs++; }
            }

            static string ConvertShortRangeToLong(string zoneIds)
            {
                var zoneParts = zoneIds.Split('-');
                var elf1Start = int.Parse(zoneParts[0]);
                var elf1Finish = int.Parse(zoneParts.Length == 1 ? zoneParts[0] : zoneParts[1]);

                var result = string.Empty;

                for (int i = elf1Start; i <= elf1Finish; i++)
                {
                    result += $"-{i}-";
                }

                return result;
            }

            Assert.Equal(expectedSumMatchingPairs, sumMatchingPairs);
        }

        [Theory]
        [InlineData("Day4DevelopmentTesting.txt", 4)]
        [InlineData("Day4.txt", 849)]
        public void Day4_Part2_Camp_Cleanup(string filename, int expectedSumOverlappingPairs)
        {
            int sumMatchingPairs = 0;

            foreach (var line in File.ReadLines($"./TestData/{filename}"))
            {
                var elfZones = line.Split(',');

                var (elf1FirstZone, elf1LastZone) = ConvertShortRangeToLong(elfZones[0]);
                var (elf2FirstZone, elf2LastZone) = ConvertShortRangeToLong(elfZones[1]);

                // Collection 1's first element is within collection 2
                if (elf1FirstZone >= elf2FirstZone && elf1FirstZone <= elf2LastZone)
                {
                    sumMatchingPairs++;
                }
                // Collection 1's last element is within collection 2
                else if (elf1LastZone >= elf2FirstZone && elf1LastZone <= elf2LastZone)
                {
                    sumMatchingPairs++;
                }
                // Collection 2's first element is within collection 1
                else if (elf2FirstZone >= elf1FirstZone && elf2FirstZone <= elf1LastZone)
                {
                    sumMatchingPairs++;
                }
                // Collection 2's last element is within collection 1
                else if (elf2LastZone >= elf1FirstZone && elf2LastZone <= elf1LastZone)
                {
                    sumMatchingPairs++;
                }
            }

            static (int firstZone, int LastZone) ConvertShortRangeToLong(string zoneIds)
            {
                var zoneParts = zoneIds.Split('-');
                var elf1Start = int.Parse(zoneParts[0]);
                var elf1Finish = int.Parse(zoneParts.Length == 1 ? zoneParts[0] : zoneParts[1]);

                return (elf1Start, elf1Finish);
            }

            Assert.Equal(expectedSumOverlappingPairs, sumMatchingPairs);
        }
    }
}