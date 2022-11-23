using System;

namespace ArenaEngine.ThirdParty.DependecyInjection
{
    public interface IDIManager
    {
        void Init(Action bindings);
        void Bind<TInterface, TImplementation>(DILifetimeScopes scope) where TImplementation : TInterface;
        void Bind<TInterface, TImplementation>(DILifetimeScopes scope, TImplementation instance) where TImplementation : TInterface, ICloneable;
        T GetDependency<T>();
    }
}
