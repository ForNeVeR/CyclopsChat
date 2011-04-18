using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Cyclops.MainApplication.Configuration;

namespace Cyclops.Client.Configuration
{
    public class ProfileManager
    {
        private const string ProfileExtensions = ".config.xml";
        private static readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof (Profile));
        private readonly string profilesFolder;

        static ProfileManager()
        {
            DefaultInstance = new ProfileManager(ConfigurationManager.AppSettings["ProfilesFolder"]);
        }

        public ProfileManager(string folder)
        {
            profilesFolder = folder;
        }

        public static ProfileManager DefaultInstance { get; private set; }

        /// <summary>
        /// Get current profile
        /// </summary>
        public Profile CurrentProfile { get; set; }

        /// <summary>
        /// Get all saved profiles
        /// </summary>
        public IEnumerable<Profile> GetSavedProfiles()
        {
            foreach (string folder in Directory.GetDirectories(profilesFolder))
            {
                string[] files = Directory.GetFiles(folder, Path.GetFileName(folder) + ProfileExtensions);

                if (!files.IsNullOrEmpty())
                {
                    Profile profile = DeserializeProfile(files.First());
                    if (profile != null)
                        yield return profile;
                }
            }
        }

        private Profile DeserializeProfile(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.OpenRead(path);
                return xmlSerializer.Deserialize(stream) as Profile;
            }
            catch
            {
                return null;
            }
            finally
            {
                stream.Close();
            }
        }

        /// <summary>
        /// Save new or changed profile to the specific folder
        /// </summary>
        public void SaveProfile(Profile profile)
        {
            string path = CreatePath(profile);
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(path))
                File.Delete(path);

            using (FileStream stream = File.Create(path))
                xmlSerializer.Serialize(stream, profile);
        }

        private string CreatePath(Profile profile)
        {
            if (Path.GetInvalidFileNameChars().Any(i => profile.Name.Contains(i)))
                throw new ArgumentException("Profile name contains invalid symbol(s)");

            string path = Path.Combine(profilesFolder, profile.Name, profile.Name + ProfileExtensions);
            return path;
        }

        /// <summary>
        /// Totally removes user profile
        /// </summary>
        public void RemoveProfile(Profile profile)
        {
            string path = Path.GetDirectoryName(CreatePath(profile));

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
    }
}