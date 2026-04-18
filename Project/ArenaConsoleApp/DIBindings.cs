/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaConsoleApp.View;
using App4di.Dotnet.ArenaEngine.Controller;
using App4di.Dotnet.ArenaEngine.Core;
using App4di.Dotnet.ArenaEngine.Service;
using FW4di.Dotnet.Core.DependencyInjection;

namespace App4di.Dotnet.ArenaConsoleApp;

public static class DIBindings
{
    public static void BindAllDependencies(IDIManager di)
    {
        di.Init
        (
            () =>
            {
                ViewLocator.Init(di);

                di.Bind<IArenaLogFormatter, ArenaLogFormatter>(DILifetimeScopes.Singleton);
                di.Bind<IRandomProvider, SystemRandomProvider>(DILifetimeScopes.Singleton);
                di.Bind<IBattleSystem, BattleSystem>(DILifetimeScopes.Singleton);
                di.Bind<GameConfigDTO, GameConfigDTO>(DILifetimeScopes.Singleton);
                   
                di.Bind<ViewManager, ViewManager>(DILifetimeScopes.Transient);
                di.Bind<Terminal, Terminal>(DILifetimeScopes.Transient);
                di.Bind<Arena, Arena>(DILifetimeScopes.Transient);
            }
        );
    }
}
