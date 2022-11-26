using ArenaEngine.Model;
using ArenaEngine.Service;

namespace ArenaEngine.Controller;

/// <summary>
/// control the game
/// contains heroes list
/// get heroes count
/// select heroes for 1v1 battle
/// play turns
/// stop the game
/// </summary>
public class Arena
{
    private readonly IBattleSystem battleSystem;

    public uint ArenaHeroesCount { get; set; } = 1000;
    private List<HeroDTO>? heroList;

    public event EventHandler<Tuple<string, ConsoleColor>>? OnLogMessage;

    public Arena(IBattleSystem battleSystem)
    {
        this.battleSystem = battleSystem;
    }

    public bool Init()
    {
        if (ArenaHeroesCount < 2)
        {
            WriteLog("The number of specified heroes must be at least 2!");
            return false;
        }

        try
        {
            heroList = battleSystem.CreateRandomHeroList(ArenaHeroesCount);
        }
        catch (ArgumentOutOfRangeException)
        {
            WriteLog("The number of recommended heroes is greater than the maximum allowed!");
            return false;
        }

        return true;
    }

    public void GameLoop()
    {
        if (!Init())
            return;

        WriteLog( "Game started.");

        int roundCounter = 1;
        do //turns
        {
            WriteLog("\n" + roundCounter++ + ". turns");
            if (heroList != null)
            {
                WriteLog("Number of heroes in arena: " + heroList.Count);

                //Select 2 heroes for battle
                var battleHeroes = battleSystem.SelectHeroesForBattle(ref heroList);
                battleHeroes[0].Description = "attacker";
                battleHeroes[1].Description = "defender";
                WriteHeroesStatsLog("Selected heroes for battle:", battleHeroes);

                //other heroes are resting
                battleSystem.RestHeroes(ref heroList);

                //play 1v1 battle
                battleSystem.PlayBattle(battleHeroes[0], battleHeroes[1]);
                WriteHeroesStatsLog("Heroes state after the battle:", battleHeroes);

                //go back
                battleSystem.GoBackHeroesAfterBattle(battleHeroes, ref heroList);
            }
        } while (heroList is {Count: > 1});

        WriteLog("\nGame over!");

        if (heroList is {Count: 1})
        {
            var winner = heroList.First();
            winner.Description = "Laurel wreath";
            WriteLog("Winner: " + HeroToString(winner), ConsoleColor.Cyan);
        }
        else
            WriteLog("Nobody survived the game!");
    }

    string HeroToString(HeroDTO hero)
    {
        return $"{hero.Id}. {hero.HeroType} hero, power: {hero.Power} [{(hero.IsAlive ? "live" : "died")}] - {hero.Description}";
    }

    void WriteLog(string message, ConsoleColor color = ConsoleColor.Yellow)
    {
        OnLogMessage?.Invoke(this, Tuple.Create(message, color));
    }

    void WriteHeroesStatsLog(string title, List<HeroDTO> heroes)
    {
        WriteLog(title);

        foreach (var hero in heroes)
            WriteLog(HeroToString(hero), hero.IsAlive ? ConsoleColor.Green : ConsoleColor.Red);
    }
}