using ArenaEngine.Model;
using ArenaEngine.Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArenaEngine.Controller
{
    /// <summary>
    /// vezérli a játékot
    /// tartalmazza a hősöket nyílvántartó heterogén kollekciót
    /// bekéri a hősök számát, toborozza őket(factory)
    /// kiválaszttat 1v1 harcra hősöket
    /// lejátsszatja a kört
    /// leállítja a játékot
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
                WriteLog("A megadott hősök száma minimum 2 kell legyen!!");
                return false;
            }

            try
            {
                heroList = battleSystem.CreateRandomHeroList(ArenaHeroesCount);
            }
            catch (ArgumentOutOfRangeException)
            {
                WriteLog("A megadott hősök száma nagyobb a maximálisan megadhatónál!");
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
            do //körök
            {
                WriteLog("\n" + roundCounter++ + ". kör");
                WriteLog("Az arénában küzdő hősök száma: " + heroList.Count);

                var battleHeroes = battleSystem.SelectHeroesForBattle(ref heroList);
                battleHeroes[0].Description = "támadó";
                battleHeroes[1].Description = "védekező";
                WriteHeroesStatsLog("Küzdelemre kiválasztott hősök:", battleHeroes);

                battleSystem.PlayBattle(battleHeroes[0], battleHeroes[1]);
                WriteHeroesStatsLog("Küzdelem utáni állapot:", battleHeroes);

                battleSystem.GoRestHeroesAfterBattle(battleHeroes, ref heroList);

            } while (heroList.Count > 1);

            WriteLog("\nVége a játéknak!");

            if (heroList.Count == 1)
            {
                var winner = heroList.First();
                winner.Description = "babérkoszorú";
                WriteLog("A győztes hős: " + HeroToString(winner), ConsoleColor.Cyan);
            }
            else
                WriteLog("Senki se élte túl a küzdelmeket!");
        }

        string HeroToString(HeroDTO hero)
        {
            return $"{hero.Id} .sz {hero.HeroType} hős, energia: {hero.Power} [{(hero.IsAlive ? "él" : "elesett")}] - {hero.Description}";
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
