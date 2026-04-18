/*
4di .NET Arena application
Copyright (c) by 4D Illusions. All rights reserved.
Released under the terms of the GNU General Public License version 3 or later.
*/
namespace App4di.Dotnet.ArenaEngine.Core;

public static class RecruitmentManager<TTypes> where TTypes : Enum
{
    /// <summary>
    /// Generate random enum items
    /// </summary>
    /// <param name="listSize"> size of random enum item list </param>
    /// <returns> A randomly generated list of enum values. </returns>
    public static List<TTypes> CreateRandomTypeList(uint listSize)
    {
        return CreateRandomTypeList(listSize, new SystemRandomProvider());
    }

    public static List<TTypes> CreateRandomTypeList(uint listSize, IRandomProvider randomProvider)
    {
        var result = new List<TTypes>();

        var enumCount = Enum.GetValues(typeof(TTypes)).Length;
        for (var i = 0; i < listSize; i++)
        {
            var randEnumItemIndex = randomProvider.Next(enumCount);
            result.Add((TTypes)Enum.ToObject(typeof(TTypes), randEnumItemIndex));
        }

        return result;
    }
}
