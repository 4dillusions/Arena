/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
using App4di.Dotnet.ArenaEngine.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace App4di.Dotnet.ArenaTest.Engine;

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
