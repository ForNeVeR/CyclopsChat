using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Smiles;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
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
    }

    public class SmilesGrid : UniformGrid
    {
        public ISmilePack SmilePack
        {
            get { return (ISmilePack)GetValue(SmilePackProperty); }
            set { SetValue(SmilePackProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmilePack.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmilePackProperty =
            DependencyProperty.Register("SmilePack", typeof(ISmilePack), typeof(SmilesGrid), new UIPropertyMetadata(OnInitializSmilePackStatic));


        private static void OnInitializSmilePackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SmilesGrid)
                ((SmilesGrid)d).OnInitializeSmilePack();
        }

        private void OnInitializeSmilePack()
        {
            if (SmilePack == null)
            {
                Children.Clear();
                return;
            }

            Columns = 10;
            
            
            foreach (var smile in SmilePack.Smiles)
            {
                AnimatedImage smileElement = new AnimatedImage();
                smileElement.StaticByDefault = true;
                smileElement.AnimatedBitmap = smile.Bitmap;
                smileElement.MaxWidth = 50;
                smileElement.MaxHeight = 50;
                smileElement.MinWidth = 22;
                smileElement.MinHeight = 22;
                smileElement.ToolTip = string.Join("  ", smile.Masks);
                smileElement.Width = smile.Bitmap.Width;
                smileElement.Tag = smile.Masks.First();
                smileElement.Stretch = Stretch.UniformToFill;

                //RenderOptions.SetBitmapScalingMode(smileElement, BitmapScalingMode.HighQuality);
                smileElement.Height = smile.Bitmap.Height;
                smileElement.MouseLeftButtonDown += InlineMouseLeftButtonDown;

                var border = new Border();
                border.Child = smileElement;
                border.Background = Brushes.White;
                border.MouseEnter += SmileElementMouseEnter;
                border.MouseLeave += SmileElementMouseLeave;
                border.BorderBrush = Brushes.LightGray;
                border.BorderThickness = new Thickness(1);

                Children.Add(border);
            }
        }

        void SmileElementMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AnimatedImage image = ((Border)sender).Child as AnimatedImage;
            image.StopAnimate();
            SetBorderColor(image, Brushes.LightGray);
        }

        void SmileElementMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AnimatedImage image = ((Border)sender).Child as AnimatedImage;
            image.StartAnimate();
            SetBorderColor(image, Brushes.Blue);
        }

        private void SetBorderColor(AnimatedImage image, Brush brush)
        {
            ((Border) image.Parent).BorderBrush = brush;
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
