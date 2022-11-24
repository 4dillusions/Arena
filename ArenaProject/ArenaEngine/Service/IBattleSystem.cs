using ArenaEngine.Core;
using ArenaEngine.Model;
using System.Collections.Generic;

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

        List<HeroDTO> CreateRandomHeroList(uint listSize);

        /// <summary> a bemenő listából kivesz harcosokat a visszatérő listába </summary>
        List<HeroDTO> SelectHeroesForBattle(ref List<HeroDTO> heroList);

        /// <summary> Harc után visszamennek pihenni a helyükre </summary>
        /// <param name="battleHeroes"> küzdő felek </param>
        /// <param name="heroList"> összes hős listája </param>
        void GoRestHeroesAfterBattle(List<HeroDTO> battleHeroes, ref List<HeroDTO> heroList);

        /// <summary>
        /// 1v1 ütközet két hős között, ahol az életerő és az élet változik attól függően, hogy ki a támadó s ki védekezik
        /// küzdelem után is fogy az életerő a harc miatt
        /// </summary>
        void PlayBattle(HeroDTO attacker, HeroDTO defender);

        /// <summary> validálja az életerőt és beállítja ez alapján, hogy él vagy nem a hős </summary>
        void ValidateHero(HeroDTO hero);
    }
}
