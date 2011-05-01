using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.MainApplication.View.Dialogs
{
    public static class DialogManager
    {
        public static bool ShowStringInputDialog(string title, string initialValue, Action<string> okAction, Func<string, bool> validator)
        {
            return InputDialog.ShowForEdit(title, initialValue, okAction, validator);
        }
    }
}
