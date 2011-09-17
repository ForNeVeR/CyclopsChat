using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core
{
    public enum Role
    {
        Banned = -3, //Special case
        Kicked = -2, //Special case
        Devoiced = -1, //Affiliation
        Regular = 0,
        Member,
        Moder, //Affiliation
        Admin,
        Owner
    }
}
