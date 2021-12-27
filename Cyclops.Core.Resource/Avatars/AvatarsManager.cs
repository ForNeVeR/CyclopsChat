using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Helpers;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;

namespace Cyclops.Core.Resource.Avatars
{
    /// <summary>
    /// </summary>
    public class AvatarsManager : IAvatarsManager
    {
        private readonly ILogger logger;
        private readonly IUserSession session;

        public AvatarsManager(ILogger logger, IUserSession session)
        {
            this.logger = logger;
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

        public async Task SendAvatarRequest(IEntityIdentifier id)
        {
            var vCard = await Session.GetVCard(id);
            var image = defaultAvatar;
            if (vCard.Photo != null)
            {
                try
                {
                    var file = BuildPath(ImageUtils.CalculateSha1HashOfAnImage(vCard.Photo));
                    if (File.Exists(file))
                        try
                        {
                            File.Delete(file);}
                        catch //file is used by another process
                        {
                            return;
                        }
                    vCard.Photo.Save(file, ImageFormat.Png);
                    image = vCard.Photo.ToBitmapImage();
                }
                catch
                {
                    //TODO: log an exception
                }
            }

            AvatarChange(this, new AvatarChangedEventArgs(id, image));
        }

        public event EventHandler<AvatarChangedEventArgs> AvatarChange = delegate { };

        private static string BuildPath(string hash)
        {
            return Path.Combine(AvatarsFolder, hash.ToLower() + ".png");
        }

        internal bool ProcessAvatarChangeHash(IPresence pres, IEntityIdentifier conferenceId)
        {
            try
            {
                bool hasAvatar = false;
                var photoTagParent = pres.Elements.FirstOrDefault(i => i.Name == "x" && i["photo"] != null);
                if (photoTagParent != null)
                {
                    var from = pres.From.Equals(session.CurrentUserId) ? conferenceId : pres.From;

                    string sha1Hash = photoTagParent["photo"].InnerText;
                    if (!string.IsNullOrWhiteSpace(sha1Hash) && sha1Hash.Length == 40)
                    {

                        hasAvatar = true;
                        if (DoesCacheContain(sha1Hash))
                            AvatarChange(this, new AvatarChangedEventArgs(from, GetFromCache(sha1Hash)));

                        SendAvatarRequest(from).NoAwait(logger);
                    }
                    else
                        AvatarChange(this, new AvatarChangedEventArgs(from, defaultAvatar));
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
