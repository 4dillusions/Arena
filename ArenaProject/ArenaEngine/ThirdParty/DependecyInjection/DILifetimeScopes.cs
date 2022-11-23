namespace ArenaEngine.ThirdParty.DependecyInjection
{
    public enum DILifetimeScopes
    {
        /// <summary> mindig új példány keletkezik </summary>
        Transient,

        /// <summary> csak egy példány lehet </summary>
        Singleton,

        /// <summary> szálanként egy példány keletkezik </summary>
        Thread,
    }
}
