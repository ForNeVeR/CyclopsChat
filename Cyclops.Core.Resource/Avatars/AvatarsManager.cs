using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Media.Imaging;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Security;
using jabber;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource.Avatars
{
    /// <summary>
    /// </summary>
    public class AvatarsManager : IAvatarsManager
    {
        private readonly IUserSession session;

        public AvatarsManager(IUserSession session)
        {
            this.session = session;
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AvatarsFolder = Path.Combine(currentDir, AvatarsFolder);
            defaultAvatar = FromFile(Path.Combine(AvatarsFolder, DefaultAvatar));
        }

        private static BitmapImage FromFile(string file)
        {
            try
            {
                return Image.FromFile(file).ToBitmapImage();
            }
            catch
            {
                return null;
            } 
        }

        public static string AvatarsFolder = @"Data\Avatars";
        public const string DefaultAvatar = "default.png";

        private BitmapImage defaultAvatar;

        #region Implementation of ISessionHolder

        public IUserSession Session
        {
            get { return session; }
        }

        #endregion

        #region Implementation of IAvatarsManager

        public bool DoesCacheContain(string hash)
        {
            string file = BuildPath(hash);
            return File.Exists(file);
        }

        public BitmapImage GetFromCache(string hash)
        {
            string file = BuildPath(hash);
            if (!File.Exists(file))
                return defaultAvatar;
            return FromFile(file);
        }
        
        public void SendAvatarRequest(IEntityIdentifier id)
        {
            var client = ((UserSession)Session).JabberClient;
            VCardIQ vcardIq = new VCardIQ(client.Document);
            vcardIq.To = (JID)id;
            vcardIq.Type = jabber.protocol.client.IQType.get;
            client.Tracker.BeginIQ(vcardIq, OnVcard, new object());
        }

        public event EventHandler<AvatarChangedEventArgs> AvatarChange = delegate { }; 

        private void OnVcard(object sender, IQ iq, object data)
        {
            if (iq.Query is VCard)
            {
                BitmapImage image = defaultAvatar;
                string file = BuildPath(iq.From);
                VCard vcard = iq.Query as VCard;
                if (File.Exists(file))
                    try
                    {
                        File.Delete(file);
                    }
                    catch //file is used by another process
                    {
                        return;
                    }
                if (vcard.Photo != null && vcard.Photo.Image != null)
                {
                    try
                    {
                        vcard.Photo.Image.Save(BuildPath(CalculateSha1HashOfAnImage(vcard.Photo.Image)), ImageFormat.Png);
                        image = vcard.Photo.Image.ToBitmapImage();
                    }
                    catch
                    {
                        //TODO: log an exception
                    }
                }

                AvatarChange(this, new AvatarChangedEventArgs(iq.From, image ?? defaultAvatar));
            }
        }

        private static byte[] ImageToByte(Image img)
        {
            var converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        private static string CalculateSha1HashOfAnImage(Image image)
        {
            byte[] buffer = ImageToByte(image);
            var cryptoTransformSha1 =new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "").ToLower();
            return hash;
        }

        private static string BuildPath(string hash)
        {
            return Path.Combine(AvatarsFolder, hash.ToLower() + ".png");
        }

        internal bool ProcessAvatarChangeHash(Presence pres)
        {
            try
            {
                bool hasAvatar = false;
                var photoTagParent = pres.OfType<Element>().FirstOrDefault(i => i.Name == "x" && i["photo"] != null);
                if (photoTagParent != null)
                {

                    string sha1Hash = photoTagParent["photo"].InnerText;
                    if (!string.IsNullOrWhiteSpace(sha1Hash) && sha1Hash.Length == 40)
                    {

                        hasAvatar = true;
                        if (DoesCacheContain(sha1Hash))
                            AvatarChange(this, new AvatarChangedEventArgs(pres.From, GetFromCache(sha1Hash)));

                        SendAvatarRequest(pres.From);
                    }
                }

                return hasAvatar;
            }
            catch
            {
                //todo: log an exception
                return false;
            }
        }

        #endregion
    }
}
