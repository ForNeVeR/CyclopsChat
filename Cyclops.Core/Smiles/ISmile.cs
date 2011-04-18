using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Cyclops.Core.Smiles
{
    public interface ISmile
    {
        string[] Masks { get;}
        string File { get; }
        Bitmap Bitmap { get; }
    }
}
