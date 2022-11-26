using ArenaEngine.Core;
using ArenaEngine.Model;

namespace ArenaEngine.Service;

/// <summary>
/// check game state
/// select 2 heroes for battle
/// runs turns
/// increase/decrease hero's power
/// runs battle
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

        //maximize the power
        if (hero.Power > maxPower)
            hero.Power = maxPower;

        //the power is less than quarter of the initial/maximum power then hero die
        if (hero.Power < maxPower / 4)
            hero.IsAlive = false;
    }

    public List<HeroDTO> SelectHeroesForBattle(ref List<HeroDTO>? heroList)
    {
        var result = new List<HeroDTO>();
        var random = new Random();

        if (heroList is {Count: < 2})
            return result;

        for (int i = 0; i < 2; i++)
        {
            if (heroList != null)
            {
                var heroRandIndex = random.Next(heroList.Count - 1);
                result.Add(heroList[heroRandIndex]);
                heroList.RemoveAt(heroRandIndex);
            }
        }
            
        return result;
    }

    public void RestHeroes(ref List<HeroDTO>? heroList)
    {
        if (heroList != null)
            foreach (var hero in heroList)
            {
                hero.Power += gameConfig.RestPowerIncrement; //rest time, increase power
                ValidateHero(hero); //maximize power
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

        //if knight rider is defending
        if (defender.HeroType == HeroTypes.KnightRider)
        {
            switch (attacker.HeroType)
            {
                case HeroTypes.KnightRider: defender.Power = 0; break; //knight rider attacks to other knight rider -> defender die
                case HeroTypes.Swordsman: /* nothing happens */ break; //swordsman attacks to knight rider
                case HeroTypes.Bowman: defender.Power = new Random().Next(1, 10) <= 6 ? defender.Power : 0;  break; //bowman attacks to knight rider -> knight rider dies 40%, lives 60%

                default: throw new ArgumentOutOfRangeException();
            }

            DecrementPowerAfterBattle();

            return;
        }

        //if swordsman or bowman is defending they die
        //except, knight rider is attacking to swordsman than knight rider dies
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

    public void GoBackHeroesAfterBattle(List<HeroDTO> battleHeroes, ref List<HeroDTO>? heroList)
    {
        foreach (var hero in battleHeroes)
        {
            ValidateHero(hero);

            if (hero.IsAlive)
            {
                ValidateHero(hero); //maximize power
                heroList?.Add(hero);
            }
        }
    }
}