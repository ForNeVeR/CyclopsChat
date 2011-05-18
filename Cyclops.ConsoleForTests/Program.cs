using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            A a = 10;
            Program p = new Program();
            p.GetType().GetProperty("A").SetValue(p, 10, null);
        }

        public A A { get; set; }
    }

    public class A
    {
        public int Int { get; set; }

        public static implicit operator A(int a)
        {
            return new A {Int = a};
        }
    }
}
