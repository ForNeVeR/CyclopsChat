using System;
using System.Collections.Generic;
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
            var c = Thread.CurrentThread.CurrentCulture;
            var encryptor = new TripleDesStringEncryptor();
            manager.RegisterNewUserAsync(new ConnectionConfig
                                             {
                                                 EncodedPassword = encryptor.EncryptString("testpassword"),
                                                 Server = "jabber.uruchie.org",
                                                 User = "testaccount",
                                             }, OnResult);


            Console.ReadKey();
        }

        private static void OnResult(RegistrationEventArgs obj)
        {
            
        }
    }
}
