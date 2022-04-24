using System.IO;
using System.Xml.Serialization;

namespace Cyclops.MainApplication.Options.Helpers;

public class Serializer<T> where T : class, new()
{
    private readonly XmlSerializer xmlSerializer = new(typeof(T));

    public T? Deserialize(string path)
    {
        try
        {
            using var stream = File.OpenRead(path);
            return xmlSerializer.Deserialize(stream) as T;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Save new or changed profile to the specific folder
    /// </summary>
    public void Serialize(T obj, string path)
    {
        if (File.Exists(path))
            File.Delete(path);

        using var stream = File.Create(path);
        xmlSerializer.Serialize(stream, obj);
    }
}
