using AutoMapper;
using System;

namespace ArenaEngine.ThirdParty.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public MappingList Mapper { get; set; }
        
        protected override void Configure()
        {
            base.Configure();

            Mapper.Mappings.ForEach(mapPair => CreateMap(mapPair.ToValueTuple().Item1, mapPair.ToValueTuple().Item2));
        }
    }
}
