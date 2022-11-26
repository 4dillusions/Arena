using ArenaEngine.Core;
using ArenaEngine.Model;

namespace ArenaEngine.Service;

public interface IBattleSystem
{
    /// <summary>
    /// Generate random list of heroes
    /// </summary>
    /// <param name="listSize"> size of this list </param>
    /// <returns> returns list contains heroes </returns>
    List<HeroTypes> CreateRandomHeroTypeList(uint listSize);

    HeroDTO CreateHero(HeroTypes heroType);

    List<HeroDTO>? CreateRandomHeroList(uint listSize);

    /// <summary> take out heroes from heroList and add them in return list </summary>
    List<HeroDTO> SelectHeroesForBattle(ref List<HeroDTO>? heroList);

    /// <summary> Heroes go rest afther the battle </summary>
    /// <param name="battleHeroes"> two heroes in the battle </param>
    /// <param name="heroList"> list of all resting heroes </param>
    void GoRestHeroesAfterBattle(List<HeroDTO> battleHeroes, ref List<HeroDTO>? heroList);

    /// <summary>
    /// 1v1 battle, changing power and live state according to rules
    /// decrease power because of battle
    /// </summary>
    void PlayBattle(HeroDTO attacker, HeroDTO defender);

    /// <summary> maximize his power and change live state according to power </summary>
    void ValidateHero(HeroDTO hero);
}