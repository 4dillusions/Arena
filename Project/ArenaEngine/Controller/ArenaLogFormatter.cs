using ArenaEngine.Model;

namespace ArenaEngine.Controller;

public class ArenaLogFormatter : IArenaLogFormatter
{
    public string FormatHero(HeroDTO hero, string role)
    {
        return $"{hero.Id}. {hero.HeroType} hero, power: {hero.Power} [{(hero.IsAlive ? "live" : "died")}] - {role}";
    }
}
