using ArenaConsoleApp;
using ArenaConsoleApp.View;
using ArenaEngine.Core;
using FW4di.Dotnet.Core.DependencyInjection;

DIBindings.BindAllDepencies(new DIManager());
ViewLocator.CreateViewManager.ShowTerminal(args.Length == 0 ? 0 : (uint)ConvertHelper.StringToInt(args[0]));  //ViewLocator.CreateViewManager.ShowTerminal(4);