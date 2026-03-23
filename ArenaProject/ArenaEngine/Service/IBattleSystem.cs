using ArenaEngine.Core;
using ArenaEngine.Model;

namespace ArenaEngine.Service;

public interface IBattleSystem
{
    /// <summary>
    /// Generate random list of heroes
    /// </summary>
    /// <param name="listSize"> size of this list </param>
    /// <returns> A list of generated hero types. </returns>
    List<HeroTypes> CreateRandomHeroTypeList(uint listSize);

    HeroDTO CreateHero(HeroTypes heroType);

    List<HeroDTO> CreateRandomHeroList(uint listSize);

    /// <summary>Clamps hero power to the allowed maximum and updates their alive state.</summary>
    void ValidateHero(HeroDTO hero);

    /// <summary>Selects two heroes for battle and returns the remaining arena heroes separately.</summary>
    BattleSelectionResult SelectHeroesForBattle(IReadOnlyList<HeroDTO> heroList);

    /// <summary>
    /// rest all heroes in list
    /// </summary>
    /// <param name="heroList"> resting heroes </param>
    void RestHeroes(List<HeroDTO> heroList);

    /// <summary>
    /// Resolves a 1v1 battle and applies post-battle power loss.
    /// </summary>
    void PlayBattle(HeroDTO attacker, HeroDTO defender);

    /// <summary>Collects surviving heroes after battle.</summary>
    /// <param name="battleHeroes">The two heroes that fought in battle.</param>
    List<HeroDTO> GetSurvivingHeroesAfterBattle(List<HeroDTO> battleHeroes);
}
