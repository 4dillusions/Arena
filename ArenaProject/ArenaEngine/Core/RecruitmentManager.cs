namespace ArenaEngine.Core;

public static class RecruitmentManager<TTypes> where TTypes : Enum
{
    /// <summary>
    /// Generate random enum items
    /// </summary>
    /// <param name="listSize"> size of random enum item list </param>
    /// <returns> returns random generated enum list </returns>
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