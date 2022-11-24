using System;
using System.Collections.Generic;
using ArenaEngine.Core;
using ArenaEngine.Model;
using ArenaEngine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArenaTest.Engine
{
    [TestClass]
    public class BattleTests
    {
        /// <summary>
        /// Kiválaszt az arénából 2 harcost (ha van) a küzdelemre
        /// </summary>
        [TestMethod]
        public void SelectHeroesForBattle()
        {
            IBattleSystem battleSystem = new LimeBattleSystem(new GameConfigDTO());
            var heroList = new List<HeroDTO>();

            Assert.IsTrue(battleSystem.SelectHeroesForBattle(ref heroList).Count == 0);

            heroList = battleSystem.CreateRandomHeroList(2);
            Assert.IsTrue(battleSystem.SelectHeroesForBattle(ref heroList).Count == 2);
            Assert.IsTrue(heroList.Count == 0);

            heroList = battleSystem.CreateRandomHeroList(3);
            Assert.IsTrue(battleSystem.SelectHeroesForBattle(ref heroList).Count == 2);
            Assert.IsTrue(heroList.Count == 1);
        }

        /// <summary>
        /// a szabályok szerint:
        /// -életerő nem mehet a max fölé
        /// -életerő kisebb, mint a max negyede, akkor nem él
        /// </summary>
        [TestMethod]
        public void ValidateLimeHero()
        {
            var heroTypeCount = Enum.GetValues(typeof(HeroTypes)).Length;
            var heroes = new HeroDTO[heroTypeCount];
            for (var i = 0; i < heroTypeCount; i++)
                heroes[i] = new HeroDTO {HeroType = (HeroTypes) i, Power = (i + 1) * 10};

            //születése után még él :)
            Assert.IsTrue(heroes[(int) HeroTypes.KnightRider].IsAlive);
            Assert.IsTrue(heroes[(int) HeroTypes.Swordsman].IsAlive);
            Assert.IsTrue(heroes[(int) HeroTypes.Bowman].IsAlive);

            var gameConfig = new GameConfigDTO()
            {
                KnightRiderMaxPower = 10,
                SwordsmanMaxPower = 20,
                BowmanMaxPower = 30,
            };
            IBattleSystem battleSystem = new LimeBattleSystem(gameConfig);

            for (var i = 0; i < heroTypeCount; i++)
            {
                heroes[i].Power++;
                battleSystem.ValidateHero(heroes[i]);
            }

            //-életerő nem mehet a max fölé
            Assert.IsTrue(heroes[(int) HeroTypes.KnightRider].Power == gameConfig.KnightRiderMaxPower);
            Assert.IsTrue(heroes[(int) HeroTypes.Swordsman].Power == gameConfig.SwordsmanMaxPower);
            Assert.IsTrue(heroes[(int) HeroTypes.Bowman].Power == gameConfig.BowmanMaxPower);

            for (var i = 0; i < heroTypeCount; i++)
            {
                heroes[i].Power /= 4;
                heroes[i].Power--;
                battleSystem.ValidateHero(heroes[i]);
            }

            //-életerő kisebb, mint a max negyede, akkor nem él
            Assert.IsFalse(heroes[(int) HeroTypes.KnightRider].IsAlive);
            Assert.IsFalse(heroes[(int) HeroTypes.Swordsman].IsAlive);
            Assert.IsFalse(heroes[(int) HeroTypes.Bowman].IsAlive);
        }

        void RecreateHeroesAndPlayBattle(IBattleSystem battleSystem, ref HeroDTO attacker, ref HeroDTO defender)
        {
            attacker = battleSystem.CreateHero(attacker.HeroType);
            defender = battleSystem.CreateHero(defender.HeroType);

            battleSystem.PlayBattle(attacker, defender);
        }

        /// <summary>
        /// Hősök között 1v1 küzdelem (mindenki mindenkivel)
        /// </summary>
        [TestMethod]
        public void PlayBattle()
        {
            IBattleSystem battleSystem = new LimeBattleSystem(new GameConfigDTO());

            var knightRiderAttacker = battleSystem.CreateHero(HeroTypes.KnightRider);
            var swordsmanAttacker = battleSystem.CreateHero(HeroTypes.Swordsman);
            var bowmanAttacker = battleSystem.CreateHero(HeroTypes.Bowman);
            var knightRiderDefender = battleSystem.CreateHero(HeroTypes.KnightRider);
            var swordsmanDefender = battleSystem.CreateHero(HeroTypes.Swordsman);
            var bowmanDefender = battleSystem.CreateHero(HeroTypes.Bowman);

            //generálása után van energiája
            Assert.IsTrue(knightRiderAttacker.Power > 0);
            Assert.IsTrue(swordsmanAttacker.Power > 0);
            Assert.IsTrue(bowmanAttacker.Power > 0);
            Assert.IsTrue(knightRiderDefender.Power > 0);
            Assert.IsTrue(swordsmanDefender.Power > 0);
            Assert.IsTrue(bowmanDefender.Power > 0);

            //-------------------------------------------------------------------------------------------
            //Íjász támad

            ////lovast: 40% eséllyel a lovas meghal, 60%-ban kivédi
            //van ötlet ezt hogyan lehetne tesztelni?

            ////kardost: kardos meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref bowmanAttacker, ref swordsmanDefender);
            Assert.IsTrue(bowmanAttacker.Power > 0 && swordsmanDefender.Power == 0);

            ////íjászt: védekező meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref bowmanAttacker, ref bowmanDefender);
            Assert.IsTrue(bowmanAttacker.Power > 0 && bowmanDefender.Power == 0);

            //-------------------------------------------------------------------------------------------
            //Kardos támad

            ////
            //lovast: nem történik semmi
            RecreateHeroesAndPlayBattle(battleSystem, ref swordsmanAttacker, ref knightRiderDefender);
            Assert.IsTrue(swordsmanAttacker.Power > 0 && knightRiderDefender.Power > 0);

            ////
            //kardost: védekező meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref swordsmanAttacker, ref swordsmanDefender);
            Assert.IsTrue(swordsmanAttacker.Power > 0 && swordsmanDefender.Power == 0);

            ////
            //íjászt: íjász meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref swordsmanAttacker, ref bowmanDefender);
            Assert.IsTrue(swordsmanAttacker.Power > 0 && bowmanDefender.Power == 0);

            //-------------------------------------------------------------------------------------------
            //Lovas támad

            ////
            //lovast: védekező meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref knightRiderAttacker, ref knightRiderDefender);
            Assert.IsTrue(knightRiderAttacker.Power > 0 && knightRiderDefender.Power == 0);

            ////
            //kardost: lovas meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref knightRiderAttacker, ref swordsmanDefender);
            Assert.IsTrue(knightRiderAttacker.Power == 0 && swordsmanDefender.Power > 0);

            ////
            //íjászt: íjász meghal
            RecreateHeroesAndPlayBattle(battleSystem, ref knightRiderAttacker, ref bowmanDefender);
            Assert.IsTrue(knightRiderAttacker.Power > 0 && bowmanDefender.Power == 0);
        }

        /// <summary>
        /// Harc után elfáradnak az emberek
        /// </summary>
        [TestMethod]
        public void AfterPlayBattle()
        {
            var gameConfig = new GameConfigDTO();
            IBattleSystem battleSystem = new LimeBattleSystem(gameConfig);

            var knightRider = battleSystem.CreateHero(HeroTypes.KnightRider);
            var swordsman = battleSystem.CreateHero(HeroTypes.Swordsman);

            Assert.IsTrue(knightRider.Power > gameConfig.KnightRiderMaxPower / 2);
            Assert.IsTrue(swordsman.Power > gameConfig.SwordsmanMaxPower / 2);

            battleSystem.PlayBattle(swordsman, knightRider);

            Assert.IsTrue(knightRider.Power == gameConfig.KnightRiderMaxPower / 2);
            Assert.IsTrue(swordsman.Power == gameConfig.SwordsmanMaxPower / 2);
        }

        /// <summary>
        /// Harc után visszamennek a helyükre pihenni (ha még élnek)
        /// </summary>
        [TestMethod]
        public void GoRestAfterPlayBattle()
        {
            var gameConfig = new GameConfigDTO();
            IBattleSystem battleSystem = new LimeBattleSystem(gameConfig);

            var heroList = new List<HeroDTO>();
            var knightRider = battleSystem.CreateHero(HeroTypes.KnightRider);
            var swordsman = battleSystem.CreateHero(HeroTypes.Swordsman);

            battleSystem.GoRestHeroesAfterBattle(new List<HeroDTO> { knightRider, swordsman }, ref heroList);
            Assert.IsTrue(heroList.Count == 2);
            Assert.IsTrue(heroList[0].Power == gameConfig.KnightRiderMaxPower);
            Assert.IsTrue(heroList[1].Power == gameConfig.SwordsmanMaxPower);

            heroList.Clear();
            knightRider.Power = 10;
            battleSystem.GoRestHeroesAfterBattle(new List<HeroDTO> { knightRider, swordsman }, ref heroList);
            Assert.IsTrue(heroList.Count == 1);
            Assert.IsTrue(knightRider.Power == 10);
            Assert.IsTrue(heroList[0].Power == gameConfig.SwordsmanMaxPower);
        }
    }
}
