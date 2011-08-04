using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Interop;
using Cyclops.MainApplication.Helpers;
using Point = System.Windows.Point;

namespace Cyclops.MainApplication.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfAnimatedControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfAnimatedControl;assembly=WpfAnimatedControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class AnimatedImage : System.Windows.Controls.Image, IDisposable
    {
        private BitmapSource[] _BitmapSources = null;
        private int _nCurrentFrame = 0;
        private bool _bIsAnimating = false;

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public bool IsAnimating
        {
            get { return _bIsAnimating; }
        }

        public AnimatedImage()
        {
            IsVisibleChanged += new DependencyPropertyChangedEventHandler(AnimatedImage_IsVisibleChanged);
            //Loaded += new RoutedEventHandler(AnimatedImage_Loaded);
        }

        void AnimatedImage_Loaded(object sender, RoutedEventArgs e)
        {
            var scrollViewer = this.TryFindParent<ChatFlowDocumentScrollViewer>();
            if (scrollViewer != null && scrollViewer.ScrollViewer != null)
            {
                scrollViewer.ScrollViewer.ScrollChanged += new ScrollChangedEventHandler(ScrollViewerScrollChanged);
            }
        }

        static AnimatedImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedImage), new FrameworkPropertyMetadata(typeof(AnimatedImage)));
        }
        public Bitmap AnimatedBitmap
        {
            get { return (Bitmap)GetValue(AnimatedBitmapProperty); }
            set { StopAnimate(); SetValue(AnimatedBitmapProperty, value); }
        }

        private static Dictionary<int, BitmapSource[]> cache = new Dictionary<int, BitmapSource[]>(20);

        public bool StaticByDefault { get; set; }

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimatedBitmapProperty =
            DependencyProperty.Register(
                "AnimatedBitmap", typeof(Bitmap), typeof(AnimatedImage),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAnimatedBitmapChanged)));

        private static void OnAnimatedBitmapChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AnimatedImage control = (AnimatedImage)obj;

            control.UpdateAnimatedBitmap();

            var e = new RoutedPropertyChangedEventArgs<Bitmap>(
                (Bitmap)args.OldValue, (Bitmap)args.NewValue, AnimatedBitmapChangedEvent);
            control.OnAnimatedBitmapChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent AnimatedBitmapChangedEvent = EventManager.RegisterRoutedEvent(
            "AnimatedBitmapChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<Bitmap>), typeof(AnimatedImage));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<Bitmap> AnimatedBitmapChanged
        {
            add { AddHandler(AnimatedBitmapChangedEvent, value); }
            remove { RemoveHandler(AnimatedBitmapChangedEvent, value); }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnAnimatedBitmapChanged(RoutedPropertyChangedEventArgs<Bitmap> args)
        {
            RaiseEvent(args);
        }

        private static bool IsGif(Bitmap bmp)
        {
            return bmp.RawFormat.Guid.Equals(ImageFormat.Gif.Guid);
        }

        private void ScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = ((ScrollViewer)sender);
            // position of your visual inside the scrollviewer    
            GeneralTransform childTransform = this.TransformToAncestor(scroll);
            Rect rectangle = childTransform.TransformBounds(new Rect(new Point(0, 0), this.RenderSize));
            //Check if the elements Rect intersects with that of the scrollviewer's
            Rect result = Rect.Intersect(new Rect(new Point(0, 0), scroll.RenderSize), rectangle);
            //if result is Empty then the element is not in view
            StopAnimationIfInvisible(result != Rect.Empty);
        }

        private void UpdateAnimatedBitmap()
        {
            if (StaticByDefault || !IsAnimated)
            {
                if (AnimatedBitmap == null)
                    Source = null;
                else
                    Source = ToBitmapSource(AnimatedBitmap);
                return;
            }
            
            int nTimeFrames = AnimatedBitmap.GetFrameCount(System.Drawing.Imaging.FrameDimension.Time);
            _nCurrentFrame = 0;
            if (nTimeFrames > 0)
            {
                int hash = AnimatedBitmap.GetHashCode();
                if (cache.ContainsKey(hash))
                {
                    _BitmapSources = cache[hash];
                }
                else
                {
                    _BitmapSources = new BitmapSource[nTimeFrames];

                    for (int i = 0; i < nTimeFrames; i++)
                    {
                        AnimatedBitmap.SelectActiveFrame(System.Drawing.Imaging.FrameDimension.Time, i);
                        _BitmapSources[i] = ToBitmapSource(AnimatedBitmap);
                    }
                    cache[hash] = _BitmapSources;
                }
                StartAnimate();
            }
        }
        
        private static BitmapSource ToBitmapSource(Bitmap bmp)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bmp.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        private delegate void VoidDelegate();

        private void OnFrameChanged(object o, EventArgs e)
        {
            if (!IsVisible)
            {
                return;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Background, new VoidDelegate(ChangeSource));
        }

        private bool isPaused = false;
        void AnimatedImage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            StopAnimationIfInvisible(IsVisible);
        }

        private void StopAnimationIfInvisible(bool isVisible)
        {
            if (isVisible)
            {
                if (isPaused)
                {
                    isPaused = false;
                    StartAnimate();
                }
            }
            else
            {
                if (IsAnimating)
                {
                    isPaused = true;
                    StopAnimate();
                }
            }
        }

        void ChangeSource()
        {
            if (!_bIsAnimating)
                return;
            Source = _BitmapSources[_nCurrentFrame++];
            _nCurrentFrame = _nCurrentFrame % _BitmapSources.Length;
            ImageAnimator.UpdateFrames(AnimatedBitmap);
        }

        public void StopAnimate()
        {
            _nCurrentFrame = 0;   
            if (!IsAnimated) return;
            if (_bIsAnimating)
            {
                ImageAnimator.StopAnimate(AnimatedBitmap, OnFrameChanged);
                _bIsAnimating = false;
            }
        }

        private bool IsAnimated
        {
            get { return AnimatedBitmap != null && IsGif(AnimatedBitmap); }
        }

        public void StartAnimate()
        {
            if (!IsAnimated) return;

            if (StaticByDefault)
            {
                StaticByDefault = false;
                UpdateAnimatedBitmap();
                return;
            }

            if (!_bIsAnimating)
            {
                _bIsAnimating = true;
                ImageAnimator.Animate(AnimatedBitmap, OnFrameChanged);
                ChangeSource();
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //if (AnimatedBitmap != null)
            //    AnimatedBitmap.Dispose();
            //AnimatedBitmap = null;
        }

        #endregion
    }
}
