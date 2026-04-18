using ArenaConsoleApp;
using ArenaConsoleApp.View;
using ArenaEngine.Core;
using FW4di.Dotnet.Core.DependencyInjection;

DIBindings.BindAllDependencies(new DIManager());
ViewLocator.CreateViewManager.ShowTerminal(args.Length == 0 ? 0 : (uint)ConvertHelper.StringToInt(args[0]));
