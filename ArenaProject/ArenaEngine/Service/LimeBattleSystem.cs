using ArenaEngine.Core;
using ArenaEngine.Model;
using System;
using System.Collections.Generic;

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
            var result = new HeroDTO
            {
                HeroType = heroType,
                IsAlive = true
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

        public List<HeroDTO> CreateRandomHeroList(uint listSize)
        {
            var result = new List<HeroDTO>();
            var heroTypeList = CreateRandomHeroTypeList(listSize);

            for (uint i = 0; i < heroTypeList.Count; i++)
            {
                var hero = CreateHero(heroTypeList[(int)i]);
                hero.Id = i + 1;
                result.Add(hero);
            }

            return result;
        }

        public List<HeroDTO> SelectHeroesForBattle(ref List<HeroDTO> heroList)
        {
            var result = new List<HeroDTO>();
            var random = new Random();

            if (heroList.Count < 2)
                return result;

            for (int i = 0; i < 2; i++)
            {
                var heroRandIndex = random.Next(heroList.Count - 1);
                result.Add(heroList[heroRandIndex]);
                heroList.RemoveAt(heroRandIndex);
            }
            
            return result;
        }

        public void GoRestHeroesAfterBattle(List<HeroDTO> battleHeroes, ref List<HeroDTO> heroList)
        {
            foreach (var hero in battleHeroes)
            {
                ValidateHero(hero);

                if (hero.IsAlive)
                {
                    hero.Power += gameConfig.RestPowerIncrement; //pihenő idő, életerő növekszik
                    ValidateHero(hero); //azért túlzásba se kell vinni a töltődést
                    
                    heroList.Add(hero);
                }
            }
        }

        public void PlayBattle(HeroDTO attacker, HeroDTO defender)
        {
            void DecrementPowerAfterBattle()
            {
                attacker.Power /= 2;
                defender.Power /= 2;

                ValidateHero(attacker);
                ValidateHero(defender);
            }

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

                DecrementPowerAfterBattle();

                return;
            }

            //ha a kardos, vagy íjjász védekezik, akkor mindig a védekező kinyúlik
            //kivétel, amikor lovas támad kardost, mert akkor viszont a lovas esik el
            if (attacker.HeroType == HeroTypes.KnightRider && defender.HeroType == HeroTypes.Swordsman)
            {
                attacker.Power = 0;
                DecrementPowerAfterBattle();

                return;
            }

            if (defender.HeroType == HeroTypes.Swordsman || defender.HeroType == HeroTypes.Bowman)
                defender.Power = 0;

            DecrementPowerAfterBattle();
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
    }
}
