using ArenaEngine.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArenaTest.Engine;

[TestClass]
public class ConverterTests
{
    [TestMethod]
    public void StringToInt()
    {
        Assert.IsTrue(ConvertHelper.StringToInt(null) == 0);
        Assert.IsTrue(ConvertHelper.StringToInt(string.Empty) == 0);
        Assert.IsTrue(ConvertHelper.StringToInt("dvsdvsdvsd") == 0);
        Assert.IsTrue(ConvertHelper.StringToInt("23") == 23);
    }
}