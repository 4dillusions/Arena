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
        var validHeroTypes = Enum.GetValues<HeroTypes>();

        Assert.IsNotNull(heroTypeList);
        Assert.AreEqual(10, heroTypeList.Count);

        foreach (var heroType in heroTypeList)
            CollectionAssert.Contains(validHeroTypes, heroType);
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
