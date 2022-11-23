namespace ArenaTest.ThirdParty.Mock
{
    public interface IMockTestInterface
    {
        string Description { get; set; }
        int AuthorId { get; set; }

        int PosX { get; set; }
        int PosY { get; set; }

        int StringToInt(string str);
    }
}
