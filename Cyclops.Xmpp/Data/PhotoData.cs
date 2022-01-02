namespace Cyclops.Xmpp.Data;

public class PhotoData
{
    public string PhotoSha1 { get; }

    public PhotoData(string photoSha1)
    {
        PhotoSha1 = photoSha1;
    }
}
