using System.IO;
using System.Xml.Serialization;

namespace Cyclops.MainApplication.Options.Helpers
{
    public class Serializer<T> where T : class, new()
    {
        private readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        
        public T Deserialize(string path)
        {
            Stream stream = null;
            try
            {
                stream = File.OpenRead(path);
                return xmlSerializer.Deserialize(stream) as T;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (stream != null) stream.Close();
            }
        }

        /// <summary>
        /// Save new or changed profile to the specific folder
        /// </summary>
        public void Serialize(T obj, string path)
        {
            if (File.Exists(path))
                File.Delete(path);

            using (FileStream stream = File.Create(path))
                xmlSerializer.Serialize(stream, obj);
        }
    }
}
