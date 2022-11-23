using System;
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
        /// Hősök között 1v1 küzdelem
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
    }
}
