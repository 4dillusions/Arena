namespace ArenaTest.ThirdParty.DependecyInjection
{
    public class Service1 : IService1
    {
        static int refCount;
        public int RefCounter1 => refCount;

        public Service1()
        {
            refCount++;
        }

        public static void Init()
        {
            refCount = 0;
        }
    }
}
