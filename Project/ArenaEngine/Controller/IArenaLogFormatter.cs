using ArenaEngine.Model;

namespace ArenaEngine.Controller;

public interface IArenaLogFormatter
{
    string FormatHero(HeroDTO hero, string role);
}
