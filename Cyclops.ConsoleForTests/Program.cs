using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Cyclops.Core;
using Cyclops.Core.Avatars;
using Cyclops.Core.Configuration;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Resource;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Resource.Registration;
using Cyclops.Core.Resource.Security;
using Cyclops.Core.Resource.Smiles;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication;

namespace Cyclops.ConsoleForTests
{
    class Program
    {
        private static IUserSession session;
        private static readonly RegistrationManager manager = new RegistrationManager(new TripleDesStringEncryptor(), null);

        static void Main(string[] args)
        {
            DateTime date;
            bool success = DateTime.TryParseExact("20110507T10:43:00", "yyyyMMddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}
