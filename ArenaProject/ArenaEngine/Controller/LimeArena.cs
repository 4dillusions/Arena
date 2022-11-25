using ArenaEngine.Model;
using ArenaEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaEngine.Controller
{
    /// <summary>
    /// control the game
    /// contains heroes list
    /// get heroes count
    /// select heroes for 1v1 battle
    /// play turns
    /// stop the game
    /// </summary>
    public class LimeArena
    {
        private readonly IBattleSystem battleSystem;

        public uint ArenaHeroesCount { get; set; } = 1000;
        private List<HeroDTO> heroList;

        public event EventHandler<Tuple<string, ConsoleColor>> OnLogMessage;

        public LimeArena(IBattleSystem battleSystem)
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

            WriteLog( "Játék indul.");

            int roundCounter = 1;
            do //turns
            {
                WriteLog("\n" + roundCounter++ + ". kör");
                WriteLog("Number of heroes in arena: " + heroList.Count);

                var battleHeroes = battleSystem.SelectHeroesForBattle(ref heroList);
                battleHeroes[0].Description = "attacker";
                battleHeroes[1].Description = "defender";
                WriteHeroesStatsLog("Selected heroes for battle:", battleHeroes);

                battleSystem.PlayBattle(battleHeroes[0], battleHeroes[1]);
                WriteHeroesStatsLog("Heroes state after the battle:", battleHeroes);

                battleSystem.GoRestHeroesAfterBattle(battleHeroes, ref heroList);

            } while (heroList.Count > 1);

            WriteLog("\nGame over!");

            if (heroList.Count == 1)
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
            return $"{hero.Id} . {hero.HeroType} hero, power: {hero.Power} [{(hero.IsAlive ? "live" : "died")}] - {hero.Description}";
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
}
