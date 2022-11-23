using System;
using System.Collections.Generic;

namespace ArenaEngine.Core
{
    public static class RecruitmentManager<TTypes> where TTypes : Enum
    {
        /// <summary>
        /// Beadott enum alapján az enum itemek közül random itemeket generál N számban
        /// </summary>
        /// <param name="listSize"> N db enum itemekből álló listának a mérete </param>
        /// <returns> visszaadja a random generált enum itemek listáját </returns>
        public static List<TTypes> CreateRandomTypeList(uint listSize)
        {
            var result = new List<TTypes>();
            var random = new Random();

            var enumCount = Enum.GetValues(typeof(TTypes)).Length;
            for (var i = 0; i < listSize; i++)
            {
                var randEnumItemIndex = random.Next(enumCount);
                result.Add((TTypes)Enum.ToObject(typeof(TTypes), randEnumItemIndex));
            }

            return result;
        }
    }
}
