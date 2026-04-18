/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Model;

namespace App4di.Dotnet.ArenaEngine.Service;

public class BattleSelectionResult
{
    public required List<HeroDTO> BattleHeroes { get; init; }
    public required List<HeroDTO> RemainingHeroes { get; init; }
}
