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
    private readonly IArenaLogFormatter logFormatter;
    private readonly List<HeroDTO> heroList = [];

    public uint ArenaHeroesCount { get; set; } = 1000;

    public event EventHandler<Tuple<string, ConsoleColor>>? OnLogMessage;

    public Arena(IBattleSystem battleSystem, IArenaLogFormatter logFormatter)
    {
        this.battleSystem = battleSystem;
        this.logFormatter = logFormatter;
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
            heroList.Clear();
            heroList.AddRange(battleSystem.CreateRandomHeroList(ArenaHeroesCount));
        }
        catch (ArgumentOutOfRangeException)
        {
            WriteLog("The specified number of heroes exceeds the configured maximum.");
            return false;
        }

        return true;
    }

    public void GameLoop()
    {
        if (!Init())
            return;

        WriteLog("Game started.");

        int roundCounter = 1;
        while (heroList.Count > 1)
        {
            var roundResult = RunRound(roundCounter++);
            if (roundResult == null)
                break;

            LogRound(roundResult);
        }

        WriteLog("\nGame over!");

        if (heroList.Count == 1)
        {
            var winner = heroList.First();
            WriteLog("Winner: " + logFormatter.FormatHero(winner, "Laurel wreath"), ConsoleColor.Cyan);
        }
        else
            WriteLog("Nobody survived the game!");
    }

    private ArenaRoundResult? RunRound(int roundCounter)
    {
        WriteLog($"\n{roundCounter}. turn");
        WriteLog("Number of heroes in arena: " + heroList.Count);

        var selection = battleSystem.SelectHeroesForBattle(heroList);
        if (selection.BattleHeroes.Count != 2)
        {
            WriteLog("Unable to select two heroes for battle!", ConsoleColor.Red);
            return null;
        }

        battleSystem.RestHeroes(selection.RemainingHeroes);
        battleSystem.PlayBattle(selection.BattleHeroes[0], selection.BattleHeroes[1]);
        var survivingHeroes = battleSystem.GetSurvivingHeroesAfterBattle(selection.BattleHeroes);

        heroList.Clear();
        heroList.AddRange(selection.RemainingHeroes);
        heroList.AddRange(survivingHeroes);

        return new ArenaRoundResult
        {
            RoundNumber = roundCounter,
            Attacker = selection.BattleHeroes[0],
            Defender = selection.BattleHeroes[1]
        };
    }

    private void LogRound(ArenaRoundResult roundResult)
    {
        WriteHeroesStatsLog
        (
            "Selected heroes for battle:",
            [
                (roundResult.Attacker, "attacker"),
                (roundResult.Defender, "defender")
            ]
        );

        WriteHeroesStatsLog
        (
            "Heroes state after the battle:",
            [
                (roundResult.Attacker, "attacker"),
                (roundResult.Defender, "defender")
            ]
        );
    }

    private void WriteLog(string message, ConsoleColor color = ConsoleColor.Yellow)
    {
        OnLogMessage?.Invoke(this, Tuple.Create(message, color));
    }

    private void WriteHeroesStatsLog(string title, IEnumerable<(HeroDTO Hero, string Role)> heroes)
    {
        WriteLog(title);

        foreach (var (hero, role) in heroes)
            WriteLog(logFormatter.FormatHero(hero, role), hero.IsAlive ? ConsoleColor.Green : ConsoleColor.Red);
    }
}
