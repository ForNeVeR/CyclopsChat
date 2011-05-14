using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using Cyclops.Core.Smiles;

namespace Cyclops.Core.Resource.Smiles
{
    public class SmilesManager : ISmilesManager
    {
        public const string SmilesPackFolder = @"Data\Smiles";
        private static readonly XmlSerializer XmlSerializer = new XmlSerializer(typeof(SmilePack));

        public ISmilePack[] GetSmilePacks()
        {
            return GetJispFiles(SmilesPackFolder)
                .Select(Deserialize)
                .Where(item => item != null)
                .OrderByDescending(item => item.Meta.Name)
                .ToArray();
        }

        private static IEnumerable<string> GetJispFiles(string directory)
        {
            return Directory.GetFiles(directory, "*.jisp");
        }

        private static ISmilePack Deserialize(string jispFile)
        {
            SmilePack smilePack = null;
            try
            {
                //*.JISP file is an 7-zip archive
                using (ZipStorer zip = ZipStorer.Open(jispFile, FileAccess.Read))
                {
                    var dir = zip.ReadCentralDir();
                    var iconDefFile = dir.First(i => string.Equals(Path.GetFileName(i.FilenameInZip), "icondef.xml", StringComparison.InvariantCultureIgnoreCase));

                    using (var ms = new MemoryStream((int)iconDefFile.FileSize))
                    {
                        zip.ExtractFile(iconDefFile, ms);
                        ms.Position = 0;
                        smilePack = XmlSerializer.Deserialize(ms) as SmilePack;
                    }

                    if (smilePack != null)
                        foreach (var item in dir.Join(smilePack.Smiles, i => Path.GetFileName(i.FilenameInZip), i => i.File, (f, i) => new { Smile = i, ZipEntry = f }))
                        {
                            try
                            {
                                var ms = new MemoryStream();
                                zip.ExtractFile(item.ZipEntry, ms);
                                ((Smile) item.Smile).Bitmap = (Bitmap) Image.FromStream(ms);
                            }
                            catch
                            {
                                //todo: log an exception
                            }
                        }
                }
            }
            catch
            {
                return null;
            }
            if (smilePack != null)
                smilePack.SmilesForDeserialization = smilePack.SmilesForDeserialization.Where(i => i != null && i.Bitmap != null && i.Masks.Any()).ToArray();
            return smilePack;
        }
    }
}
