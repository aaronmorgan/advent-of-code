using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode._2015;

public class Day4
{
    [Theory]
    [InlineData("abcdef", 609043)]
    [InlineData("yzbqklnj", 282749)]
    public void Day4_Part1_TheIdealStockingStuffer(string secretKey, int expectedAnswer)
    {
        for (var i = 100000; i < 999999; i++)
        {
            if (ToMd5Hash(secretKey + i)[..5] == "00000")
            {
                Assert.Equal(expectedAnswer, i);
                break;
            }
        }
    }

    [Theory]
    [InlineData("yzbqklnj", 9962624)]
    public void Day4_Part2_TheIdealStockingStuffer(string secretKey, int expectedAnswer)
    {
        for (var i = 1000000; i < 9999999; i++)
        {
            if (ToMd5Hash(secretKey + i)[..6] == "000000")
            {
                Assert.Equal(expectedAnswer, i);
                break;
            }
        }
    }

    private static string ToMd5Hash(string input)
    {
        var hashBytes = MD5.HashData(Encoding.ASCII.GetBytes(input));

        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}