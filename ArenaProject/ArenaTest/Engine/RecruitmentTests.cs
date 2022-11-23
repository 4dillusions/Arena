using ArenaEngine.Core;
using ArenaEngine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ArenaTest.Engine
{
    /// <summary>
    /// Kapott enum alapján az enum itemek tartományában N db enum itemet sorsol ki
    /// </summary>
    [TestClass]
    public class RecruitmentTests
    {
        [TestMethod]
        public void RandomRecruitment()
        {
            var heroTypeList = RecruitmentManager<HeroTypes>.CreateRandomTypeList(10);
            
            Assert.IsNotNull(heroTypeList);

            //minden elemből legalább egynek kell lennie a 10-es listában
            Assert.IsTrue(heroTypeList.Any(t => t == HeroTypes.KnightRider));
            Assert.IsTrue(heroTypeList.Any(t => t == HeroTypes.Swordsman));
            Assert.IsTrue(heroTypeList.Any(t => t == HeroTypes.Bowman));
        }

        [TestMethod]
        public void RandomRecruitmentFromLimeArena()
        {
            var gameConfig = new GameConfigDTO() { MaximumArenaHeroCount = 4 };
            IBattleSystem battleSystem = new LimeBattleSystem(gameConfig);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => battleSystem.CreateRandomHeroTypeList(5));
            Assert.IsTrue(battleSystem.CreateRandomHeroTypeList(4).Count == 4);
        }
    }
}
