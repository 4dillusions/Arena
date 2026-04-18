/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Model;

namespace App4di.Dotnet.ArenaEngine.Controller;

public interface IArenaLogFormatter
{
    string FormatHero(HeroDTO hero, string role);
}
