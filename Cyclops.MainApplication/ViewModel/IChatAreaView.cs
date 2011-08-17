using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Cyclops.MainApplication.ViewModel
{
    public interface IChatAreaView
    {
        void InputboxFocus();
        int InputBoxSelectionLength { get; set; }
        int InputBoxSelectionStart { get; set; }
        void ClearOutputArea();
        void OpenMenuOnHyperlink(Uri uri);
        UIElement SmileElement { get; }
    }
}
