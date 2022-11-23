using System;

namespace ArenaEngine.ThirdParty.DependecyInjection
{
    public class DIManager : IDIManager
    {
        #region DI
        readonly IDIManager di;

        /// <summary>
        /// Jelenleg a Ninject-et támogatja, de ez lecserélhető később Microsoft, vagy egyéb megoldásra
        /// </summary>
        public DIManager()
        {
            di = new NinjectDIManager();
        }
        #endregion

        #region IDIManager
        public void Bind<TInterface, TImplementation>(DILifetimeScopes scope) where TImplementation : TInterface
        {
            di.Bind<TInterface, TImplementation>(scope);
        }

        public void Bind<TInterface, TImplementation>(DILifetimeScopes scope, TImplementation instance) where TImplementation : TInterface, ICloneable
        {
            di.Bind<TInterface, TImplementation>(scope, instance);
        }

        public T GetDependency<T>()
        {
            return di.GetDependency<T>();
        }

        public void Init(Action bindings)
        {
            di.Init(bindings);
        }
        #endregion
    }
}
