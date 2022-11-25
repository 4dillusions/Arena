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
        /// Select 2 heroes from arena (if it possible) for battle
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
        /// rules:
        /// maximize the power
        /// the power is less than quarter of the initial/maximum power then hero die
        /// </summary>
        [TestMethod]
        public void ValidateLimeHero()
        {
            var heroTypeCount = Enum.GetValues(typeof(HeroTypes)).Length;
            var heroes = new HeroDTO[heroTypeCount];
            for (var i = 0; i < heroTypeCount; i++)
                heroes[i] = new HeroDTO {HeroType = (HeroTypes) i, Power = (i + 1) * 10};

            //hero is alive afther born :)
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

            //maximize power
            Assert.IsTrue(heroes[(int) HeroTypes.KnightRider].Power == gameConfig.KnightRiderMaxPower);
            Assert.IsTrue(heroes[(int) HeroTypes.Swordsman].Power == gameConfig.SwordsmanMaxPower);
            Assert.IsTrue(heroes[(int) HeroTypes.Bowman].Power == gameConfig.BowmanMaxPower);

            for (var i = 0; i < heroTypeCount; i++)
            {
                heroes[i].Power /= 4;
                heroes[i].Power--;
                battleSystem.ValidateHero(heroes[i]);
            }

            //the power is less than quarter of the initial/maximum power then hero die
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
        /// 1v1 battle rules
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

            //hero has power after generate
            Assert.IsTrue(knightRiderAttacker.Power > 0);
            Assert.IsTrue(swordsmanAttacker.Power > 0);
            Assert.IsTrue(bowmanAttacker.Power > 0);
            Assert.IsTrue(knightRiderDefender.Power > 0);
            Assert.IsTrue(swordsmanDefender.Power > 0);
            Assert.IsTrue(bowmanDefender.Power > 0);

            //-------------------------------------------------------------------------------------------
            //Bowman (attack)

            ////knight rider (defense): dies 40%, lives 60%
            //Does anybody have an idea for testing it?

            ////swordsman (defense): dies
            RecreateHeroesAndPlayBattle(battleSystem, ref bowmanAttacker, ref swordsmanDefender);
            Assert.IsTrue(bowmanAttacker.Power > 0 && swordsmanDefender.Power == 0);

            ////bowman (defense): dies
            RecreateHeroesAndPlayBattle(battleSystem, ref bowmanAttacker, ref bowmanDefender);
            Assert.IsTrue(bowmanAttacker.Power > 0 && bowmanDefender.Power == 0);

            //-------------------------------------------------------------------------------------------
            //Swordsman (attack)

            ////
            //knight rider (defense): nothing happens
            RecreateHeroesAndPlayBattle(battleSystem, ref swordsmanAttacker, ref knightRiderDefender);
            Assert.IsTrue(swordsmanAttacker.Power > 0 && knightRiderDefender.Power > 0);

            ////
            //swordsman (defense): dies
            RecreateHeroesAndPlayBattle(battleSystem, ref swordsmanAttacker, ref swordsmanDefender);
            Assert.IsTrue(swordsmanAttacker.Power > 0 && swordsmanDefender.Power == 0);

            ////
            //bowman (defense): dies
            RecreateHeroesAndPlayBattle(battleSystem, ref swordsmanAttacker, ref bowmanDefender);
            Assert.IsTrue(swordsmanAttacker.Power > 0 && bowmanDefender.Power == 0);

            //-------------------------------------------------------------------------------------------
            //Knight rider (attack)

            ////
            //knight rider (defense): dies
            RecreateHeroesAndPlayBattle(battleSystem, ref knightRiderAttacker, ref knightRiderDefender);
            Assert.IsTrue(knightRiderAttacker.Power > 0 && knightRiderDefender.Power == 0);

            ////
            //swordsman (defense): knight rider dies
            RecreateHeroesAndPlayBattle(battleSystem, ref knightRiderAttacker, ref swordsmanDefender);
            Assert.IsTrue(knightRiderAttacker.Power == 0 && swordsmanDefender.Power > 0);

            ////
            //bowman (defense): dies
            RecreateHeroesAndPlayBattle(battleSystem, ref knightRiderAttacker, ref bowmanDefender);
            Assert.IsTrue(knightRiderAttacker.Power > 0 && bowmanDefender.Power == 0);
        }

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
