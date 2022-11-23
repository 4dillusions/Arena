using ArenaEngine.ThirdParty.AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArenaTest.ThirdParty.AutoMapper
{
    /// <summary>
    /// Nem szokás 3rd party-t tesztelni -"vakon" megbízunk minden külsős függőségben :)-, 
    /// de itt a hozzáadott saját implementáció van tesztelve ebben a környezetben és a használata is látszik ebből
    /// </summary>
    [TestClass]
    public class AutoMapperTests
    {
        IAutoMapperManager CreateMapEnvironment()
        {
            var mappingList = new MappingList();
            mappingList.AddMapping<MapDTO2, MapDTO1>();
            mappingList.AddMapping<MapDTO2, MapDTO2>();

            return new AutoMapperManager(mappingList);
        }

        MapDTO2 CreateDTO2()
        {
            return new MapDTO2()
            {
                Id = 122,
                Name = "DTO2",
                Description = "This is DTO2",
                Author = "Lime Corporation",
                AuthorId = 10,
                AuthorName = "Lime CTO"
            };
        }
        
        [TestMethod]
        public void MappingSameTypes()
        {
            var map = CreateMapEnvironment();
            var DTO2 = CreateDTO2();

            var DTO22 = map.Map<MapDTO2, MapDTO2>(DTO2);
            Assert.IsTrue(DTO2.Equals(DTO22), "AutoMapperTests.MappingSameTypes - nem sikerült DTO2-ből mappolni egy másik új DTO2 példányra");
            Assert.IsFalse(ReferenceEquals(DTO2, DTO22), "AutoMapperTests.MappingSameTypes - nem sikerült DTO2-ből mappolni egy új másik példányt");

            DTO2.Name = "DTO3";
            DTO2.Author = "Umbrella Corporation";
            var DTO33 = map.Map<MapDTO2, MapDTO2>(DTO2);
            Assert.IsTrue(DTO2.Equals(DTO33), "AutoMapperTests.MappingSameTypes - nem sikerült módosított DTO2-ből mappolni egy másik új DTO2 példányra");
            Assert.IsFalse(ReferenceEquals(DTO2, DTO33), "AutoMapperTests.MappingSameTypes - nem sikerült módosított DTO2-ből mappolni egy új másik példányt");
        }

        [TestMethod]
        public void MappingDifferentTypes()
        {
            var map = CreateMapEnvironment();
            var DTO2 = CreateDTO2();

            var DTO1 = map.Map<MapDTO2, MapDTO1>(DTO2);
            Assert.IsTrue(DTO1.Description == DTO2.Description && DTO1.AuthorId == DTO2.AuthorId, "AutoMapperTests.MappingDifferentTypes - nem sikerült DTO2-ből mappolni egy új DTO1 példányra");
        }
    }
}