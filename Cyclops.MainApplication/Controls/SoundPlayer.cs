using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Cyclops.MainApplication.Controls
{
    public static class SoundPlayer
    {
        private static readonly MediaPlayer Player = new MediaPlayer();
        public static void Play(string soundFile, bool throwOnError = false)
        {
            if (string.IsNullOrEmpty(soundFile))
                return;

            try
            {
                Player.Open(new Uri(soundFile));
                Player.Play();
            }
            catch(Exception)
            {
                if (throwOnError)
                    throw;
            }
        }

        public static void Stop()
        {
            Player.Stop();
        }
    }
}
