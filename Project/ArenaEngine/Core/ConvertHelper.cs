/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
namespace App4di.Dotnet.ArenaEngine.Core;

public static class ConvertHelper
{
    public static int StringToInt(string? text)
    {
        if (!int.TryParse(text, out var result))
            return 0;

        return result;
    }
}
