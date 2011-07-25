using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Cyclops.MainApplication.Controls
{
    public class ChatFlowDocumentScrollViewer : FlowDocumentScrollViewer
    {
        public ChatFlowDocumentScrollViewer()
        {
        }
        
        /// <summary>
        /// Backing store for the <see cref="ScrollViewer"/> property.
        /// </summary>
        private ScrollViewer scrollViewer;

        /// <summary>
        /// Gets the scroll viewer contained within the FlowDocumentScrollViewer control
        /// </summary>
        public ScrollViewer ScrollViewer
        {
            get
            {
                if (scrollViewer == null)
                {
                    DependencyObject obj = this;
                    do
                    {
                        if (VisualTreeHelper.GetChildrenCount(obj) > 0)
                            obj = VisualTreeHelper.GetChild(obj as Visual, 0);
                        else
                            return null;
                    } while (!(obj is ScrollViewer));
                    scrollViewer = obj as ScrollViewer;
                }
                return scrollViewer;
            }
        }

    }
}