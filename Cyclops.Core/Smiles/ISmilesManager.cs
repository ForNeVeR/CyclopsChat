using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.Smiles
{
    public interface ISmilesManager
    {
        ISmilePack[] GetSmilePacks();
    }
}
