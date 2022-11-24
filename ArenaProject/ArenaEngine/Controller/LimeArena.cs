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

        public event EventHandler<string> OnLogMessage;

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

            WriteLog( "Játék indul. Az arénában küzdő hősök száma: " + ArenaHeroesCount + "\n");

            int roundCounter = 1;
            do //körök
            {
                WriteLog("\n" + roundCounter++ + ". kör");

                var battleHeroes = battleSystem.SelectHeroesForBattle(ref heroList);
                battleHeroes[0].Description = "támadó";
                battleHeroes[1].Description = "védekező";
                WriteHeroesStatsLog("Küzdelemre kiválasztott hősök:", battleHeroes);

                battleSystem.PlayBattle(battleHeroes[1], battleHeroes[2]);
                WriteHeroesStatsLog("Küzdelem utáni állapot:", battleHeroes);

                battleSystem.GoRestHeroesAfterBattle(battleHeroes, ref heroList);

            } while (heroList.Count > 1);

            WriteLog("Vége a játéknak!");

            if (heroList.Count == 1)
                WriteLog("A győztes hős: " + HeroToString(heroList.First()));
            else
                WriteLog("Senki se élte túl a küzdelmeket!");
        }

        string HeroToString(HeroDTO hero)
        {
            return hero.Id + ".sz. " + hero.HeroType + " hős [" + (hero.IsAlive ? "él]" : "elesett] - " + hero.Description);
        }

        void WriteLog(string message)
        {
            OnLogMessage?.Invoke(this, message);
        }

        void WriteHeroesStatsLog(string title, List<HeroDTO> heroes)
        {
            WriteLog(title);

            foreach (var hero in heroes)
                WriteLog(HeroToString(hero));
        }
    }
}
