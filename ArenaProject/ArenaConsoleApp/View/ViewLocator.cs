using ArenaEngine.ThirdParty.DependecyInjection;

namespace ArenaConsoleApp.View;

public static class ViewLocator
{
    private static IDIManager di = null!;

    public static void Init(IDIManager dinjection)
    {
        di = dinjection;
    }

    public static ViewManager CreateViewManager => di.GetDependency<ViewManager>();
}