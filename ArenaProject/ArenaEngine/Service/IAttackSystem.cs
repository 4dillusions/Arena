using System.Collections.Generic;
using ArenaEngine.Core;

namespace ArenaEngine.Service
{
    public interface IAttackSystem
    {
        /// <summary>
        /// Hős típusokról ad vissza egy random listát
        /// </summary>
        /// <param name="listSize"> random lista mérete </param>
        /// <returns> hős típusok listája </returns>
        List<HeroTypes> CreateRandomHeroTypeList(uint listSize);
    }
}
