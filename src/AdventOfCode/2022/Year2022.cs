namespace AdventOfCode
{
    public class Year2022
    {
        [Theory]
        [InlineData("Day1DevelopmentTesting.txt", 24000)]
        [InlineData("Day1.txt", 71924)]
        public void Day1_Part1_CalorieCounting(string filename, int expectedAnswer)
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
        public void Day1_Part2_CalorieCounting(string filename, int expectedAnswer)
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
    }
}