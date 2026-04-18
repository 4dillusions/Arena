/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Model;

namespace App4di.Dotnet.ArenaEngine.Controller;

public class ArenaLogFormatter : IArenaLogFormatter
{
    public string FormatHero(HeroDTO hero, string role)
    {
        return $"{hero.Id}. {hero.HeroType} hero, power: {hero.Power} [{(hero.IsAlive ? "live" : "died")}] - {role}";
    }
}
