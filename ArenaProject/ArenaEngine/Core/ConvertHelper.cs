namespace ArenaEngine.Core
{
    public static class ConvertHelper
    {
        public static int StringToInt(string text)
        {
            if (!int.TryParse(text, out var result))
                return 0;

            return result;
        }
    }
}
