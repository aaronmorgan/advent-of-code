using System.Diagnostics;
using AdventOfCode.Utilities;

namespace AdventOfCode._2023;

public class Day10
{
    [Theory]
    [InlineData("Day10DevelopmentTesting1.txt", 4)]
    [InlineData("Day10DevelopmentTesting2.txt", 4)]
    [InlineData("Day10DevelopmentTesting3.txt", 8)]
    [InlineData("Day10DevelopmentTesting4.txt", 8)]
    [InlineData("Day10.txt", 6828)]
    public void Day10_Part1_PipeMaze(string filename, int expectedAnswer)
    {
        var fileInput = FileLoader.ReadFile("2023/" + filename).ToList();

        var map = new char[fileInput[0].Length, fileInput.Count];
        (int X, int Y) startLocation = (0, 0);

        for (var i = 0; i < fileInput.Count; i++)
        {
            for (var j = 0; j < fileInput[i].Length; j++)
            {
                var c = fileInput[i][j];
                map[i, j] = c;

                if (c == 'S') startLocation = (X: j, Y: i);
            }
        }

        var exitNodes = new [] { new MapLocation(), new MapLocation() };
        
        // Determine exit nodes from the S starting point.
        var x = startLocation.X;
        var y = startLocation.Y;
            
        var exitNodeIndex = 0;
            
        if (y > 0 && map[y - 1, x] == '|') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y - 1, X = x, PrevX = x, PrevY = y, Symbol = map[y - 1, x] }; exitNodeIndex ++; map[y, x] = '|';}
        if (map[y, x + 1] == '-') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y, X = x + 1, PrevX = x, PrevY = y, Symbol = map[y, x + 1] }; exitNodeIndex ++; map[y, x] = '-'; }
        if (map[y + 1, x] == '|') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y + 1, X = x, PrevX = x, PrevY = y, Symbol = map[y + 1, x] }; exitNodeIndex ++; map[y, x] = '|'; }
        if (x > 0 && map[y, x - 1] == '-') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y, X = x - 1, PrevX = x, PrevY = y, Symbol = map[y, x - 1] }; exitNodeIndex++; map[y, x] = '-'; }
        
        if (y > 0 && map[y - 1, x] == 'F') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y - 1, X = x, PrevX = x, PrevY = y, Symbol = map[y - 1, x] }; exitNodeIndex ++; map[y, x] = 'F'; }
        if (y > 0 && map[y - 1, x] == '7') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y - 1, X = x, PrevX = x, PrevY = y, Symbol = map[y - 1, x] }; exitNodeIndex ++; map[y, x] = '7'; }
        if (y + 1 < fileInput.Count && map[y + 1, x] == 'L') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y + 1, X = x, PrevX = x, PrevY = y, Symbol = map[y + 1, x] }; exitNodeIndex ++; map[y, x] = 'L'; }
        if (y + 1 < fileInput.Count && map[y + 1, x] == 'J') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y + 1, X = x, PrevX = x, PrevY = y, Symbol = map[y + 1, x] }; exitNodeIndex ++; map[y, x] = 'J'; }
        
        if (x > 0 && map[y, x - 1] == 'F') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y, X = x - 1, PrevX = x, PrevY = y, Symbol = map[y, x - 1] }; map[y, x] = 'F';}
        if (x > 0 && map[y, x - 1] == 'L') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y, X = x - 1, PrevX = x, PrevY = y, Symbol = map[y, x - 1] }; map[y, x] = 'L';}
        if (x + 1 < fileInput[0].Length && map[y, x + 1] == '7') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y, X = x + 1, PrevX = x, PrevY = y, Symbol = map[y, x + 1] }; exitNodeIndex ++; map[y, x] = '7';}
        if (x + 1 < fileInput[0].Length && map[y, x + 1] == 'J') { exitNodes[exitNodeIndex] = new MapLocation{ Y = y, X = x + 1, PrevX = x, PrevY = y, Symbol = map[y, x + 1] }; exitNodeIndex ++; map[y, x] = 'J';}
            
        var routeA = exitNodes[0];
        var routeB = exitNodes[1];
        
        Dictionary<string, int> pastPositions = new();
        
        long routeALength = 1, maxDistance = 0;
        
        while(true)
        {
            if (routeA.Symbol == '|')
            {
                if (routeA.PrevY == routeA.Y - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else if (routeA.Symbol == '-')
            {
                if (routeA.PrevX == routeA.X - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else if (routeA.Symbol == '7')
            {
                if (routeA.PrevX == routeA.X - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else if (routeA.Symbol == 'J')
            {
                if (routeA.PrevY == routeA.Y - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else if (routeA.Symbol == 'L')
            {
                if (routeA.PrevY == routeA.Y - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else if (routeA.Symbol == 'F')
            {
                if (routeA.PrevY == routeA.Y + 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else if (routeA.Symbol == '.')
            {
                if (routeA.PrevY == routeA.Y + 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else if (routeA.PrevY == routeA.Y - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.Y += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else if (routeA.PrevX == routeA.X + 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X -= 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
                else if (routeA.PrevX == routeA.X - 1) { routeA.PrevX = routeA.X; routeA.PrevY = routeA.Y; routeA.X += 1; routeA.Symbol = map[routeA.Y, routeA.X]; }
            }
            else Debugger.Break();
            
            routeALength += 1;
            
            if (routeALength > 13658) Assert.Fail();
            
            if (routeB.Symbol == '|')
            {
                if (routeB.PrevY == routeB.Y - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else if (routeB.Symbol == '-')
            {
                if (routeB.PrevX == routeB.X - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else if (routeB.Symbol == '7')
            {
                if (routeB.PrevX == routeB.X - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else if (routeB.Symbol == 'J')
            {
                if (routeB.PrevY == routeB.Y - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else if (routeB.Symbol == 'L')
            {
                if (routeB.PrevY == routeB.Y - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else if (routeB.Symbol == 'F')
            {
                if (routeB.PrevY == routeB.Y + 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else if (routeB.Symbol == '.')
            {
                if (routeB.PrevY == routeB.Y + 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else if (routeB.PrevY == routeB.Y - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.Y += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else if (routeB.PrevX == routeB.X + 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X -= 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
                else if (routeB.PrevX == routeB.X - 1) { routeB.PrevX = routeB.X; routeB.PrevY = routeB.Y; routeB.X += 1; routeB.Symbol = map[routeB.Y, routeB.X]; }
            }
            else Debugger.Break();
            
            if (routeA.X == routeB.X && routeA.Y == routeB.Y)
            {
                break;
            }
            
            if (routeALength > maxDistance) maxDistance = routeALength;
        }

        Assert.Equal(expectedAnswer, routeALength);
    }

    private struct MapLocation
    {
        public int X;
        public int Y;
        public int PrevX;
        public int PrevY;
        public char Symbol;
    }
}
