// -----------------------------------------------------------------------
// <copyright file="MetroNumberLabel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

///Animation was originally using WPF animations. It was throwing errors with no workaround found so I decided to use a Timer.
namespace MangaEplision.Metro
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using System.Timers;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MetroNumberLabel : Control
    {
        static MetroNumberLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroNumberLabel), new FrameworkPropertyMetadata(typeof(MetroNumberLabel)));
        }
        public MetroNumberLabel()
            : base()
        {
            this.Loaded += new RoutedEventHandler(MetroNumberLabel_Loaded);
            this.Unloaded += new RoutedEventHandler(MetroNumberLabel_Unloaded);

            CountupTimer = new Timer();
            CountupTimer.Interval = 0.1;
            CountupTimer.Elapsed += new ElapsedEventHandler(CountupTimer_Elapsed);

            //CountupAnimation = new Int32Animation(-1,Number,new Duration(TimeSpan.FromSeconds(5)));
            /*this.BeginAnimation(ActualNumberProperty, ani);
            CountupStoryboard = new Storyboard();
            CountupStoryboard.Duration = new Duration(TimeSpan.FromSeconds(5));
            CountupStoryboard.BeginTime = TimeSpan.FromSeconds(0); */


        }
        
        void CountupTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(new EmptyDelegate(() =>
                {
                    if (ActualNumber != Number)
                    {
                        var diff = Math.Abs(Number - ActualNumber);

                        if (diff > (0.01) * Number)
                        {
                            var mag = Math.Floor(Math.Sqrt(Number) / 2);
                            if (Number > ActualNumber)
                                ActualNumber += (int)mag;
                            else if (Number < ActualNumber)
                                ActualNumber -= (int)mag;
                        }
                        else
                        {
                            if (Number > ActualNumber)
                                ActualNumber += 1;
                            else if (Number < ActualNumber)
                                ActualNumber -= 1;
                        }

                        if (ActualNumberUpdate != null)
                            ActualNumberUpdate();
                    }
                    else
                    {
                        CountupTimer.Stop();
                    }
                }));
            }
            catch (Exception) { }
        }
        private Timer CountupTimer = null;
        private Int32Animation CountupAnimation = null;
        private Storyboard CountupStoryboard = null;
        void MetroNumberLabel_Unloaded(object sender, RoutedEventArgs e)
        {
            CountupTimer.Stop();
        }

        void MetroNumberLabel_Loaded(object sender, RoutedEventArgs e)
        {
            ContentControl = this.Template.FindName("PART_ContentControl", this) as ContentPresenter;
            //Number = 10000;

            if (Number != ActualNumber)
                CountupTimer.Start();
        }
        ~MetroNumberLabel()
        {
            CountupTimer.Elapsed -= new ElapsedEventHandler(CountupTimer_Elapsed);
            CountupTimer.Stop();
            CountupTimer.Dispose();

            this.Loaded -= new RoutedEventHandler(MetroNumberLabel_Loaded);
            try
            {
                this.Unloaded -= new RoutedEventHandler(MetroNumberLabel_Unloaded);
            }
            catch (Exception) { }
        }
        private ContentPresenter ContentControl = null;
        private bool Resetting = false;
        public void Reset()
        {
            Resetting = true;
            Number = 0;
            ActualNumber = 0;
            Resetting = false;
        }
        public int Number
        {
            get { return (int)this.GetValue(NumberProperty); }
            set
            {
                this.SetValue(NumberProperty, value);
                if (this.IsLoaded)
                    this.RaiseEvent(new RoutedEventArgs(NumberUpdatedEvent, this));
                //this.BeginAnimation(ActualNumberProperty, CountupAnimation);
                if (CountupTimer.Enabled == false && Resetting == false && CountingEnabled == true)
                    CountupTimer.Start();
                else if (CountingEnabled == false)
                    ActualNumber = value;
            }
        }
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(MetroNumberLabel), new PropertyMetadata(0));

        public static readonly RoutedEvent NumberUpdatedEvent = EventManager.RegisterRoutedEvent("NumberUpdated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroNumberLabel));
        public event RoutedEventHandler NumberUpdated
        {
            add { AddHandler(NumberUpdatedEvent, value); }
            remove { RemoveHandler(NumberUpdatedEvent, value); }
        }

        public int ActualNumber
        {
            get { return (int)this.GetValue(ActualNumberProperty); }
            internal set
            {
                this.SetValue(ActualNumberProperty, value);
            }
        }
        public static readonly DependencyProperty ActualNumberProperty = DependencyProperty.Register("ActualNumber", typeof(int), typeof(MetroNumberLabel), new PropertyMetadata(0));

        public bool CountingEnabled
        {
            get
            {
                return (bool)this.GetValue(CountingEnabledProperty);
            }
            set { this.SetValue(CountingEnabledProperty, value); }
        }
        public static readonly DependencyProperty CountingEnabledProperty = DependencyProperty.Register("CountingEnabled", typeof(bool), typeof(MetroNumberLabel), new PropertyMetadata(true));

        public bool Counting
        {
            get { return CountupTimer.Enabled; }
        }

        public event EmptyDelegate ActualNumberUpdate;
        public delegate void EmptyDelegate();
    }
}
