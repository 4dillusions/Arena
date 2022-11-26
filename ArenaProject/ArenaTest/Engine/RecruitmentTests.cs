using ArenaEngine.Core;
using ArenaEngine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArenaTest.Engine;

/// <summary>
/// generate random enum items into a list
/// </summary>
[TestClass]
public class RecruitmentTests
{
    [TestMethod]
    public void RandomRecruitment()
    {
        var heroTypeList = RecruitmentManager<HeroTypes>.CreateRandomTypeList(10);
            
        Assert.IsNotNull(heroTypeList);

        //after generate list with 10 items, there must be at least one of each item
        Assert.IsTrue(heroTypeList.Any(t => t == HeroTypes.KnightRider));
        Assert.IsTrue(heroTypeList.Any(t => t == HeroTypes.Swordsman));
        Assert.IsTrue(heroTypeList.Any(t => t == HeroTypes.Bowman));
    }

    [TestMethod]
    public void RandomRecruitmentFromArena()
    {
        var gameConfig = new GameConfigDTO() { MaximumArenaHeroCount = 4 };
        IBattleSystem battleSystem = new BattleSystem(gameConfig);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => battleSystem.CreateRandomHeroTypeList(5));
        Assert.IsTrue(battleSystem.CreateRandomHeroTypeList(4).Count == 4);
    }
}