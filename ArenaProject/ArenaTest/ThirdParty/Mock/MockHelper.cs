using Moq;

namespace ArenaTest.ThirdParty.Mock
{
    public static class MockHelper
    {
        public static T Generate<T>() where T : class
        {
            return new Mock<T>().Object;
        }

        public static T Generate<T>(MockBehavior behavior) where T : class
        {
            var mockRepo = new MockRepository(behavior)
            {
                DefaultValue = DefaultValue.Mock
            };

            return mockRepo.Create<T>().Object;
        }
    }
}
