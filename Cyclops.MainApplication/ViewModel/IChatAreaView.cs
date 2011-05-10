using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.MainApplication.ViewModel
{
    public interface IChatAreaView
    {
        void InputboxFocus();
        int InputBoxSelectionLength { get; set; }
        int InputBoxSelectionStart { get; set; }
        void ClearOutputArea();
    }
}
