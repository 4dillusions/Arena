/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Model;

namespace App4di.Dotnet.ArenaEngine.Controller;

public class ArenaRoundResult
{
    public required int RoundNumber { get; init; }
    public required HeroDTO Attacker { get; init; }
    public required HeroDTO Defender { get; init; }
}
