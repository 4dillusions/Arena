namespace ArenaTest.ThirdParty.AutoMapper
{
    public class MapDTO2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as MapDTO2;
            if (other == null) 
                return false;

            return Equals(other);
        }

        protected bool Equals(MapDTO2 other)
        {
            return Id == other.Id && Name == other.Name && Description == other.Description && Author == other.Author && AuthorId == other.AuthorId && AuthorName == other.AuthorName;
        }

        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AuthorId;
                hashCode = (hashCode * 397) ^ (AuthorName != null ? AuthorName.GetHashCode() : 0);

                return hashCode;
            }
        }
    }
}
