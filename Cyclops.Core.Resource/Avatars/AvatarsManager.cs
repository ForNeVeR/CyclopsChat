using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Security;
using jabber;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource.Avatars
{
    /// <summary>
    /// TODO:
    /// Handle user updating his vcard
    /// </summary>
    public class AvatarsManager : IAvatarsManager
    {
        private readonly IUserSession session;

        public AvatarsManager(IUserSession session)
        {
            this.session = session;
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            AvatarsFolder = Path.Combine(currentDir, AvatarsFolder);
            defaultAvatar = FromFile(Path.Combine(AvatarsFolder, "default.png"));
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
        private BitmapImage defaultAvatar;

        #region Implementation of ISessionHolder

        public IUserSession Session
        {
            get { return session; }
        }

        #endregion

        #region Implementation of IAvatarsManager

        public BitmapImage GetFromCache(IEntityIdentifier id)
        {
            string file = BuildPath(id);
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
                    vcard.Photo.Image.Save(BuildPath(iq.From), ImageFormat.Png);
                    image = vcard.Photo.Image.ToBitmapImage();
                }

                AvatarChange(this, new AvatarChangedEventArgs(iq.From, image ?? defaultAvatar));
            }
        }
        
        private static string BuildPath(IEntityIdentifier id)
        {
            return Path.Combine(AvatarsFolder, Base64Helper.Encode(id.ToString()) + ".png");
        }

        #endregion
    }
}
