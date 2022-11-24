using ArenaConsoleApp.View;
using ArenaEngine.Core;
using ArenaEngine.ThirdParty.DependecyInjection;

namespace ArenaConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            DIBindings.BindAllDepencies(new DIManager());
            ViewLocator.CreateViewManager.ShowTerminal(args.Length == 0 ? 0 : (uint)ConvertHelper.StringToInt(args[0]));  //ViewLocator.CreateViewManager.ShowTerminal(4);
        }
    }
}
