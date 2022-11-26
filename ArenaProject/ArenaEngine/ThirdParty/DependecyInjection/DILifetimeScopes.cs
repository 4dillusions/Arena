namespace ArenaEngine.ThirdParty.DependecyInjection;

public enum DILifetimeScopes
{
    /// <summary> always create new object </summary>
    Transient,

    /// <summary> only one object </summary>
    Singleton,

    /// <summary> one object per thread </summary>
    Thread,
}