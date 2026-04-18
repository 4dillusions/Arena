/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Core;
using App4di.Dotnet.ArenaEngine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App4di.Dotnet.ArenaTest.Engine;

/// <summary>
/// generate random enum items into a list
/// </summary>
[TestClass]
public class RecruitmentTests
{
    private sealed class FakeRandomProvider : IRandomProvider
    {
        private readonly Queue<int> values;

        public FakeRandomProvider(params int[] values)
        {
            this.values = new Queue<int>(values);
        }

        public int Next(int maxValue)
        {
            if (!values.TryDequeue(out var value))
                throw new AssertFailedException("No random values left for Next(maxValue).");

            if (value < 0 || value >= maxValue)
                throw new AssertFailedException($"Random value {value} is out of range for maxValue {maxValue}.");

            return value;
        }

        public int Next(int minValue, int maxValue)
        {
            if (!values.TryDequeue(out var value))
                throw new AssertFailedException("No random values left for Next(minValue, maxValue).");

            if (value < minValue || value >= maxValue)
                throw new AssertFailedException($"Random value {value} is out of range for interval [{minValue}, {maxValue}).");

            return value;
        }
    }

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
    public void RandomRecruitmentCanBeDeterministic()
    {
        var heroTypeList = RecruitmentManager<HeroTypes>.CreateRandomTypeList(3, new FakeRandomProvider(0, 2, 1));

        CollectionAssert.AreEqual
        (
            new[] { HeroTypes.KnightRider, HeroTypes.Bowman, HeroTypes.Swordsman },
            heroTypeList
        );
    }

    [TestMethod]
    public void RandomRecruitmentFromArena()
    {
        var gameConfig = new GameConfigDTO() { MaximumArenaHeroCount = 4 };
        IBattleSystem battleSystem = new BattleSystem(gameConfig);

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => battleSystem.CreateRandomHeroTypeList(5));
        Assert.IsTrue(battleSystem.CreateRandomHeroTypeList(4).Count == 4);
    }

    [TestMethod]
    public void CreateRandomHeroListCanBeDeterministic()
    {
        IBattleSystem battleSystem = new BattleSystem(new GameConfigDTO(), new FakeRandomProvider(2, 0, 1));

        var heroList = battleSystem.CreateRandomHeroList(3);

        Assert.AreEqual(3, heroList.Count);
        CollectionAssert.AreEqual(new uint[] { 1, 2, 3 }, heroList.Select(hero => hero.Id).ToArray());
        CollectionAssert.AreEqual
        (
            new[] { HeroTypes.Bowman, HeroTypes.KnightRider, HeroTypes.Swordsman },
            heroList.Select(hero => hero.HeroType).ToArray()
        );
        CollectionAssert.AreEqual(new[] { 100, 150, 120 }, heroList.Select(hero => hero.Power).ToArray());
    }
}
