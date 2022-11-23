using System.Collections.Generic;
using ArenaEngine.Core;
using ArenaEngine.Model;

namespace ArenaEngine.Service
{
    public interface IBattleSystem
    {
        /// <summary>
        /// Hős típusokról ad vissza egy random listát
        /// </summary>
        /// <param name="listSize"> random lista mérete </param>
        /// <returns> hős típusok listája </returns>
        List<HeroTypes> CreateRandomHeroTypeList(uint listSize);

        HeroDTO CreateHero(HeroTypes heroType);

        /// <summary> validálja az életerőt és beállítja ez alapján, hogy él vagy nem a hős </summary>
        void ValidateHero(HeroDTO hero);

        /// <summary> 1v1 ütközet két hős között, ahol az életerő és az élet változik attól függően, hogy ki a támadó s ki védekezik </summary>
        void PlayBattle(HeroDTO attacker, HeroDTO defender);
    }
}
