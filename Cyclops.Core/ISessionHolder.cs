namespace Cyclops.Core
{
    public interface ISessionHolder
    {
        IUserSession Session { get; }
    }
}