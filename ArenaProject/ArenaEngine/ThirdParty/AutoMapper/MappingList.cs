using System;
using System.Collections.Generic;

namespace ArenaEngine.ThirdParty.AutoMapper
{
    public class MappingList
    {
        public List<Tuple<Type, Type>> Mappings { get; } = new List<Tuple<Type, Type>>();

        public void AddMapping<TSource, TDestination>()
        {
            Mappings.Add(Tuple.Create(typeof(TSource), typeof(TDestination)));
        }
    }
}
