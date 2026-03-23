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
public class BattleSystem : IBattleSystem
{
    private readonly GameConfigDTO gameConfig;
    private readonly IRandomProvider randomProvider;

    public BattleSystem(GameConfigDTO gameConfig)
        : this(gameConfig, new SystemRandomProvider())
    {
    }

    public BattleSystem(GameConfigDTO gameConfig, IRandomProvider randomProvider)
    {
        this.gameConfig = gameConfig;
        this.randomProvider = randomProvider;
    }

    public List<HeroTypes> CreateRandomHeroTypeList(uint listSize)
    {
        if (listSize > gameConfig.MaximumArenaHeroCount)
            throw new ArgumentOutOfRangeException();

        return RecruitmentManager<HeroTypes>.CreateRandomTypeList(listSize, randomProvider);
    }

    public HeroDTO CreateHero(HeroTypes heroType)
    {
        var result = new HeroDTO
        {
            HeroType = heroType,
            IsAlive = true,
            Power = GetMaxPower(heroType)
        };

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
        var maxPower = GetMaxPower(hero.HeroType);

        //maximize the power
        if (hero.Power > maxPower)
            hero.Power = maxPower;

        //the power is less than quarter of the initial/maximum power then hero die
        if (hero.Power < maxPower / 4)
            hero.IsAlive = false;
    }

    public List<HeroDTO> SelectHeroesForBattle(List<HeroDTO> heroList)
    {
        var result = new List<HeroDTO>();

        if (heroList.Count < 2)
            return result;

        for (int i = 0; i < 2; i++)
        {
            var heroRandIndex = randomProvider.Next(heroList.Count);
            result.Add(heroList[heroRandIndex]);
            heroList.RemoveAt(heroRandIndex);
        }
            
        return result;
    }

    public void RestHeroes(List<HeroDTO> heroList)
    {
        foreach (var hero in heroList)
        {
            hero.Power += gameConfig.RestPowerIncrement; //rest time, increase power
            ValidateHero(hero); //maximize power
        }
    }

    public void PlayBattle(HeroDTO attacker, HeroDTO defender)
    {
        if (defender.HeroType == HeroTypes.KnightRider)
        {
            ResolveKnightRiderDefense(attacker, defender);
            ApplyPostBattlePowerLoss(attacker, defender);
            return;
        }

        if (attacker.HeroType == HeroTypes.KnightRider && defender.HeroType == HeroTypes.Swordsman)
        {
            ResolveKnightRiderIntoSwordsman(attacker, defender);
            ApplyPostBattlePowerLoss(attacker, defender);
            return;
        }

        ResolveStandardDefense(defender);
        ApplyPostBattlePowerLoss(attacker, defender);
    }

    public void GoBackHeroesAfterBattle(List<HeroDTO> battleHeroes, List<HeroDTO> heroList)
    {
        foreach (var hero in battleHeroes)
        {
            ValidateHero(hero);

            if (hero.IsAlive)
            {
                ValidateHero(hero); //maximize power
                heroList.Add(hero);
            }
        }
    }

    private short GetMaxPower(HeroTypes heroType)
    {
        return heroType switch
        {
            HeroTypes.KnightRider => gameConfig.KnightRiderMaxPower,
            HeroTypes.Swordsman => gameConfig.SwordsmanMaxPower,
            HeroTypes.Bowman => gameConfig.BowmanMaxPower,
            _ => throw new ArgumentOutOfRangeException(nameof(heroType), heroType, null)
        };
    }

    private void ResolveKnightRiderDefense(HeroDTO attacker, HeroDTO defender)
    {
        switch (attacker.HeroType)
        {
            case HeroTypes.KnightRider:
                defender.Power = 0;
                break;
            case HeroTypes.Swordsman:
                break;
            case HeroTypes.Bowman:
                defender.Power = BowmanLeavesKnightRiderAlive() ? defender.Power : 0;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attacker.HeroType), attacker.HeroType, null);
        }
    }

    private void ResolveKnightRiderIntoSwordsman(HeroDTO attacker, HeroDTO defender)
    {
        attacker.Power = 0;
    }

    private void ResolveStandardDefense(HeroDTO defender)
    {
        if (defender.HeroType == HeroTypes.Swordsman || defender.HeroType == HeroTypes.Bowman)
            defender.Power = 0;
    }

    private bool BowmanLeavesKnightRiderAlive()
    {
        return randomProvider.Next(1, 10) <= 6;
    }

    private void ApplyPostBattlePowerLoss(HeroDTO attacker, HeroDTO defender)
    {
        attacker.Power /= 2;
        defender.Power /= 2;

        ValidateHero(attacker);
        ValidateHero(defender);
    }
}
