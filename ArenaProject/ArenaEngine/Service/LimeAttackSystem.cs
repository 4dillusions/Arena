using System;
using System.Collections.Generic;
using ArenaEngine.Core;

namespace ArenaEngine.Service
{
    /// <summary>
    /// ellenőrzi van e még hős, ha csak egy van és él vége a játéknak, ő nyert
    /// harcra választ 2 hőst
    /// kört futtat, ahol két hős közül egyik elesik, vagy más történik
    /// harcost pihentet, regenerálódik
    /// harcost harcoltat és csökken az életereje
    /// </summary>
    public class LimeAttackSystem : IAttackSystem
    {
        private readonly GameConfigDTO gameConfig;

        public LimeAttackSystem(GameConfigDTO gameConfig)
        {
            this.gameConfig = gameConfig;
        }

        public List<HeroTypes> CreateRandomHeroTypeList(uint listSize)
        {
            if (listSize > gameConfig.MaximumArenaHeroCount)
                throw new ArgumentOutOfRangeException();

            return RecruitmentManager<HeroTypes>.CreateRandomTypeList(listSize);
        }
    }
}
