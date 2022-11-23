using ArenaEngine.ThirdParty.DependecyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArenaTest.ThirdParty.DependecyInjection
{
    /// <summary>
    /// Nem szokás 3rd party-t tesztelni -"vakon" megbízunk minden külsős függőségben :)-, 
    /// de itt a hozzáadott saját implementáció van tesztelve ebben a környezetben és a használata is látszik ebből
    /// </summary>
    [TestClass]
    public class DependencyInjectionTest
    {
        IDIManager CreateDIEnvironment()
        {
            Service1.Init();
            Service2.Init();

            return new DIManager();
        }

        [TestMethod]
        public void InstanceLifecycle()
        {
            IDIManager di = CreateDIEnvironment();
            di.Init
            (
                () =>
                {
                    di.Bind<IService1, Service1>(DILifetimeScopes.Singleton);
                    di.Bind<IService2, Service2>(DILifetimeScopes.Transient);
                }
            );

            di.GetDependency<IService1>();
            var ser11 = di.GetDependency<IService1>(); //ugyanazt a példányt kapja meg
            di.GetDependency<IService2>();
            var ser22 = di.GetDependency<IService2>(); //keletkezik még egy Service2

            Assert.IsTrue(ser11.RefCounter1 == 1, "DependencyInjectionTests.InstanceLifecycle - Service1 singleton, csak egy lehet");
            Assert.IsTrue(ser22.RefCounter2 == 2, "DependencyInjectionTests.InstanceLifecycle - Service2 kétszer lett létrehozva");
        }

        [TestMethod]
        public void InstanceInject()
        {
            string injectedName = "Prototype object";

            IDIManager di = CreateDIEnvironment();
            di.Init
            (
                () =>
                {
                    di.Bind<IService1, Service1>(DILifetimeScopes.Transient);
                    di.Bind<IService2, Service2>(DILifetimeScopes.Singleton);
                    di.Bind<Injected, Injected>(DILifetimeScopes.Transient, new Injected() { Name = injectedName });
                }
            );

            di.GetDependency<IService1>(); //keletkezik egy Service1
            var ser2 = di.GetDependency<IService2>(); //keletkezik egy Service2 singleton
            var ser11 = di.GetDependency<IService1>(); //keletkezik egy másik Service1

            Assert.IsTrue(ser11.RefCounter1 == 2, "DependencyInjectionTests.InstanceInject - Servie1-ből két példány keletkezik");
            Assert.IsTrue(ser2.RefCounter2 == 1, "DependencyInjectionTests.InstanceInject - Servie2 singleton, csak egy lehet");

            var injected1 = di.GetDependency<Injected>();
            var injected2 = di.GetDependency<Injected>();
            Assert.IsFalse(injected1.Equals(injected2), "DependencyInjectionTests.InstanceInject - Injected 1-2 transiens obj készítése nem sikerült");
            Assert.IsFalse(ReferenceEquals(injected1, injected2), "DependencyInjectionTests.InstanceInject - Injected típus nem transient életciklusú");

            IDIManager di2 = CreateDIEnvironment();
            di2.Init
            (
                () =>
                {
                    di2.Bind<Injected, Injected>(DILifetimeScopes.Singleton, new Injected() { Name = injectedName });
                }
            );

            var injected11 = di2.GetDependency<Injected>();
            var injected22 = di2.GetDependency<Injected>();
            Assert.IsTrue(injected11.Equals(injected22), "DependencyInjectionTests.InstanceInject - Injected 1-2 signleton kell legyen");
            Assert.IsTrue(ReferenceEquals(injected11, injected22), "DependencyInjectionTests.InstanceInject - Injected típus nem singleton életciklusú");
        }
    }
}
