using ArenaEngine.ThirdParty.AutoMapper;

namespace ArenaConsoleApp
{
    public class AutoMapperMappings : IAutoMapperMappings
    {
        public IAutoMapperManager Manager { get; }

        public AutoMapperMappings()
        {
            var mappingList = new MappingList();
            //mappingList.AddMapping<MapDTO2, MapDTO1>();
            //mappingList.AddMapping<MapDTO2, MapDTO2>();

            Manager = new AutoMapperManager(mappingList);
        }
    }
}
