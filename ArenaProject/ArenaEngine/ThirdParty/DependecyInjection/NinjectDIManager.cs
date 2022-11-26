using Ninject;
using Ninject.Modules;
using System.Reflection;

namespace ArenaEngine.ThirdParty.DependecyInjection;

public class NinjectDIManager : NinjectModule, IDIManager
{
    /// <summary> 
    /// these are static because of Ninject initialization logic. Ninject create new instances every init and we have to store original one.
    /// </summary>
    static Action? bindings;
    static StandardKernel? kernel;

    #region Ninject
    /// <summary> 
    /// Will call automatically by Ninject
    /// </summary>
    public override void Load()
    {
        bindings?.Invoke();
    }
    #endregion

    #region IDIManager
    public T GetDependency<T>() => kernel!.Get<T>();

    public void Init(Action? bindingsParam)
    {
        bindings = bindingsParam;

        kernel = new StandardKernel();
        kernel.Load(Assembly.GetExecutingAssembly());
    }

    public void Bind<TInterface, TImplementation>(DILifetimeScopes scope) where TImplementation : TInterface
    {
        switch (scope)
        {
            case DILifetimeScopes.Transient: kernel?.Bind<TInterface>().To<TImplementation>().InTransientScope(); break;
            case DILifetimeScopes.Singleton: kernel?.Bind<TInterface>().To<TImplementation>().InSingletonScope(); break;
            case DILifetimeScopes.Thread: kernel?.Bind<TInterface>().To<TImplementation>().InThreadScope(); break;
        }
    }

    public void Bind<TInterface, TImplementation>(DILifetimeScopes scope, TImplementation instance) where TImplementation : TInterface, ICloneable
    {
        switch (scope)
        {
            case DILifetimeScopes.Transient: kernel?.Bind<TInterface>().ToMethod(_ => (TInterface)instance.Clone()).InTransientScope(); break;
            case DILifetimeScopes.Singleton: kernel?.Bind<TInterface>().ToMethod(_ => instance).InSingletonScope(); break;
            case DILifetimeScopes.Thread: throw new NotImplementedException(); //kernel.Bind<TInterface>().ToMethod(context => instance).InThreadScope(); break;
        }
    }
    #endregion
}