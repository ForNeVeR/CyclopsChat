using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Cyclops.Core;
using Cyclops.MainApplication.ViewModel;

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
            dlg.DataContext = new VcardViewModel(target, dlg.Close, !readonlyMode);
            if (!readonlyMode)
                dlg.ShowDialog();
            else
                dlg.Show();
        }
    }
}
