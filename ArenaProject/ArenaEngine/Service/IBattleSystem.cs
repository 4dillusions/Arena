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

    /// <summary> take out heroes from heroList and add them in return list </summary>
    List<HeroDTO> SelectHeroesForBattle(List<HeroDTO> heroList);

    /// <summary>
    /// rest all heroes in list
    /// </summary>
    /// <param name="heroList"> resting heroes </param>
    void RestHeroes(List<HeroDTO> heroList);

    /// <summary>
    /// Resolves a 1v1 battle and applies post-battle power loss.
    /// </summary>
    void PlayBattle(HeroDTO attacker, HeroDTO defender);

    /// <summary>Returns surviving heroes to the arena after battle.</summary>
    /// <param name="battleHeroes">The two heroes that fought in battle.</param>
    /// <param name="heroList">The list of heroes currently in the arena.</param>
    void GoBackHeroesAfterBattle(List<HeroDTO> battleHeroes, List<HeroDTO> heroList);
}
