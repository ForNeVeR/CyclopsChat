namespace Cyclops.Xmpp.Data.Rooms;

/// <remarks>Descriptions taken from XEO-0045: https://xmpp.org/extensions/xep-0045.html</remarks>
public enum MucUserStatus
{
    /// <summary>
    /// <para>Stanza: message or presence</para>
    /// <para>Context: Entering a room</para>
    /// <para>Purpose: Inform user that any occupant is allowed to see the user's full JID</para>
    /// </summary>
    AnyOccupantCanSeeUserFullJid = 100,

    /// <summary>
    /// <para>Stanza: message (out of band)</para>
    /// <para>Context: Affiliation change</para>
    /// <para>Purpose: Inform user that his or her affiliation changed while not in the room</para>
    /// </summary>
    AffiliationChangeWhileNotInRoom = 101,

    /// <summary>
    /// <para>Stanza: message</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that room now shows unavailable members</para>
    /// </summary>
    ConfigurationChangeShowUnavailable = 102,

    /// <summary>
    /// <para>Stanza: message</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that room now does not show unavailable members</para>
    /// </summary>
    ConfigurationChangeNotShowUnavailable = 103,

    /// <summary>
    /// <para>Stanza: message</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that a non-privacy-related room configuration change has occurred</para>
    /// </summary>
    ConfigurationChangeNonPrivacyRelated = 104,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Any room presence</para>
    /// <para>Purpose: Inform user that presence refers to itself</para>
    /// </summary>
    SelfReferringPresence = 110,

    /// <summary>
    /// <para>Stanza: message or initial presence</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that room logging is now enabled</para>
    /// </summary>
    ConfigurationChangeLoggingEnabled = 170,

    /// <summary>
    /// <para>Stanza: message</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that room logging is now disabled</para>
    /// </summary>
    ConfigurationChangeLoggingDisabled = 171,

    /// <summary>
    /// <para>Stanza: message</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that the room is now non-anonymous</para>
    /// </summary>
    ConfigurationChangeRoomIsNonAnonymous = 172,

    /// <summary>
    /// <para>Stanza: message</para>
    /// <para>Context: Configuration change</para>
    /// <para>Purpose: Inform occupants that the room is now semi-anonymous</para>
    /// </summary>
    ConfigurationChangeRoomIsSemiAnonymous = 173,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Entering a room</para>
    /// <para>Purpose: Inform user that a new room has been created</para>
    /// </summary>
    NewRoomCreated = 201,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Entering a room, changing nickname, etc.</para>
    /// <para>Purpose: Inform user that service has assigned or modified occupant's roomnick</para>
    /// </summary>
    ServiceHasAssignedARoomNick = 210,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Removal from room</para>
    /// <para>Purpose: Inform user that he or she has been banned from the room</para>
    /// </summary>
    Banned = 301,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Exiting a room</para>
    /// <para>Purpose: Inform all occupants of new room nickname</para>
    /// </summary>
    NewRoomNickname = 303,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Removal from room</para>
    /// <para>Purpose: Inform user that he or she has been kicked from the room</para>
    /// </summary>
    Kicked = 307,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Removal from room</para>
    /// <para>Purpose: Inform user that he or she is being removed from the room because of an affiliation change</para>
    /// </summary>
    RemovedFromRoomDueToAffiliationChange = 321,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Removal from room</para>
    /// <para>Purpose: Inform user that he or she is being removed from the room because the room has been changed to
    /// members-only and the user is not a member</para>
    /// </summary>
    RemovedFromRoomDueToRoomBecomingMemberOnly = 322,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Removal from room</para>
    /// <para>
    ///    Purpose: Inform user that he or she is being removed from the room because the MUC service is being shut
    ///    down
    /// </para>
    /// </summary>
    RemovedFromRoomDueToMucServiceShutdown = 332,

    /// <summary>
    /// <para>Stanza: presence</para>
    /// <para>Context: Removal from room</para>
    /// <para>
    ///     Purpose: Inform users that a user was removed because of an error reply (for example when an s2s link fails
    ///     between the MUC and the removed users server).
    /// </para>
    /// </summary>
    RemovedFromRoomDueToErrorReply = 333
}
