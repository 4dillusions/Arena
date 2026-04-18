namespace ArenaEngine.Core;

public class GameConfigDTO
{
    public uint MaximumArenaHeroCount { get; set; } = 50;

    public byte KnightRiderMaxPower { get; set; } = 150;
    public byte SwordsmanMaxPower { get; set; } = 120;
    public byte BowmanMaxPower { get; set; } = 100;

    public byte RestPowerIncrement { get; set; } = 10;
}