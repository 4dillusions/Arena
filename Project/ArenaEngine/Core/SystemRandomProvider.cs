/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
namespace App4di.Dotnet.ArenaEngine.Core;

public class SystemRandomProvider : IRandomProvider
{
    private readonly Random random = new();

    public int Next(int maxValue)
    {
        return random.Next(maxValue);
    }

    public int Next(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }
}
