using System;
using System.Collections.Generic;
using ArenaEngine.Core;
using ArenaEngine.Model;

namespace ArenaEngine.Service
{
    /// <summary>
    /// ellenőrzi van e még hős, ha csak egy van és él vége a játéknak, ő nyert
    /// harcra választ 2 hőst
    /// kört futtat, ahol két hős közül egyik elesik, vagy más történik
    /// harcost pihentet, regenerálódik
    /// harcost harcoltat és csökken az életereje
    /// </summary>
    public class LimeBattleSystem : IBattleSystem
    {
        private readonly GameConfigDTO gameConfig;

        public LimeBattleSystem(GameConfigDTO gameConfig)
        {
            this.gameConfig = gameConfig;
        }

        public List<HeroTypes> CreateRandomHeroTypeList(uint listSize)
        {
            if (listSize > gameConfig.MaximumArenaHeroCount)
                throw new ArgumentOutOfRangeException();

            return RecruitmentManager<HeroTypes>.CreateRandomTypeList(listSize);
        }

        public HeroDTO CreateHero(HeroTypes heroType)
        {
            var result = new HeroDTO()
            {
                HeroType = heroType
            };

            switch (heroType)
            {
                case HeroTypes.KnightRider: result.Power = gameConfig.KnightRiderMaxPower; break;
                case HeroTypes.Swordsman: result.Power = gameConfig.SwordsmanMaxPower; break;
                case HeroTypes.Bowman: result.Power = gameConfig.BowmanMaxPower; break;

                default: throw new ArgumentOutOfRangeException(nameof(heroType), heroType, null);
            }

            return result;
        }

        public void ValidateHero(HeroDTO hero)
        {
            short maxPower;
            
            switch (hero.HeroType)
            {
                case HeroTypes.KnightRider: maxPower = gameConfig.KnightRiderMaxPower; break;
                case HeroTypes.Swordsman: maxPower = gameConfig.SwordsmanMaxPower; break;
                case HeroTypes.Bowman: maxPower = gameConfig.BowmanMaxPower; break;
                
                default: throw new ArgumentOutOfRangeException();
            }

            //-életerő nem mehet a max fölé
            if (hero.Power > maxPower)
                hero.Power = maxPower;

            //-ha életerő kisebb, mint a max negyede, akkor nem él
            if (hero.Power < maxPower / 4)
                hero.IsAlive = false;
        }

        public void PlayBattle(HeroDTO attacker, HeroDTO defender)
        {
            //ha a lovas védekezik, akkor...
            if (defender.HeroType == HeroTypes.KnightRider)
            {
                switch (attacker.HeroType)
                {
                    case HeroTypes.KnightRider: defender.Power = 0; break; //lovas támad lovast -> védekező kinyúlik
                    case HeroTypes.Swordsman: /* nem történik semmi */ break; //kardos támad lovast
                    case HeroTypes.Bowman: defender.Power = new Random().Next(1, 10) <= 6 ? defender.Power : 0;  break; //ijjász támad lovast -> az esetek 40%-ban lovas meghal, 60%-ban kivédi

                    default: throw new ArgumentOutOfRangeException();
                }

                return;
            }

            //ha a kardos, vagy íjjász védekezik, akkor mindig a védekező kinyúlik
            //kivétel, amikor lovas támad kardost, mert akkor viszont a lovas esik el
            if (attacker.HeroType == HeroTypes.KnightRider && defender.HeroType == HeroTypes.Swordsman)
            {
                attacker.Power = 0;
                return;
            }

            if (defender.HeroType == HeroTypes.Swordsman || defender.HeroType == HeroTypes.Bowman)
                defender.Power = 0;
        }
    }
}
