/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.ArenaConsoleApp.View;

public static class ViewLocator
{
    private static IDIManager di = null!;

    public static void Init(IDIManager dinjection)
    {
        di = dinjection;
    }

    public static ViewManager CreateViewManager => di.GetDependency<ViewManager>();
}
