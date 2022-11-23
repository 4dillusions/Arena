using System;
using System.Reflection;
using Ninject;
using Ninject.Modules;

namespace ArenaEngine.ThirdParty.DependecyInjection
{
    public class NinjectDIManager : NinjectModule, IDIManager
    {
        /// <summary> 
        /// azért static-ek, mert az initnél a Ninject újrapéldányosítja az osztályt és az utoljára
        /// megadott állapotot vhogy muszáj megjegyezni
        /// </summary>
        static Action bindings;
        static StandardKernel kernel;

        #region Ninject
        /// <summary> 
        /// Ezt a Ninject automatikusan hívja meg
        /// </summary>
        public override void Load()
        {
            bindings.Invoke();
        }
        #endregion

        #region IDIManager
        public T GetDependency<T>() => kernel.Get<T>();

        public void Init(Action bindingsParam)
        {
            bindings = bindingsParam;

            kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
        }

        public void Bind<TInterface, TImplementation>(DILifetimeScopes scope) where TImplementation : TInterface
        {
            switch (scope)
            {
                case DILifetimeScopes.Transient: kernel.Bind<TInterface>().To<TImplementation>().InTransientScope(); break;
                case DILifetimeScopes.Singleton: kernel.Bind<TInterface>().To<TImplementation>().InSingletonScope(); break;
                case DILifetimeScopes.Thread: kernel.Bind<TInterface>().To<TImplementation>().InThreadScope(); break;
            }
        }

        public void Bind<TInterface, TImplementation>(DILifetimeScopes scope, TImplementation instance) where TImplementation : TInterface, ICloneable
        {
            switch (scope)
            {
                case DILifetimeScopes.Transient: kernel.Bind<TInterface>().ToMethod(context => (TInterface)instance.Clone()).InTransientScope(); break;
                case DILifetimeScopes.Singleton: kernel.Bind<TInterface>().ToMethod(context => instance).InSingletonScope(); break;
                case DILifetimeScopes.Thread: throw new NotImplementedException(); //kernel.Bind<TInterface>().ToMethod(context => instance).InThreadScope(); break;
            }
        }
        #endregion
    }
}
