using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Cyclops.Core.Smiles;

namespace Cyclops.MainApplication.Controls
{
    public class SmilesTable : FlowDocument
    {
        public ISmilePack SmilePack
        {
            get { return (ISmilePack)GetValue(SmilePackProperty); }
            set { SetValue(SmilePackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmilePack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmilePackProperty =
            DependencyProperty.Register("SmilePack", typeof(ISmilePack), typeof(SmilesTable), new UIPropertyMetadata(OnInitializSmilePackStatic));


        private static void OnInitializSmilePackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SmilesTable)
                ((SmilesTable)d).OnInitializeSmilePack();
        }

        private void OnInitializeSmilePack()
        {
            if (SmilePack == null)
            {
                Blocks.Clear();
                return;
            }

            Paragraph p = new Paragraph();
            p.TextAlignment = System.Windows.TextAlignment.Left;
            p.LineHeight = 0.2;
            p.FontFamily = new System.Windows.Media.FontFamily("Tahoma");

            foreach (var smile in SmilePack.Smiles.OrderBy(i => i.Bitmap.Height).ThenBy(i => i.Bitmap.Width))
            {
                AnimatedImage image = new AnimatedImage();
                image.ToolTip = string.Join(", ", smile.Masks);
                image.AnimatedBitmap = smile.Bitmap;
                image.Width = smile.Bitmap.Width;
                image.Tag = smile.Masks.First();
                image.Height = smile.Bitmap.Height;
                image.MouseLeftButtonDown += InlineMouseLeftButtonDown;

                var inline = new InlineUIContainer(image);
                p.Inlines.Add(inline);
            }
            Blocks.Add(p);
        }

        void InlineMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnimatedImage image = sender as AnimatedImage;
            var mask = image.Tag.ToString();
            SmilePick(sender, new SmilesPickEventArgs(mask));
        }

        public event EventHandler<SmilesPickEventArgs> SmilePick = delegate { };
    }

    public class SmilesPickEventArgs : EventArgs
    {
        public string Mask { get; private set; }
        public SmilesPickEventArgs(string mask)
        {
            Mask = mask;
        }
    }
}
