using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.MainApplication.Configuration;
using Cyclops.MainApplication.Options.Controls;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.Controls
{
    public static class ApplicationSounds
    {
        public static void PlayOnUserJoin(ChatAreaViewModel viewModel)
        {
            if (!Context.Main.IsApplicationInActiveState || !viewModel.IsActive)
                PlaySound(viewModel, Context.Settings.SoundOnUserJoin);
        }

        public static void PlayOnUserLeave(ChatAreaViewModel viewModel)
        {
            PlaySound(viewModel, Context.Settings.SoundOnUserLeave);
        }

        public static void PlayOnStatusChanging(ChatAreaViewModel viewModel)
        {
            PlaySound(viewModel, Context.Settings.SoundOnStatusChange);
        }

        public static void PlayOnSystemMessage(ChatAreaViewModel viewModel)
        {
            PlaySound(viewModel, Context.Settings.SoundOnSystemMessage);
        }

        public static void PlayOnIcomingPrivate(ChatAreaViewModel viewModel)
        {
            PlaySound(viewModel, Context.Settings.SoundOnIncomingPrivate);
        }

        public static void PlayOnIcomingPublic(ChatAreaViewModel viewModel)
        {
            PlaySound(viewModel, Context.Settings.SoundOnIncomingPublic);
        }

        private static ApplicationContext Context
        {
            get { return ApplicationContext.Current; }
        }

        private static void PlaySound(ChatAreaViewModel viewModel, string file)
        {
            if (Context.Settings.DisableAllSounds || Context.DisableAllSounds)
                return;

            if (Context.Settings.SoundEvenIfActive 
                || !Context.Main.IsApplicationInActiveState 
                || !viewModel.IsActive)
                SoundPlayer.Play(file);
        }
    }
}
