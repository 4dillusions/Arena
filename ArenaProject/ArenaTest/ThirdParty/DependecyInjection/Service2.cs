namespace ArenaTest.ThirdParty.DependecyInjection
{
    public class Service2 : IService2
    {
        static int refCount;
        public int RefCounter2 => refCount;

        public Service2()
        {
            refCount++;
        }

        public static void Init()
        {
            refCount = 0;
        }
    }
}
