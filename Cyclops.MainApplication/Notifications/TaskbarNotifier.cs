using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Cyclops.MainApplication.Notifications
{
    /// <summary>
    /// A window that slides up from the bottom right corner of the desktop to
    /// notify the user of some event.
    /// </summary>
    public class TaskbarNotifier : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Internal states.
        /// </summary>
        private enum DisplayStates
        {
            Opening,
            Opened,
            Hiding,
            Hidden
        }

        private DispatcherTimer stayOpenTimer = null;
        private Storyboard storyboard;
        private DoubleAnimation animation;

        private double hiddenTop;
        private double openedTop;
        private EventHandler arrivedHidden;
        private EventHandler arrivedOpened;

        private readonly object locker = new object();

        public TaskbarNotifier()
        {
            this.Loaded += new RoutedEventHandler(TaskbarNotifier_Loaded);
        }

        private void TaskbarNotifier_Loaded(object sender, RoutedEventArgs e)
        {
            // Set initial settings based on the current screen working area.
            this.SetInitialLocations(false);

            // Start the window in the Hidden state.
            this.DisplayState = DisplayStates.Hidden;

            // Prepare the timer for how long the window should stay open.
            this.stayOpenTimer = new DispatcherTimer();
            this.stayOpenTimer.Interval = TimeSpan.FromMilliseconds(this.stayOpenMilliseconds);
            this.stayOpenTimer.Tick += new EventHandler(this.stayOpenTimer_Elapsed);

            // Prepare the animation to change the Top property.
            this.animation = new DoubleAnimation();
            Storyboard.SetTargetProperty(this.animation, new PropertyPath(Window.TopProperty));
            this.storyboard = new Storyboard();
            this.storyboard.Children.Add(this.animation);
            this.storyboard.FillBehavior = FillBehavior.Stop;

            // Create the event handlers for when the animation finishes.
            this.arrivedHidden = new EventHandler(this.Storyboard_ArrivedHidden);
            this.arrivedOpened = new EventHandler(this.Storyboard_ArrivedOpened);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            // For lack of a better way, bring the notifier window to the top whenever
            // Top changes.  Let me know if you have a better way.
            if (e.Property.Name == "Top")
            {
                if (((double)e.NewValue != (double)e.OldValue) && ((double)e.OldValue != this.hiddenTop))
                {
                    this.BringToTop();
                }
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            // No title bar or resize border.
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            // Don't show in taskbar.
            this.ShowInTaskbar = false;

            base.OnInitialized(e);
        }

        private int openingMilliseconds = 1000;
        /// <summary>
        /// The time the TaskbarNotifier window should take to open in milliseconds.
        /// </summary>
        public int OpeningMilliseconds
        {
            get { return this.openingMilliseconds; }
            set
            {
                this.openingMilliseconds = value;
                this.OnPropertyChanged("OpeningMilliseconds");
            }
        }

        private int hidingMilliseconds = 1000;
        /// <summary>
        /// The time the TaskbarNotifier window should take to hide in milliseconds.
        /// </summary>
        public int HidingMilliseconds
        {
            get { return this.hidingMilliseconds; }
            set
            {
                this.hidingMilliseconds = value;
                this.OnPropertyChanged("HidingMilliseconds");
            }
        }

        private int stayOpenMilliseconds = 1000;
        /// <summary>
        /// The time the TaskbarNotifier window should stay open in milliseconds.
        /// </summary>
        public int StayOpenMilliseconds
        {
            get { return this.stayOpenMilliseconds; }
            set
            {
                this.stayOpenMilliseconds = value;
                if (this.stayOpenTimer != null)
                    this.stayOpenTimer.Interval = TimeSpan.FromMilliseconds(this.stayOpenMilliseconds);
                this.OnPropertyChanged("StayOpenMilliseconds");
            }
        }

        private int leftOffset = 0;
        /// <summary>
        /// The space, if any, between the left side of the TaskNotifer window and the right side of the screen.
        /// </summary>
        public int LeftOffset
        {
            get { return this.leftOffset; }
            set
            {
                this.leftOffset = value;
                this.OnPropertyChanged("LeftOffset");
            }
        }

        private DisplayStates displayState;
        /// <summary>
        /// The current DisplayState
        /// </summary>
        private DisplayStates DisplayState
        {
            get
            {
                return this.displayState;
            }
            set
            {
                if (value != this.displayState)
                {
                    this.displayState = value;

                    // Handle the new state.
                    this.OnDisplayStateChanged();
                }
            }
        }

        private void SetInitialLocations(bool showOpened)
        {
            // Determine screen working area.
            System.Drawing.Rectangle workingArea = new System.Drawing.Rectangle((int)this.Left, (int)this.Top, (int)this.ActualWidth, (int)this.ActualHeight);
            workingArea = Screen.GetWorkingArea(workingArea);

            // Initialize the window location to the bottom right corner.
            this.Left = workingArea.Right - this.ActualWidth - this.leftOffset;

            // Set the opened and hidden locations.
            this.hiddenTop = workingArea.Bottom;
            this.openedTop = workingArea.Bottom - this.ActualHeight;

            // Set Top based on whether opened or hidden is desired
            if (showOpened)
                this.Top = openedTop;
            else
                this.Top = hiddenTop;
        }

        private void BringToTop()
        {
            // Bring this window to the top without making it active.
            this.Topmost = true;
            this.Topmost = false;
        }

        private void OnDisplayStateChanged()
        {
            // The display state has changed.

            // Unless the stortboard as already been created, nothing can be done yet.
            if (this.storyboard == null)
                return;

            // Stop the current animation.
            this.storyboard.Stop(this);

            // Since the storyboard is reused for opening and closing, both possible
            // completed event handlers need to be removed.  It is not a problem if
            // either of them was not previously set.
            this.storyboard.Completed -= arrivedHidden;
            this.storyboard.Completed -= arrivedOpened;

            if (this.displayState != DisplayStates.Hidden)
            {
                // Unless the window has just arrived at the hidden state, it must be
                // moving, and should be shown.
                this.BringToTop();
            }

            if (this.displayState == DisplayStates.Opened)
            {
                // The window has just arrived at the opened state.

                // Because the inital settings of this TaskNotifier depend on the screen's working area,
                // it is best to reset these occasionally in case the screen size has been adjusted.
                this.SetInitialLocations(true);

                if (!this.IsMouseOver)
                {
                    // The mouse is not within the window, so start the countdown to hide it.
                    this.stayOpenTimer.Stop();
                    this.stayOpenTimer.Start();
                }
            }
            else if (this.displayState == DisplayStates.Opening)
            {
                // The window should start opening.

                // Make the window visible.
                this.Visibility = Visibility.Visible;
                this.BringToTop();

                // Because the window may already be partially open, the rate at which
                // it opens may be a fraction of the normal rate.
                // This must be calculated.
                int milliseconds = this.CalculateMillseconds(this.openingMilliseconds, this.openedTop);

                if (milliseconds < 1)
                {
                    // This window must already be open.
                    this.DisplayState = DisplayStates.Opened;
                    return;
                }

                // Reconfigure the animation.
                this.animation.To = this.openedTop;
                this.animation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, milliseconds));

                // Set the specific completed event handler.
                this.storyboard.Completed += arrivedOpened;

                // Start the animation.
                this.storyboard.Begin(this, true);
            }
            else if (this.displayState == DisplayStates.Hiding)
            {
                // The window should start hiding.

                // Because the window may already be partially hidden, the rate at which
                // it hides may be a fraction of the normal rate.
                // This must be calculated.
                int milliseconds = this.CalculateMillseconds(this.hidingMilliseconds, this.hiddenTop);

                if (milliseconds < 1)
                {
                    // This window must already be hidden.
                    this.DisplayState = DisplayStates.Hidden;
                    return;
                }

                // Reconfigure the animation.
                this.animation.To = this.hiddenTop;
                this.animation.Duration = new Duration(new TimeSpan(0, 0, 0, 0, milliseconds));

                // Set the specific completed event handler.
                this.storyboard.Completed += arrivedHidden;

                // Start the animation.
                this.storyboard.Begin(this, true);
            }
            else if (this.displayState == DisplayStates.Hidden)
            {
                // Ensure the window is in the hidden position.
                SetInitialLocations(false);

                // Hide the window.
                this.Visibility = Visibility.Hidden;
                Close();
            }
        }

        private int CalculateMillseconds(int totalMillsecondsNormally, double destination)
        {
            if (this.Top == destination)
            {
                // The window is already at its destination.  Nothing to do.
                return 0;
            }

            double distanceRemaining = Math.Abs(this.Top - destination);
            double percentDone = distanceRemaining / this.ActualHeight;

            // Determine the percentage of normal milliseconds that are actually required.
            return (int)(totalMillsecondsNormally * percentDone);
        }

        protected virtual void Storyboard_ArrivedHidden(object sender, EventArgs e)
        {
            // Setting the display state will result in any needed actions.
            this.DisplayState = DisplayStates.Hidden;
        }

        protected virtual void Storyboard_ArrivedOpened(object sender, EventArgs e)
        {
            // Setting the display state will result in any needed actions.
            this.DisplayState = DisplayStates.Opened;
        }

        private void stayOpenTimer_Elapsed(Object sender, EventArgs args)
        {
            // Stop the timer because this should not be an ongoing event.
            this.stayOpenTimer.Stop();

            if (!this.IsMouseOver)
            {
                // Only start closing the window if the mouse is not over it.
                this.DisplayState = DisplayStates.Hiding;
            }
        }

        protected override void OnMouseEnter(System.Windows.Input.MouseEventArgs e)
        {
            if (this.DisplayState == DisplayStates.Opened)
            {
                // When the user mouses over and the window is already open, it should stay open.
                // Stop the timer that would have otherwise hidden it.
                this.stayOpenTimer.Stop();
            }
            else if ((this.DisplayState == DisplayStates.Hidden) ||
                     (this.DisplayState == DisplayStates.Hiding))
            {
                // When the user mouses over and the window is hidden or hiding, it should open. 
                this.DisplayState = DisplayStates.Opening;
            }

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(System.Windows.Input.MouseEventArgs e)
        {
            if (this.DisplayState == DisplayStates.Opened)
            {
                // When the user moves the mouse out of the window, the timer to hide the window
                // should be started.
                this.stayOpenTimer.Stop();
                this.stayOpenTimer.Start();
            }

            base.OnMouseEnter(e);
        }

        public void Notify()
        {
            if (this.DisplayState == DisplayStates.Opened)
            {
                // The window is already open, and should now remain open for another count.
                this.stayOpenTimer.Stop();
                this.stayOpenTimer.Start();
            }
            else
            {
                this.DisplayState = DisplayStates.Opening;
            }
        }

        /// <summary>
        /// Force the window to immediately move to the hidden state.
        /// </summary>
        public void ForceHidden()
        {
            this.DisplayState = DisplayStates.Hidden;
        }

        #region <<< INotifyPropertyChanged Members >>>

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }
}
