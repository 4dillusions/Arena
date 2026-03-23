using ArenaEngine.Core;
using ArenaEngine.Model;
using ArenaEngine.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArenaTest.Engine;

[TestClass]
public class BattleTests
{
    private sealed class FakeRandomProvider : IRandomProvider
    {
        private readonly Queue<int> values;

        public FakeRandomProvider(params int[] values)
        {
            this.values = new Queue<int>(values);
        }

        public List<int> MaxValueCalls { get; } = new();

        public int Next(int maxValue)
        {
            MaxValueCalls.Add(maxValue);

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

    /// <summary>
    /// Select 2 heroes from arena (if it possible) for battle
    /// </summary>
    [TestMethod]
    public void SelectHeroesForBattle()
    {
        IBattleSystem battleSystem = new BattleSystem(new GameConfigDTO());
        var heroList = new List<HeroDTO>();

        Assert.IsTrue(battleSystem.SelectHeroesForBattle(heroList).Count == 0);

        heroList = battleSystem.CreateRandomHeroList(2);
        Assert.IsTrue(battleSystem.SelectHeroesForBattle(heroList).Count == 2);
        Assert.IsTrue(heroList?.Count == 0);

        heroList = battleSystem.CreateRandomHeroList(3);
        Assert.IsTrue(battleSystem.SelectHeroesForBattle(heroList).Count == 2);
        Assert.IsTrue(heroList?.Count == 1);
    }

    [TestMethod]
    public void SelectHeroesForBattleUsesFullListRange()
    {
        var randomProvider = new FakeRandomProvider(2, 0);
        IBattleSystem battleSystem = new BattleSystem(new GameConfigDTO(), randomProvider);
        List<HeroDTO> heroList =
        [
            new() { Id = 1, HeroType = HeroTypes.KnightRider, Power = 150 },
            new() { Id = 2, HeroType = HeroTypes.Swordsman, Power = 120 },
            new() { Id = 3, HeroType = HeroTypes.Bowman, Power = 100 }
        ];

        var battleHeroes = battleSystem.SelectHeroesForBattle(heroList);

        CollectionAssert.AreEqual(new[] { 3u, 1u }, battleHeroes.Select(hero => hero.Id).ToArray());
        CollectionAssert.AreEqual(new[] { 3, 2 }, randomProvider.MaxValueCalls);
        CollectionAssert.AreEqual(new[] { 2u }, heroList.Select(hero => hero.Id).ToArray());
    }

    /// <summary>
    /// rules:
    /// maximize the power
    /// the power is less than quarter of the initial/maximum power then hero die
    /// </summary>
    [TestMethod]
    public void ValidateHero()
    {
        var heroTypeCount = Enum.GetValues(typeof(HeroTypes)).Length;
        var heroes = new HeroDTO[heroTypeCount];
        for (var i = 0; i < heroTypeCount; i++)
            heroes[i] = new HeroDTO {HeroType = (HeroTypes) i, Power = (i + 1) * 10};

        //heroes are alive after creation
        Assert.IsTrue(heroes[(int) HeroTypes.KnightRider].IsAlive);
        Assert.IsTrue(heroes[(int) HeroTypes.Swordsman].IsAlive);
        Assert.IsTrue(heroes[(int) HeroTypes.Bowman].IsAlive);

        var gameConfig = new GameConfigDTO()
        {
            KnightRiderMaxPower = 10,
            SwordsmanMaxPower = 20,
            BowmanMaxPower = 30,
        };
        IBattleSystem battleSystem = new BattleSystem(gameConfig);

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

    /// <summary> rest heroes in hero list </summary>
    [TestMethod]
    public void RestHeroes()
    {
        var gameConfig = new GameConfigDTO();
        IBattleSystem battleSystem = new BattleSystem(gameConfig);
        
        var heroList = new List<HeroDTO>()
        {
            battleSystem.CreateHero(HeroTypes.KnightRider),
            battleSystem.CreateHero(HeroTypes.Bowman)
        };

        heroList[0].Power = 90; //KnightRider
        battleSystem.RestHeroes(heroList);
        
        Assert.IsTrue(heroList?.Count == 2);
        Assert.IsTrue(heroList[0].Power == 90 + gameConfig.RestPowerIncrement); //increased current power
        Assert.IsTrue(heroList[1].Power == gameConfig.BowmanMaxPower); //maximum power
    }

    /// <summary>
    /// 1v1 battle rules
    /// </summary>
    [TestMethod]
    public void PlayBattle()
    {
        IBattleSystem battleSystem = new BattleSystem(new GameConfigDTO());

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
    public void PlayBattleBowmanAgainstKnightRiderCanBeDeterministic()
    {
        var defenderLives = new BattleSystem(new GameConfigDTO(), new FakeRandomProvider(6));
        var livingBowman = defenderLives.CreateHero(HeroTypes.Bowman);
        var livingKnightRider = defenderLives.CreateHero(HeroTypes.KnightRider);

        defenderLives.PlayBattle(livingBowman, livingKnightRider);

        Assert.IsTrue(livingBowman.IsAlive);
        Assert.IsTrue(livingKnightRider.IsAlive);
        Assert.IsTrue(livingBowman.Power > 0);
        Assert.IsTrue(livingKnightRider.Power > 0);

        var defenderDies = new BattleSystem(new GameConfigDTO(), new FakeRandomProvider(7));
        var lethalBowman = defenderDies.CreateHero(HeroTypes.Bowman);
        var defeatedKnightRider = defenderDies.CreateHero(HeroTypes.KnightRider);

        defenderDies.PlayBattle(lethalBowman, defeatedKnightRider);

        Assert.IsTrue(lethalBowman.Power > 0);
        Assert.AreEqual(0, defeatedKnightRider.Power);
        Assert.IsFalse(defeatedKnightRider.IsAlive);
    }

    [TestMethod]
    public void AfterPlayBattle()
    {
        var gameConfig = new GameConfigDTO();
        IBattleSystem battleSystem = new BattleSystem(gameConfig);

        var knightRider = battleSystem.CreateHero(HeroTypes.KnightRider);
        var swordsman = battleSystem.CreateHero(HeroTypes.Swordsman);

        Assert.IsTrue(knightRider.Power > gameConfig.KnightRiderMaxPower / 2);
        Assert.IsTrue(swordsman.Power > gameConfig.SwordsmanMaxPower / 2);

        battleSystem.PlayBattle(swordsman, knightRider);

        Assert.IsTrue(knightRider.Power == gameConfig.KnightRiderMaxPower / 2);
        Assert.IsTrue(swordsman.Power == gameConfig.SwordsmanMaxPower / 2);
    }

    [TestMethod]
    public void GoBackAfterPlayBattle()
    {
        var gameConfig = new GameConfigDTO();
        IBattleSystem battleSystem = new BattleSystem(gameConfig);

        //after go back same power
        var heroList = new List<HeroDTO>();
        var knightRider = battleSystem.CreateHero(HeroTypes.KnightRider);
        var swordsman = battleSystem.CreateHero(HeroTypes.Swordsman);
        battleSystem.GoBackHeroesAfterBattle(new List<HeroDTO> { knightRider, swordsman }, heroList);
        Assert.IsTrue(heroList?.Count == 2);
        Assert.IsTrue(heroList[0].Power == gameConfig.KnightRiderMaxPower); //maximum power
        Assert.IsTrue(heroList[1].Power == gameConfig.SwordsmanMaxPower); //maximum power

        //after go back, power remains unchanged when it is already below maximum
        heroList = new List<HeroDTO>();
        knightRider = battleSystem.CreateHero(HeroTypes.KnightRider);
        swordsman = battleSystem.CreateHero(HeroTypes.Swordsman);
        knightRider.Power = 100;
        swordsman.Power = 80;
        battleSystem.GoBackHeroesAfterBattle(new List<HeroDTO> { knightRider, swordsman }, heroList);
        Assert.IsTrue(knightRider.Power == 100); //high power is same after go back
        Assert.IsTrue(swordsman.Power == 80); //high power is same after go back

        //after go back, low power hero died and didn't go back
        heroList = new List<HeroDTO>();
        knightRider = battleSystem.CreateHero(HeroTypes.KnightRider);
        swordsman = battleSystem.CreateHero(HeroTypes.Swordsman);
        knightRider.Power = 10;
        battleSystem.GoBackHeroesAfterBattle(new List<HeroDTO> { knightRider, swordsman }, heroList);
        Assert.IsTrue(heroList?.Count == 1); //one hero went back
        Assert.IsTrue(heroList[0].Id == swordsman.Id); //high power hero is alive and go back
        Assert.IsTrue(swordsman.Power == gameConfig.SwordsmanMaxPower); //and it keeps the same maximum power
        Assert.IsTrue(knightRider.Power == 10 && knightRider.IsAlive == false); //low power hero died
    }
}
