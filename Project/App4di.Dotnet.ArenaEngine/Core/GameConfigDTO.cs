/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
namespace App4di.Dotnet.ArenaEngine.Core;

public class GameConfigDTO
{
    public uint MaximumArenaHeroCount { get; set; } = 50;

    public byte KnightRiderMaxPower { get; set; } = 150;
    public byte SwordsmanMaxPower { get; set; } = 120;
    public byte BowmanMaxPower { get; set; } = 100;

    public byte RestPowerIncrement { get; set; } = 10;
}
