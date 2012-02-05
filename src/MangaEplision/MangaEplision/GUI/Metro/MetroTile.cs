using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Timers;

namespace MangaEplision.Metro
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AppStoreConcept"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AppStoreConcept;assembly=AppStoreConcept"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MetroTile/>
    ///
    /// </summary>
    public class MetroTile : Control
    {
        public MetroTile()
        {
            base.MouseLeftButtonUp += new MouseButtonEventHandler(MetroButton_MouseLeftButtonUp);
            base.SizeChanged += new SizeChangedEventHandler(MetroTile_SizeChanged);



        }


        void MetroTile_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            AniHeight = ActualHeight + 20;
            AniWidth = ActualWidth + 20;
        }
        void MetroButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }
        ~MetroTile()
        {
            
            base.SizeChanged -= new SizeChangedEventHandler(MetroTile_SizeChanged);
            base.MouseLeftButtonUp -= new MouseButtonEventHandler(MetroButton_MouseLeftButtonUp);
        }
        static MetroTile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroTile), new FrameworkPropertyMetadata(typeof(MetroTile)));
        }
        public int Number
        {
            get { return (int)this.GetValue(NumberProperty); }
            set { this.SetValue(NumberProperty, value); }
        }
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
          "Number", typeof(int), typeof(MetroTile), new PropertyMetadata(0));
        public object Icon
        {
            get { return (object)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
          "Icon", typeof(object), typeof(MetroTile), new PropertyMetadata(null));
        public bool IsEffectsEnabled
        {
            get { return (bool)this.GetValue(IsEffectsEnabledProperty); }
            set { this.SetValue(IsEffectsEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsEffectsEnabledProperty = DependencyProperty.Register(
          "IsEffectsEnabled", typeof(bool), typeof(MetroTile), new PropertyMetadata(true));
        public bool IsNumbersEnabled
        {
            get { return (bool)this.GetValue(IsNumbersEnabledProperty); }
            set { this.SetValue(IsNumbersEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsNumbersEnabledProperty = DependencyProperty.Register(
          "IsNumbersEnabled", typeof(bool), typeof(MetroTile), new PropertyMetadata(true));
        public string Header
        {
            get { return (string)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
          "Header", typeof(string), typeof(MetroTile), new PropertyMetadata("Header"));
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroTile));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        public double AniHeight
        {
            get
            {
                return (double)Dispatcher.Invoke(
                    new EmptyReturnDelegate(() =>
                  {
                      return this.GetValue(AniHeightProperty) == null ? 0 : this.GetValue(AniHeightProperty);
                  }));
            }
            set { this.SetValue(AniHeightProperty, value); }
        }
        public static readonly DependencyProperty AniHeightProperty = DependencyProperty.Register(
          "AniHeight", typeof(double), typeof(MetroTile));

        public double AniWidth
        {
            get
            {
                return (double)Dispatcher.Invoke(
                    new EmptyDelegate(() => this.GetValue(AniWidthProperty)));
            }
            set { this.SetValue(AniWidthProperty, value); }
        }
        public static readonly DependencyProperty AniWidthProperty = DependencyProperty.Register(
          "AniWidth", typeof(double), typeof(MetroTile));
    }
}
