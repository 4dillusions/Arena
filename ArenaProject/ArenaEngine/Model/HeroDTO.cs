using ArenaEngine.Core;

namespace ArenaEngine.Model;

/// <summary>
/// The hero in arena
/// it is a data transfering object for game logic and others
/// has id, power, type
/// is alive or not depending on power
/// </summary>
public class HeroDTO
{
    public uint Id { get; set; }
    public int Power { get; set; }
    public HeroTypes HeroType { get; set; }
    public bool IsAlive { get; set; } = true;
    public string Description { get; set; } = null!;
}