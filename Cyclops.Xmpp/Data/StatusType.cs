namespace Cyclops.Xmpp.Data;

public enum StatusType
{
    Online,
    Busy,
    Away,
    ExtendedAway,

    Chat
}

public static class StatusTypeEx
{
    public static string Map(this StatusType statusType) => statusType switch
    {
        StatusType.Online => "online",
        StatusType.Busy => "dnd",
        StatusType.Away => "away",
        StatusType.ExtendedAway => "xa",
        StatusType.Chat => "chat",
        _ => throw new ArgumentException(nameof(statusType))
    };

}
