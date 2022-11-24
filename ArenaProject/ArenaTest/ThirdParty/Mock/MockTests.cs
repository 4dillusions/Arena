using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Mocker = Moq.Mock;

namespace ArenaTest.ThirdParty.Mock
{
    /// <summary>
    /// A mockolás kizárólag teszt környezetben lesz használva, ezért a teszteléshez
    /// bemelegítésképpen előtesztelem, ami egyben a működést is bemutatja
    /// </summary>
    [TestClass]
    public class MockTests
    {
        [TestMethod]
        public void GenerateObject()
        {
            //normál esetben ennek le se kéne fordulnia, de a Moq generál interfészből egy objektumot
            var mockObj1 = MockHelper.Generate<IMockTestInterface>();
            mockObj1.Description = "Description";
            mockObj1.PosX = 23;

            //default értékekkel rendelkező objektum keletkezik, aminek a tulajdonságait meg se lehet változtatni utólag
            Assert.IsTrue(mockObj1.Description == null && mockObj1.PosX == 0, "MockTests.GenerateObject - a mock objektum értékeit utólag módosítani lehetett");

            //másik objektum interfészből
            var mockObj2 = MockHelper.Generate<IMockTestInterface>();
            Mocker.Get(mockObj2).Setup(obj => obj.Description).Returns("Description");
            Mocker.Get(mockObj2).Setup(obj => obj.PosX).Returns(23);

            //setuppal lehet módosítani a generált objektum tulajdonságait
            Assert.IsTrue(mockObj2.Description == "Description" && mockObj2.PosX == 23, "MockTests.GenerateObject - a mock objektum értékeit nem lehetett felsetupolni");

            //később át lehet setupolni
            Mocker.Get(mockObj2).Setup(obj => obj.PosX).Returns(24);
            Assert.IsTrue(mockObj2.PosX == 24, "MockTests.GenerateObject - a mock objektum értékeit nem lehetett még egyszer felsetupolni");
        }

        [TestMethod]
        public void GenerateBehavior()
        {
            var mockObj = MockHelper.Generate<IMockTestInterface>(MockBehavior.Default);
            Mocker.Get(mockObj).Setup(obj => obj.StringToInt(It.IsAny<string>()))
                .Callback<string>((str) => { })
                .Returns(20);
            Assert.IsTrue(mockObj.StringToInt("fgdfgfdg") == 20, "MockTests.GenerateBehavior - a metódus viselkedését nem sikerült setupolni");
        }
    }
}
