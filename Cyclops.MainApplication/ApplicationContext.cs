using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.Core.Smiles;

namespace Cyclops.MainApplication
{
    public class ApplicationContext
    {
        #region Singleton implementation
        private ApplicationContext()
        {
            SmilePacks = new ISmilePack[0];
        }

        private static ApplicationContext instance = null;
        public static ApplicationContext Current
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationContext();
                return instance;
            }
        } 
        #endregion

        public ISmilePack[] SmilePacks { get; set; }
    }
}
