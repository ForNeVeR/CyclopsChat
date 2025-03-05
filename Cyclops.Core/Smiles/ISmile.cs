using System.IO;

namespace Cyclops.Core.Smiles
{
    public interface ISmile
    {
        string[] Masks { get;}
        string File { get; }
        MemoryStream Stream { get; }
    }
}
