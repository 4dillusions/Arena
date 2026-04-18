/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
namespace App4di.Dotnet.ArenaEngine.Core;

public interface IRandomProvider
{
    int Next(int maxValue);
    int Next(int minValue, int maxValue);
}
