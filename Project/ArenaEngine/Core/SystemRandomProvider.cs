namespace ArenaEngine.Core;

public class SystemRandomProvider : IRandomProvider
{
    private readonly Random random = new();

    public int Next(int maxValue)
    {
        return random.Next(maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }
}
