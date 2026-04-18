/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaConsoleApp;
using App4di.Dotnet.ArenaConsoleApp.View;
using App4di.Dotnet.ArenaEngine.Core;
using FW4di.Dotnet.Core.DependencyInjection;

DIBindings.BindAllDependencies(new DIManager());
ViewLocator.CreateViewManager.ShowTerminal(args.Length == 0 ? 0 : (uint)ConvertHelper.StringToInt(args[0]));
