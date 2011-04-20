using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.Core;
using Cyclops.Core.Avatars;
using Cyclops.Core.Configuration;
using Cyclops.Core.Resource;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Resource.Security;
using Cyclops.Core.Resource.Smiles;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication;

namespace Cyclops.ConsoleForTests
{
    class Program
    {
        private static IUserSession session;

        static void Main(string[] args)
        {
            var encoder = new TripleDesStringEncryptor();
            session = new UserSession(encoder);
            session.Authenticated += SessionAuthenticated;
            session.AuthenticateAsync(new ConnectionConfig
                                          {
                                              User = "cyclops",
                                              Server = "jabber.uruchie.org",
                                              EncodedPassword = encoder.EncryptString("cyclops")
                                          });

            Console.ReadKey();
        }

        static void SessionAuthenticated(object sender, Core.CustomEventArgs.AuthenticationEventArgs e)
        {
            session.OpenConference(IdentifierBuilder.Create("main@conference.jabber.uruchie.org/console-tests"));
            var conference = session.Conferences.First();
            conference.Joined += conference_Joined;

        }

        static void conference_Joined(object sender, Core.CustomEventArgs.ConferenceJoinEventArgs e)
        {
            IAvatarsManager avatarsManager = new AvatarsManager(session);
            avatarsManager.SendAvatarRequest(IdentifierBuilder.Create("main@conference.jabber.uruchie.org/Un1c0rn"));
        }
    }
}
