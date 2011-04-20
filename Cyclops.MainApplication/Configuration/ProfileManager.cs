using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Cyclops.MainApplication.Configuration
{
    public static class ProfileManager
    {
        private const string ProfileExtensions = ".config.xml";
        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof (Profile));
        private static readonly string ProfilesFolder;

        static ProfileManager()
        {
            ProfilesFolder = ConfigurationManager.AppSettings["ProfilesFolder"];
        }
        
        /// <summary>
        /// Get all saved profiles
        /// </summary>
        public static IEnumerable<Profile> GetSavedProfiles()
        {
            foreach (string folder in Directory.GetDirectories(ProfilesFolder))
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

        private static Profile DeserializeProfile(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.OpenRead(path);
                return XmlSerializer.Deserialize(stream) as Profile;
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
        public static void SaveProfile(Profile profile)
        {
            string path = CreatePath(profile);
            string directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            if (File.Exists(path))
                File.Delete(path);

            using (FileStream stream = File.Create(path))
                XmlSerializer.Serialize(stream, profile);
        }

        private static string CreatePath(Profile profile)
        {
            if (Path.GetInvalidFileNameChars().Any(i => profile.Name.Contains(i)))
                throw new ArgumentException("Profile name contains invalid symbol(s)");

            string path = Path.Combine(ProfilesFolder, profile.Name, profile.Name + ProfileExtensions);
            return path;
        }

        /// <summary>
        /// Totally removes user profile
        /// </summary>
        public static void RemoveProfile(Profile profile)
        {
            string path = Path.GetDirectoryName(CreatePath(profile));

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }
    }
}