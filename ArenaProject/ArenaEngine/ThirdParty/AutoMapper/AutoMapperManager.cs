using AutoMapper;
using System;

namespace ArenaEngine.ThirdParty.AutoMapper
{
    public class AutoMapperManager : IAutoMapperManager
    {
        public AutoMapperManager(MappingList mappingList)
        {
            Profile profile = new AutoMapperProfile { Mapper = mappingList };
            Mapper.Configuration.AddProfile(profile);
        }

        public TDestination DynamicMap<TSource, TDestination>(TSource source) => Mapper.DynamicMap<TSource, TDestination>(source);

        public TDestination DynamicMap<TDestination>(object source) => Mapper.DynamicMap<TDestination>(source);

        public object DynamicMap(object source, Type sourceType, Type destinationType) => Mapper.DynamicMap(source, sourceType, destinationType);

        public void DynamicMap<TSource, TDestination>(TSource source, TDestination destination) => Mapper.DynamicMap(source, destination);

        public void DynamicMap(object source, object destination, Type sourceType, Type destinationType) => Mapper.DynamicMap(source, destination, sourceType, destinationType);

        public TDestination Map<TSource, TDestination>(TSource source) => Mapper.Map<TSource, TDestination>(source);

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination) => Mapper.Map(source, destination);

        public object Map(object source, Type sourceType, Type destinationType) => Mapper.Map(source, sourceType, destinationType);

        public object Map(object source, object destination, Type sourceType, Type destinationType) => Mapper.Map(source, destination, sourceType, destinationType);
    }
}
