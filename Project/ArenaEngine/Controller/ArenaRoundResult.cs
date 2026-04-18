using ArenaEngine.Model;

namespace ArenaEngine.Controller;

public class ArenaRoundResult
{
    public required int RoundNumber { get; init; }
    public required HeroDTO Attacker { get; init; }
    public required HeroDTO Defender { get; init; }
}
