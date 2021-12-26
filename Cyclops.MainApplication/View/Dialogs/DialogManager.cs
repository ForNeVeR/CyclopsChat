using System;
using System.Windows;
using Cyclops.MainApplication.ViewModel;
using Cyclops.Xmpp.Data;

namespace Cyclops.MainApplication.View.Dialogs
{
    public static class DialogManager
    {
        public static bool ShowStringInputDialog(string title, string initialValue, Action<string> okAction, Func<string, bool> validator)
        {
            return InputDialog.ShowForEdit(title, initialValue, okAction, validator);
        }

        public static void ShowUsersVcard(IEntityIdentifier target, bool readonlyMode = true)
        {
            VcardDialog dlg = new VcardDialog();
            dlg.Owner = Application.Current.MainWindow;
            dlg.DataContext = new VcardViewModel(ChatObjectFactory.GetDebugLogger(), target, dlg.Close, !readonlyMode);
            if (!readonlyMode)
                dlg.ShowDialog();
            else
                dlg.Show();
        }
    }
}
