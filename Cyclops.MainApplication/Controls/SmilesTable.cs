using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Smiles;
using Image = System.Windows.Controls.Image;

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

            foreach (var smile in SmilePack.Smiles/*.OrderBy(i => i.Bitmap.Height).ThenBy(i => i.Bitmap.Width)*/)
            {
                FrameworkElement smileElement = null;
                //if (IsGif(smile.Bitmap))
                //{
                    smileElement = new AnimatedImage();
                    ((AnimatedImage)smileElement).AnimatedBitmap = smile.Bitmap;
                //}
                //else
                //{
                //    smileElement = new Image();
                //    ((Image) smileElement).Source = smile.Bitmap.ToBitmapImage();
                //}

                smileElement.ToolTip = string.Join(", ", smile.Masks);
                smileElement.Width = smile.Bitmap.Width;
                smileElement.Tag = smile.Masks.First();
                smileElement.Height = smile.Bitmap.Height;
                smileElement.MouseLeftButtonDown += InlineMouseLeftButtonDown;
                var inline = new InlineUIContainer(smileElement);
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

        private static readonly Guid GiffFormatId = new Guid("{B96B3CB0-0728-11D3-9D7B-0000F81EF32E}");
        private bool IsGif(Bitmap bmp)
        {
            return bmp.RawFormat.Guid.Equals(GiffFormatId);
        }
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
