using ArenaEngine.Model;

namespace ArenaEngine.Service;

public class BattleSelectionResult
{
    public required List<HeroDTO> BattleHeroes { get; init; }
    public required List<HeroDTO> RemainingHeroes { get; init; }
}
