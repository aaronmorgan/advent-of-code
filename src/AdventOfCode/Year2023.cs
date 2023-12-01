namespace AdventOfCode
{
    public class Year2023
    {
        private static IEnumerable<string> ReadFile(string filename) => File.ReadLines($"./TestData/2023/{filename}");

        [Theory]
        [InlineData("Day1DevelopmentTesting1.txt", 142)]
        [InlineData("Day1.txt", 55172)]
        public void Day1_Part1_Trebuchet(string filename, int expectedAnswer)
        {
            int result = 0;
            foreach (var line in ReadFile(filename))
            {
                if (string.IsNullOrEmpty(line)) continue;

                var firstDigit = string.Empty;
                var secondDigit = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    if (int.TryParse(line[i].ToString(), out var f))
                    {
                        if (string.IsNullOrEmpty(firstDigit))
                        {
                            firstDigit = f.ToString();
                        }

                        secondDigit = f.ToString();
                    }
                }

                result += int.Parse(firstDigit + secondDigit);
            }

            Assert.Equal(expectedAnswer, result);
        }

        [Theory]
        [InlineData("Day1DevelopmentTesting2.txt", 281)]
        [InlineData("Day1.txt", 54925)]
        public void Day1_Part2_Trebuchet(string filename, int expectedAnswer)
        {
            int result = 0;

            string? firstDigit;
            string? secondDigit;

            foreach (var line in ReadFile(filename))
            {
                if (string.IsNullOrEmpty(line)) continue;

                firstDigit = string.Empty;
                secondDigit = string.Empty;

                for (int i = 0; i < line.Length; i++)
                {
                    switch (line[i..])
                    {
                        case string s when int.TryParse(s, out var x): SetDigit(x); continue;
                        case string s when s.StartsWith("one"): SetDigit(1); continue;
                        case string s when s.StartsWith("two"): SetDigit(2); continue;
                        case string s when s.StartsWith("three"): SetDigit(3); continue;
                        case string s when s.StartsWith("four"): SetDigit(4); continue;
                        case string s when s.StartsWith("five"): SetDigit(5); continue;
                        case string s when s.StartsWith("six"): SetDigit(6); continue;
                        case string s when s.StartsWith("seven"): SetDigit(7); continue;
                        case string s when s.StartsWith("eight"): SetDigit(8); continue;
                        case string s when s.StartsWith("nine"): SetDigit(9); continue;

                        default:
                            return;
                    }
                }

                result += int.Parse(firstDigit + secondDigit);
            }

            void SetDigit(int number)
            {
                if (string.IsNullOrEmpty(firstDigit))
                {
                    firstDigit = number.ToString();
                }

                secondDigit = number.ToString();
            }

            Assert.Equal(expectedAnswer, result);
        }
    }
}
