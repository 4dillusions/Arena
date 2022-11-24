namespace ArenaTest.ThirdParty.Mock
{
    public interface IMockTestInterface
    {
        string Description { get; set; }
        // ReSharper disable once UnusedMember.Global
        int AuthorId { get; set; }

        int PosX { get; set; }
        // ReSharper disable once UnusedMember.Global
        int PosY { get; set; }

        int StringToInt(string str);
    }
}
