using System;
using System.IO;
using System.Reflection;

namespace Cyclops.Core.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// Reads a content from resource file in specific assembly 
        /// </summary>
        public static string ReadFromResource(string path, Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(path);
            if (stream == null)
                throw new ArgumentException("Cannot find resource by path in " + assembly);
            using (var sr = new StreamReader(stream))
                return sr.ReadToEnd();
        }
    }
}