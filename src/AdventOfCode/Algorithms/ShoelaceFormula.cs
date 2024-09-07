namespace AdventOfCode.Algorithms;

public static class ShoelaceFormula
{
    public record Point(double X, double Y);
    
    /// <summary>
    /// Calculate polygon area using shoelace formula.
    /// </summary>
    /// <remarks>
    /// Does NOT handle polygons with overlaping verticies.
    /// https://youtu.be/bxNVXQNMA7o?si=IWIanbgW5qBcyO9v&t=409
    /// </remarks>
    public static double CalculatePolygonArea(IReadOnlyList<Point> coords, int boundaryLength, double depth = 1)
    {
        double area = 0.0;
        int j = coords.Count - 1;

        for (var i = 0; i < coords.Count; i++)
        {
            area += (coords[j].X + coords[i].X) * (coords[j].Y - coords[i].Y);

            // j is the previous vertex to i.
            j = i;
        }

        return Math.Abs(area) / 2 + boundaryLength / 2 + 1;
    }
}
