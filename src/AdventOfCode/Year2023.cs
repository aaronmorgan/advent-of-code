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

                char[] digits = new char[2];

                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        if (digits[0] == '\0')
                        {
                            digits[0] = line[i];
                        }

                        digits[1] = line[i];
                    }
                }

                result += int.Parse(new string(digits));
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
                        case string s when s.StartsWith("one"): SetDigit(1); i += 2; continue;
                        case string s when s.StartsWith("two"): SetDigit(2); i += 2; continue;
                        case string s when s.StartsWith("three"): SetDigit(3); i += 4; continue;
                        case string s when s.StartsWith("four"): SetDigit(4); i += 3; continue;
                        case string s when s.StartsWith("five"): SetDigit(5); i += 3; continue;
                        case string s when s.StartsWith("six"): SetDigit(6); i += 2; continue;
                        case string s when s.StartsWith("seven"): SetDigit(7); i += 4; continue;
                        case string s when s.StartsWith("eight"): SetDigit(8); i += 4; continue;
                        case string s when s.StartsWith("nine"): SetDigit(9); i += 3; continue;

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

        [Theory]
        [InlineData("Day2DevelopmentTesting1.txt", 8, 12, 13, 14)]
        [InlineData("Day2.txt", 2006, 12, 13, 14)]
        public void Day2_Part1_CubeConundrum(string filename, int expectedAnswer, int redCubes, int greenCubes, int blueCubes)
        {
            int result = 0;
            var games = ReadFile(filename);

            foreach (var game in games)
            {
                var gamenumber = int.Parse(game[..game.IndexOf(':')].Split(' ')[1]);
                var hands = game[(game.IndexOf(':') + 2)..].Split(';');
                var handSucceeds = true;

                foreach (var hand in hands)
                {
                    var redCount = 0;
                    var greenCount = 0;
                    var blueCount = 0;

                    var cubes = hand.Split(',').Select(x => x.Trim());

                    foreach (var cube in cubes)
                    {
                        switch (cube)
                        {
                            case string s when s.EndsWith("red"): redCount += DeriveValue(s); continue;
                            case string s when s.EndsWith("green"): greenCount += DeriveValue(s); continue;
                            case string s when s.EndsWith("blue"): blueCount += DeriveValue(s); continue;
                            default: break;
                        }

                        static int DeriveValue(string intput)
                        {
                            return int.Parse(intput[..intput.IndexOf(' ')]);
                        };
                    }

                    if (redCount > redCubes || greenCount > greenCubes || blueCount > blueCubes)
                    {
                        handSucceeds = false;
                        break;
                    }

                }

                if (handSucceeds)
                {
                    result += gamenumber;
                }
            }

            Assert.Equal(expectedAnswer, result);
        }

        [Theory]
        [InlineData("Day2DevelopmentTesting1.txt", 2286)]
        [InlineData("Day2.txt", 84911)]
        public void Day2_Part2_CubeConundrum(string filename, int expectedAnswer)
        {
            int result = 0;
            var games = ReadFile(filename);

            foreach (var game in games)
            {
                var gamenumber = int.Parse(game[..game.IndexOf(':')].Split(' ')[1]);
                var hands = game[(game.IndexOf(':') + 2)..].Split(';');

                var redCount = 0;
                var greenCount = 0;
                var blueCount = 0;

                foreach (var hand in hands)
                {
                    var cubes = hand.Split(',').Select(x => x.Trim());

                    foreach (var cube in cubes)
                    {
                        switch (cube)
                        {
                            case string s when s.EndsWith("red"): redCount = Math.Max(redCount, DeriveValue(s)); continue;
                            case string s when s.EndsWith("green"): greenCount = Math.Max(greenCount, DeriveValue(s)); continue;
                            case string s when s.EndsWith("blue"): blueCount = Math.Max(blueCount, DeriveValue(s)); continue;
                            default: break;
                        }

                        static int DeriveValue(string intput) => int.Parse(intput[..intput.IndexOf(' ')]);
                    }
                }

                result += redCount * greenCount * blueCount;
            }

            Assert.Equal(expectedAnswer, result);
        }
    }
}
