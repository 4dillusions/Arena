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
        public int Power { get; set; }
        public HeroTypes HeroType { get; set; }
        public bool IsAlive { get; set; } = true;
        public string Description { get; set; } 
    }
}
