using ArenaEngine.Core;

namespace ArenaEngine.Model
{
    /// <summary>
    /// Arénában résztvevő hős
    /// van azonosítója életereje, szerepe
    /// az életerő alapján meghatározandó, hogy él e
    /// felület számára közvetít
    /// </summary>
    public class HeroDTO
    {
        public uint Id { get; set; }
        public byte Power { get; set; }
        HeroTypes HeroType { get; set; }
    }
}
