/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Core;

namespace App4di.Dotnet.ArenaEngine.Model;

/// <summary>
/// The hero in arena
/// it is a data transfering object for game logic and others
/// has id, power, type
/// is alive or not depending on power
/// </summary>
public class HeroDTO
{
    public uint Id { get; set; }
    public int Power { get; set; }
    public HeroTypes HeroType { get; set; }
    public bool IsAlive { get; set; } = true;
}
