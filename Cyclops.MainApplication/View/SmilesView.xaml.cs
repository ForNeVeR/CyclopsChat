using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication.Controls;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for SmilesView.xaml
    /// </summary>
    public partial class SmilesView : UserControl
    {
        public SmilesView()
        {
            SmilePacks = ApplicationContext.Current.SmilePacks;
            DataContext = this;
            InitializeComponent();
        }

        public ISmilePack[] SmilePacks { get; set; }
        
        private void UserControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true && tabControl.Items.Count == 0) 
                foreach (var smilePack in SmilePacks.OrderByDescending(i => i.Meta.Name))
                {
                    var document = new SmilesTable { SmilePack = smilePack };
                    document.SmilePick += DocumentSmilePick;
                    tabControl.Items.Add(new TabItem
                    {
                        Content = new FlowDocumentScrollViewer { Document = document },
                        Header = smilePack.Meta.Name,
                    });
                }
        }

        private static void DocumentSmilePick(object sender, SmilesPickEventArgs e)
        {
            callbackStatic(e.Mask);
            popup.IsOpen = false;
        }

        private static Action<string> callbackStatic;
        private static Popup popup;
        public static string OpenForChoise(UIElement sender, Action<string> callback)
        {
            callbackStatic = callback;
            if (popup == null)
            {
                popup = new Popup
                            {
                                Child = new SmilesView(),
                                StaysOpen = false,
                                Height = 320,
                                Width = 500,
                                AllowsTransparency = true
                            };
            }
            popup.PlacementTarget = sender;
            popup.IsOpen = true;

            return string.Empty;
        }
    }
}
