using ArenaConsoleApp.View;
using ArenaEngine.Controller;
using ArenaEngine.Core;
using ArenaEngine.Service;
using ArenaEngine.ThirdParty.DependecyInjection;

namespace ArenaConsoleApp
{
    public static class DIBindings
    {
        public static void BindAllDepencies(IDIManager di)
        {
            di.Init
            (
                () =>
                {
                    ViewLocator.Init(di);

                    di.Bind<IBattleSystem, LimeBattleSystem>(DILifetimeScopes.Singleton);
                    di.Bind<GameConfigDTO, GameConfigDTO>(DILifetimeScopes.Singleton);
                   
                    di.Bind<ViewManager, ViewManager>(DILifetimeScopes.Transient);
                    di.Bind<Terminal, Terminal>(DILifetimeScopes.Transient);
                    di.Bind<LimeArena, LimeArena>(DILifetimeScopes.Transient);
                }
            );
        }
    }
}
