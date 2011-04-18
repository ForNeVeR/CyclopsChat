namespace Cyclops.Core
{
    public interface IConferenceMember : ISessionHolder
    {
        string Nick { get; }
        bool IsModer { get; }
        string StatusText { get; }
        string StatusType { get; }
        string AvatarUrl { get; }

        IEntityIdentifier ConferenceUserId { get; }
        IEntityIdentifier RealUserId { get; }
    }
}