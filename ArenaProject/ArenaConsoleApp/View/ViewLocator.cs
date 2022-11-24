using ArenaEngine.ThirdParty.DependecyInjection;

namespace ArenaConsoleApp.View
{
    public static class ViewLocator
    {
        private static IDIManager di;

        public static void Init(IDIManager dinjection)
        {
            ViewLocator.di = dinjection;
        }

        public static ViewManager CreateViewManager => di.GetDependency<ViewManager>();
    }
}
