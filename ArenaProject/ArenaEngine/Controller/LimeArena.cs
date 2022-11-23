using System.Collections;
using System.Collections.Generic;
using ArenaEngine.Model;
using ArenaEngine.Service;

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
        private readonly IBattleSystem _battleSystem;

        private Queue<HeroDTO> heroList = new Queue<HeroDTO>();

        public LimeArena(IBattleSystem battleSystem)
        {
            this._battleSystem = battleSystem;
        }
    }
}
