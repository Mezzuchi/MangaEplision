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
    ///     <MyNamespace:MetroCircleButton/>
    ///
    /// </summary>
    public class MetroCircleButton : ContentControl
    {
        static MetroCircleButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetroCircleButton), new FrameworkPropertyMetadata(typeof(MetroCircleButton)));
        }
        public MetroCircleButton()
        {
            this.Loaded += new RoutedEventHandler(MetroCircleButton_Loaded);
            this.Unloaded += new RoutedEventHandler(MetroCircleButton_Unloaded);
        }

        void MetroCircleButton_Unloaded(object sender, RoutedEventArgs e)
        {
            base.MouseLeftButtonDown -= new MouseButtonEventHandler(MetroCircleButton_MouseLeftButtonDown);
            this.Loaded -= new RoutedEventHandler(MetroCircleButton_Loaded);
            this.Unloaded -= new RoutedEventHandler(MetroCircleButton_Unloaded);
        }

        void MetroCircleButton_Loaded(object sender, RoutedEventArgs e)
        {
            base.MouseLeftButtonDown += new MouseButtonEventHandler(MetroCircleButton_MouseLeftButtonDown);
        }
        public void MetroCircleButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }
        static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MetroCircleButton));

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
    }
}
